using KHCN.Data.Entities.System;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Web.Views.Shared.Components
{
    public class MenuInfoViewComponent : ViewComponent
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePageRepository _rolePageRepository;

        public MenuInfoViewComponent(IPageRepository pageRepository, IUserRepository userRepository, IUserRoleRepository userRoleRepository, IRolePageRepository rolePageRepository)
        {
            _pageRepository = pageRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _rolePageRepository = rolePageRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUsername = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserName").Value;
            if (!string.IsNullOrEmpty(currentUsername))
            {
                var userInfo = _userRepository.GetByUsername(currentUsername);
                if (userInfo != null && userInfo.Id > 0)
                {
                    var userRoleInfo = _userRoleRepository.GetByIdUser(userInfo.Id);
                    if (userRoleInfo != null && userRoleInfo.Select(m => m.IdRole).Any())
                    {
                        var lstRole = userRoleInfo.Select(m => m.IdRole).ToList();
                        var lstPage = (from a in _rolePageRepository.GetAll().ToList()
                                       join b in _pageRepository.GetAll().Where(p => p.IsActive).ToList() on a.IdPage equals b.Id
                                       where lstRole.Contains(a.IdRole)
                                       select b).ToList();

                        var listAllMenu = lstPage.Distinct().Where(x => x.IsActive).ToList();
                        ViewBag.ViewMenu = listAllMenu.Count() == 0 ? "" : BuildTreeMenu(listAllMenu.ToList());
                    }
                }
            }

            return View();
        }

        private string BuildTreeMenu(List<CMS_Page> listAllMenu)
        {
            try
            {
                string url = new Uri($"{Request.Scheme}://{Request.Host}").ToString();
                url = url.ToLower();
                string stringMenu = "<ul class='nav nav-pills nav-sidebar flex-column' data-widget='treeview' role='menu' data-accordion='false'>";
                var parentMenu = listAllMenu.Where(x => x.IdParent == 0 || x.IdParent == null || x.IdParent == null).OrderBy(m => m.OrderHint).ToList();
                int level = 1;
                foreach (var menu in parentMenu)
                {
                    level = 1;
                    var childMenu = listAllMenu.Where(x => x.IdParent == menu.Id).OrderBy(m => m.OrderHint).ToList();
                    if (childMenu.Any())
                    {
                        stringMenu += "<li class='nav-item has-treeview'>";
                        stringMenu += "<a href='#' class='nav-link'>";
                        stringMenu += menu.Icon;
                        stringMenu += "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "<i class='right fas fa-angle-left'></i></p>";
                        stringMenu += "</a>";
                        stringMenu += GetSubMenu(childMenu, listAllMenu, url, level);
                        stringMenu += "</li>";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(menu.Key))
                            stringMenu += "<li class='nav-item'><a class='nav-link' href='" + url + menu.Key + "'>" + menu.Icon + "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "</p></a></li>";
                        else
                            stringMenu += "<li class='nav-item'><a class='nav-link' style='cursor: pointer'>" + menu.Icon + "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "</p></a></li>";
                    }
                }
                stringMenu += "</ul>";
                return stringMenu;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetSubMenu(List<CMS_Page> childMenu, List<CMS_Page> listAllMenu, string url, int level)
        {
            try
            {
                level++;
                string submenu = "";
                if (level == 2)
                {
                    submenu += "<ul class='nav nav-treeview'>";
                }
                else
                {
                    submenu += "<ul class='nav nav-treeview'>";
                }
                foreach (var menu in childMenu)
                {
                    var xChildPage = listAllMenu.Where(x => x.IdParent == menu.Id).OrderBy(m => m.OrderHint).ToList();
                    if (xChildPage.Any())
                    {
                        submenu += "<li class='nav-item has-treeview'>";
                        submenu += "<a href='#' class='nav-link'>";
                        submenu += menu.Icon;
                        submenu += "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "<i class='right fas fa-angle-left'></i></p>";
                        submenu += "</a>";
                        submenu += GetSubMenu(xChildPage, listAllMenu, url, level);
                        submenu += "</li>";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(menu.Key))
                            submenu += "<li class='nav-item'><a class='nav-link' href='" + url + menu.Key + "'>" + menu.Icon + "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "</p></a></li>";
                        else
                            submenu += "<li class='nav-item'><a class='nav-link' style='cursor: pointer'>" + menu.Icon + "&nbsp;&nbsp;&nbsp;<p>" + menu.Name + "</p></a></li>";
                    }
                }
                submenu += "</ul>";
                return submenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}