using KHCN.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Helpers
{
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Claims.Any())
            {
                var roleFunctionRepository = context.HttpContext.RequestServices.GetService(typeof(IRoleFunctionRepository)) as IRoleFunctionRepository;
                var functionRepository = context.HttpContext.RequestServices.GetService(typeof(IFunctionRepository)) as IFunctionRepository;
                var userRepository = context.HttpContext.RequestServices.GetService(typeof(IUserRepository)) as IUserRepository;
                var userRoleRepository = context.HttpContext.RequestServices.GetService(typeof(IUserRoleRepository)) as IUserRoleRepository;

                var currentUsername = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserName").Value;
                if (!string.IsNullOrEmpty(currentUsername))
                {
                    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                    var actionName = descriptor.ActionName;
                    var controllerName = descriptor.ControllerName;

                    var userInfo = userRepository.GetByUsername(currentUsername);
                    if (userInfo != null && userInfo.Id > 0)
                    {
                        var userRoleInfo = userRoleRepository.GetByIdUser(userInfo.Id);
                        if (userRoleInfo != null && userRoleInfo.Select(m => m.IdRole).Any())
                        {
                            var lstRole = userRoleInfo.Select(m => m.IdRole).ToList();
                            string controllerAction = $"{controllerName}-{actionName}";

                            var allRoleFunc = roleFunctionRepository.GetAll().ToList();
                            var allFunc = functionRepository.GetAll().Where(p => p.IsActive).ToList();

                            var lstRoleFunction = (from a in allRoleFunc
                                                   join b in allFunc on a.IdFunction equals b.Id
                                                   where lstRole.Contains(a.IdRole)
                                                   select new { ca = b.ControllerAction.ToLower() }).ToList();

                            var permission = lstRoleFunction.Where(x => x.ca.Contains(controllerAction, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (permission != null)
                            {
                                return; //User Authorized. Wihtout setting any result value and just returning is sufficent for authorizing user
                            }
                            else
                            {
                                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                                    { "Controller", "Home" },
                                    { "Action", "Privacy" }
                               });

                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                        { "Controller", "Account" },
                        { "Action", "Login" }
                   });

                return;
            }

            context.Result = new UnauthorizedResult();
            return;
        }

        public void OnAuthorization_Backup(AuthorizationFilterContext context)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerName;

            var lstRole = context.HttpContext.User.FindFirst("Role")?.Value;
            string controllerAction = $"{controllerName}-{actionName}";

            var roleFunctionRepository = context.HttpContext.RequestServices.GetService(typeof(IRoleFunctionRepository)) as IRoleFunctionRepository;
            var functionRepository = context.HttpContext.RequestServices.GetService(typeof(IFunctionRepository)) as IFunctionRepository;

            var lstRoleFunction = (from a in roleFunctionRepository.GetAll()
                                   join b in functionRepository.GetAll().Where(p => p.IsActive) on a.IdFunction equals b.Id
                                   where lstRole.Contains(a.IdRole.ToString())
                                   select new { ca = b.ControllerAction.ToLower() }).ToList();

            var permission = lstRoleFunction.Where(x => x.ca.Contains(controllerAction, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (permission != null)
            {
                return; //User Authorized. Wihtout setting any result value and just returning is sufficent for authorizing user
            }
            else
                context.Result = new ForbidResult();

            context.Result = new UnauthorizedResult();
            return;
        }
    }
}