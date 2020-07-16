using KHCN.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Helpers
{
    public class ApiAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Claims.Any())
            {
                var roleApiRepository = context.HttpContext.RequestServices.GetService(typeof(IRoleApiRepository)) as IRoleApiRepository;
                var apiRepository = context.HttpContext.RequestServices.GetService(typeof(IApiRepository)) as IApiRepository;
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

                            var allRoleFunc = roleApiRepository.GetAll().ToList();
                            var allFunc = apiRepository.GetAll().Where(p => p.IsActive).ToList();

                            var lstRoleFunction = (from a in allRoleFunc
                                                   join b in allFunc on a.IdApi equals b.Id
                                                   where lstRole.Contains(a.IdRole)
                                                   select new { ca = b.ControllerAction.ToLower() }).ToList();

                            var permission = lstRoleFunction.Where(x => x.ca.Contains(controllerAction, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                            if (permission != null)
                            {
                                return; //User Authorized. Wihtout setting any result value and just returning is sufficent for authorizing user
                            }
                            else
                            {
                                context.Result = new ForbidResult("Bạn không có quyền truy cập chức năng này!");
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}