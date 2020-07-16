using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using KHCN.Data.Interfaces;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Web.Controllers
{
    [CustomAuthorize]
    public class QT_RoleManagerController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IModuleRepository _moduleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IApiRepository _apiRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IRoleFunctionRepository _rolefunctionRepository;
        private readonly IRoleApiRepository _roleapiRepository;
        private readonly IRolePageRepository _rolepageRepository;

        public QT_RoleManagerController(IUnitOfWork unitOfWork, IModuleRepository moduleRepository, IRoleRepository roleRepository, IFunctionRepository functionRepository,
            IPageRepository pageRepository, IRoleFunctionRepository rolefunctionRepository, IRolePageRepository rolepageRepository, IUserRepository userRepository, IUserRoleRepository userRoleRepository
            , IApiRepository apiRepository, IRoleApiRepository roleapiRepository)
        {
            _unitOfWork = unitOfWork;
            _moduleRepository = moduleRepository;
            _roleRepository = roleRepository;
            _functionRepository = functionRepository;
            _pageRepository = pageRepository;
            _rolefunctionRepository = rolefunctionRepository;
            _rolepageRepository = rolepageRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _apiRepository = apiRepository;
            _roleapiRepository = roleapiRepository;
        }

        public IActionResult RoleFunctionPage()
        {
            try
            {
                ViewBag.ROLEDATA = _roleRepository.GetAll().Where(p => p.IsActive).ToList();
                ViewBag.FUNCTIONDATA = _functionRepository.GetAll().Where(p => p.IsActive).ToList();
                return View();
            }
            catch (Exception ex)
            {
                SetError("Lỗi" + ex.Message.ToString());
                log.Error(ex);
                return View();
            }
        }

        public IActionResult RoleApi()
        {
            try
            {
                ViewBag.ROLEDATA = _roleRepository.GetAll().Where(p => p.IsActive).ToList();
                ViewBag.APIDATA = _apiRepository.GetAll().Where(p => p.IsActive).ToList();
                return View();
            }
            catch (Exception ex)
            {
                SetError("Lỗi" + ex.Message.ToString());
                log.Error(ex);
                return View();
            }
        }

        public IActionResult RoleUser()
        {
            try
            {
                ViewBag.ROLEDATA = _roleRepository.GetAll().Where(p => p.IsActive).ToList();
                ViewBag.USERDATA = _userRepository.GetAll().Where(p => p.IsActive).ToList();
                return View();
            }
            catch (Exception ex)
            {
                SetError("Lỗi" + ex.Message.ToString());
                log.Error(ex);
                return View();
            }
        }

        [HttpPost]
        public JsonResult GetDataPermission()
        {
            try
            {
                var lRole = from a in _roleRepository.GetAll().ToList() select new { a.Id, a.Name };

                var lFunc = from a in _functionRepository.GetAll().ToList()
                            join b in _moduleRepository.GetAll().ToList() on a.IdModule equals b.Id
                            where b.IsActive
                            orderby a.IdModule
                            select new { a.Id, a.Name, a.IdModule, ModuleName = b.Name };

                var lPage = from a in _pageRepository.GetAll().ToList()
                            join b in _moduleRepository.GetAll().ToList() on a.IdModule equals b.Id
                            where b.IsActive
                            orderby a.IdModule
                            where a.IsActive == true
                            select new { a.Id, a.Name, a.IdModule, ModuleName = b.Name };

                var lModule = from a in _moduleRepository.GetAll().ToList() where a.IsActive orderby a.Name select new { a.Id, a.Name };
                var result = new { state = true, lRole, lFunc, lPage, lModule };

                return Json(new { state = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex });
            }
        }

        [HttpPost]
        public JsonResult GetDataPermissionApi()
        {
            try
            {
                var lRole = from a in _roleRepository.GetAll().ToList() select new { a.Id, a.Name };

                var lFunc = from a in _apiRepository.GetAll().ToList()
                            join b in _moduleRepository.GetAll().ToList() on a.IdModule equals b.Id
                            where b.IsActive
                            orderby a.IdModule
                            select new { a.Id, a.Name, a.IdModule, ModuleName = b.Name };

                var lModule = from a in _moduleRepository.GetAll().ToList() where a.IsActive orderby a.Name select new { a.Id, a.Name };
                var result = new { state = true, lRole, lFunc, lModule };

                return Json(new { state = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex });
            }
        }

        [HttpPost]
        public JsonResult GetPermissionByRole(string idRole, string idModule, string isadmin)
        {
            try
            {
                int roleId = Convert.ToInt32(idRole);
                int moduleId = Convert.ToInt32(idModule);
                bool isAdmin = Convert.ToBoolean(isadmin);

                List<int> lFuncByRole = _functionRepository.GetAll()
                    .Join(_moduleRepository.GetAll(),
                        fc => fc.IdModule,
                        md => md.Id,
                        (fc, md) => new { CMS_Function = fc, CMS_Module = md })
                    .Where(md => md.CMS_Module.IsActive)
                    .Select(mfc => mfc.CMS_Function)
                    .Join(_rolefunctionRepository.GetAll(),
                        mfc => mfc.Id,
                        rfc => rfc.IdFunction,
                        (mfc, rfc) => new { CMS_Function = mfc, CMS_RoleFunction = rfc })
                    .Select(fr => fr.CMS_RoleFunction)
                    .Where(x => x.IdRole == roleId)
                    .Select(x => (int)x.IdFunction)
                    .ToList();

                List<int> lPageByRole = _pageRepository.GetAll()
                    .Join(_moduleRepository.GetAll(),
                        pg => pg.IdModule,
                        md => md.Id,
                        (pg, md) => new { CMS_Page = pg, CMS_Modules = md })
                    .Where(md => md.CMS_Modules.IsActive)
                    .Select(mpg => mpg.CMS_Page)
                    .Join(_rolepageRepository.GetAll(),
                        pg => pg.Id,
                        pr => pr.IdPage,
                        (pg, pr) => new { CMS_Page = pg, CMS_PageRole = pr })
                    .Select(pgr => pgr.CMS_PageRole)
                    .Where(x => x.IdRole == roleId).Select(x => (int)x.IdPage).ToList();

                #region jsTreePage

                List<JSTree> jsTreeList = new List<JSTree>();
                JSTree objTree = new JSTree();
                JSTreeState state;
                var dataAll = _pageRepository.GetAll()
                    .Join(_moduleRepository.GetAll(),
                        pg => pg.IdModule,
                        md => md.Id,
                        (pg, md) => new { CMS_Page = pg, CMS_Modules = md })
                    .Where(md => md.CMS_Modules.IsActive)
                    .Select(pgmd => pgmd.CMS_Page)
                    .Where(x => x.IsAdmin == isAdmin && x.IsActive).OrderBy(x => x.OrderHint).ToList();
                var data = dataAll.Where(x => x.IdParent == 0);

                foreach (var item in data)
                {
                    objTree = new JSTree();
                    state = new JSTreeState();
                    objTree.id = item.Id;
                    objTree.text = item.Name;
                    state.disabled = true;
                    if (lPageByRole.Contains(item.Id))
                    {
                        state.selected = true;
                    }
                    objTree.children = getChildPage(item.Id, dataAll, lPageByRole);
                    objTree.state = state;
                    jsTreeList.Add(objTree);
                }

                #endregion jsTreePage

                #region jsTreeFunction

                List<JSTree> jsTreeListF = new List<JSTree>();
                JSTree objTreeF = new JSTree();
                JSTreeState stateF;

                var dataAllF = _functionRepository.GetAll()
                    .Join(_moduleRepository.GetAll(),
                        fc => fc.IdModule,
                        md => md.Id,
                        (fc, md) => new { CMS_Function = fc, CMS_Modules = md })
                    .Where(md => md.CMS_Modules.IsActive)
                    .Select(fc => fc.CMS_Function)
                    .Where(x => x.IdModule == moduleId).ToList();
                var dataF = dataAllF.Where(x => x.IdParent == 0 || x.IdParent == null);

                foreach (var item in dataF)
                {
                    objTreeF = new JSTree();
                    stateF = new JSTreeState();
                    objTreeF.id = item.Id;
                    objTreeF.text = item.Name;
                    stateF.disabled = true;
                    if (lFuncByRole.Contains(item.Id))
                    {
                        stateF.selected = true;
                    }
                    objTreeF.children = getChildFunction(item.Id, dataAllF, lFuncByRole);
                    objTreeF.state = stateF;
                    jsTreeListF.Add(objTreeF);
                }

                #endregion jsTreeFunction

                var result = new { lFuncByRole, lPageByRole, jsTreeList, jsTreeListF, state = true };

                return Json(new { state = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex });
            }
        }

        [HttpPost]
        public JsonResult GetPermissionApiByRole(string idRole, string idModule)
        {
            try
            {
                int roleId = Convert.ToInt32(idRole);
                int moduleId = Convert.ToInt32(idModule);

                List<int> lFuncByRole = _apiRepository.GetAll()
                    .Join(_moduleRepository.GetAll(),
                        fc => fc.IdModule,
                        md => md.Id,
                        (fc, md) => new { CMS_Api = fc, CMS_Module = md })
                    .Where(md => md.CMS_Module.IsActive)
                    .Select(mfc => mfc.CMS_Api)
                    .Join(_roleapiRepository.GetAll(),
                        mfc => mfc.Id,
                        rfc => rfc.IdApi,
                        (mfc, rfc) => new { CMS_Api = mfc, CMS_RoleApi = rfc })
                    .Select(fr => fr.CMS_RoleApi)
                    .Where(x => x.IdRole == roleId)
                    .Select(x => (int)x.IdApi)
                    .ToList();

                #region jsTree Api

                List<JSTree> jsTreeListF = new List<JSTree>();
                JSTree objTreeF = new JSTree();
                JSTreeState stateF;

                var dataAllF = _apiRepository.GetAll()
                    .Join(_moduleRepository.GetAll(),
                        fc => fc.IdModule,
                        md => md.Id,
                        (fc, md) => new { CMS_Api = fc, CMS_Modules = md })
                    .Where(md => md.CMS_Modules.IsActive)
                    .Select(fc => fc.CMS_Api)
                    .Where(x => x.IdModule == moduleId).ToList();
                var dataF = dataAllF.Where(x => x.IdParent == 0 || x.IdParent == null);

                foreach (var item in dataF)
                {
                    objTreeF = new JSTree();
                    stateF = new JSTreeState();
                    objTreeF.id = item.Id;
                    objTreeF.text = item.Name;
                    stateF.disabled = true;
                    if (lFuncByRole.Contains(item.Id))
                    {
                        stateF.selected = true;
                    }
                    objTreeF.children = getChildApi(item.Id, dataAllF, lFuncByRole);
                    objTreeF.state = stateF;
                    jsTreeListF.Add(objTreeF);
                }

                #endregion jsTree Api

                var result = new { lFuncByRole, jsTreeListF, state = true };

                return Json(new { state = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex });
            }
        }

        [NonAction]
        public List<JSTree> getChildPage(int ParentID, List<CMS_Page> dataAll, List<int> lPageByRole)
        {
            try
            {
                List<JSTree> jsTreeList = new List<JSTree>();
                JSTree objTree;
                JSTreeState state;
                var data = dataAll.Where(x => x.IdParent == ParentID);
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        objTree = new JSTree();
                        state = new JSTreeState();
                        objTree.id = item.Id;
                        objTree.text = item.Name;
                        if (lPageByRole.Contains(item.Id))
                        {
                            state.selected = true;
                        }
                        objTree.children = getChildPage(item.Id, dataAll, lPageByRole);
                        objTree.state = state;
                        jsTreeList.Add(objTree);
                    }
                }
                return jsTreeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public List<JSTree> getChildFunction(int ParentID, List<CMS_Function> dataAll, List<int> lFuncByRole)
        {
            try
            {
                List<JSTree> jsTreeList = new List<JSTree>();
                JSTree objTree;
                JSTreeState state;
                var data = dataAll.Where(x => x.IdParent == ParentID);
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        objTree = new JSTree();
                        state = new JSTreeState();
                        objTree.id = item.Id;
                        objTree.text = item.Name;
                        if (lFuncByRole.Contains(item.Id))
                        {
                            state.selected = true;
                        }
                        objTree.children = getChildFunction(item.Id, dataAll, lFuncByRole);
                        objTree.state = state;
                        jsTreeList.Add(objTree);
                    }
                }
                return jsTreeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public List<JSTree> getChildApi(int ParentID, List<CMS_Api> dataAll, List<int> lFuncByRole)
        {
            try
            {
                List<JSTree> jsTreeList = new List<JSTree>();
                JSTree objTree;
                JSTreeState state;
                var data = dataAll.Where(x => x.IdParent == ParentID);
                if (data.Count() > 0)
                {
                    foreach (var item in data)
                    {
                        objTree = new JSTree();
                        state = new JSTreeState();
                        objTree.id = item.Id;
                        objTree.text = item.Name;
                        if (lFuncByRole.Contains(item.Id))
                        {
                            state.selected = true;
                        }
                        objTree.children = getChildApi(item.Id, dataAll, lFuncByRole);
                        objTree.state = state;
                        jsTreeList.Add(objTree);
                    }
                }
                return jsTreeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult SaveRoleFunction(string idRole, string idModule, string idsFunction)
        {
            try
            {
                int roleId = Convert.ToInt32(idRole);
                int moduleId = Convert.ToInt32(idModule);
                var lstFunction = string.IsNullOrEmpty(idsFunction) ? new List<int>() : idsFunction.Split(',').Select(Int32.Parse).ToList();
                if (lstFunction != null && lstFunction.Any())
                {
                    var now = DateTime.Now;
                    var lstRoleFunction = lstFunction.Select(m => new CMS_RoleFunction
                    {
                        IdRole = roleId,
                        IdFunction = m,
                        CreatedBy = "system",
                        UpdatedBy = "system",
                        CreatedDate = now,
                        UpdatedDate = now,
                    }).ToList();

                    _unitOfWork.Context.Set<CMS_RoleFunction>().AddRange(lstRoleFunction);
                    _unitOfWork.Commit();

                    return Json(new { state = true, message = "Gán quyền chức năng thành công" });
                }

                return Json(new { state = false, message = "Lỗi gán quyền chức năng" });
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex });
            }
        }

        [HttpPost]
        public JsonResult SaveRoleApi(string idRole, string idModule, string idsApi)
        {
            try
            {
                int roleId = Convert.ToInt32(idRole);
                int moduleId = Convert.ToInt32(idModule);
                var lstApi = string.IsNullOrEmpty(idsApi) ? new List<int>() : idsApi.Split(',').Select(Int32.Parse).ToList();
                if (lstApi != null && lstApi.Any())
                {
                    var now = DateTime.Now;
                    var lstRoleApi = lstApi.Select(m => new CMS_RoleApi
                    {
                        IdRole = roleId,
                        IdApi = m,
                        CreatedBy = "host",
                        UpdatedBy = "host",
                        CreatedDate = now,
                        UpdatedDate = now,
                    }).ToList();

                    _unitOfWork.Context.Set<CMS_RoleApi>().AddRange(lstRoleApi);
                    _unitOfWork.Commit();

                    return Json(new { state = true, message = "Gán quyền api thành công" });
                }

                return Json(new { state = false, message = "Lỗi gán quyền api" });
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex });
            }
        }

        [HttpPost]
        public JsonResult SaveRolePage(string idRole, string idsPage, string isadmin)
        {
            try
            {
                int roleId = Convert.ToInt32(idRole);
                bool isAdmin = Convert.ToBoolean(isadmin);
                var lstPage = string.IsNullOrEmpty(idsPage) ? new List<int>() : idsPage.Split(',').Select(Int32.Parse).ToList();
                if (lstPage != null && lstPage.Any())
                {
                    var now = DateTime.Now;
                    var lstRolePage = lstPage.Select(m => new CMS_RolePage
                    {
                        IdRole = roleId,
                        IdPage = m,
                        CreatedBy = "system",
                        UpdatedBy = "system",
                        CreatedDate = now,
                        UpdatedDate = now,
                    }).ToList();

                    _unitOfWork.Context.Set<CMS_RolePage>().AddRange(lstRolePage);
                    _unitOfWork.Commit();

                    return Json(new { state = true, message = "Gán quyền page thành công" });
                }

                return Json(new { state = false, message = "Lỗi gán quyền page" });
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex });
            }
        }

        [HttpPost]
        public JsonResult GetUsersGroup(string Name, string RoleID)
        {
            try
            {
                int intRoleID = Convert.ToInt32(RoleID);
                var lstUser = _userRepository.GetAll().Where(p => p.IsActive).ToList();
                var lstUserIdByRole = _userRoleRepository.GetByIdRole(intRoleID).Select(m => m.IdUser).ToList();
                var listUserInGroup = lstUser.Where(m => lstUserIdByRole.Contains(m.Id)).ToList();
                var listUserNotInGroup = lstUser.Except(listUserInGroup).ToList();

                if (!string.IsNullOrEmpty(Name))
                {
                    var temp = listUserNotInGroup.Where(m => m.FullName.Contains(Name, StringComparison.OrdinalIgnoreCase) || m.Email == Name || m.UserName.Equals(Name, StringComparison.OrdinalIgnoreCase)).ToList();
                    listUserNotInGroup = temp;
                }

                var jsonResults = new { listUserNotInGroup = listUserNotInGroup, listUserInGroup = listUserInGroup, status = true };
                return Json(new { status = true, data = jsonResults });
            }
            catch (Exception ex)
            {
                SetError("Lỗi" + ex.Message.ToString());
                log.Error(ex);
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetUsersInGroup(string[] arrChecked, string RoleID)
        {
            try
            {
                int roleId = Convert.ToInt32(RoleID);
                var lstUserID = arrChecked == null || arrChecked.Count() == 0 ? new List<int>() : arrChecked.Select(Int32.Parse).ToList();
                if (lstUserID != null && lstUserID.Any())
                {
                    var now = DateTime.Now;
                    var lstUserRole = lstUserID.Select(m => new CMS_UserRole
                    {
                        IdRole = roleId,
                        IdUser = m,
                        CreatedBy = "system",
                        UpdatedBy = "system",
                        CreatedDate = now,
                        UpdatedDate = now,
                    }).ToList();

                    _unitOfWork.Context.Set<CMS_UserRole>().AddRange(lstUserRole);
                    _unitOfWork.Commit();

                    return Json(new { state = true, message = "Gán nhóm người dùng thành công" });
                }

                return Json(new { state = false, message = "Lỗi gán nhóm người dùng" });
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex });
            }
        }

        [HttpPost]
        public JsonResult SetUsersOutGroup(string[] arrChecked, string RoleID)
        {
            try
            {
                int roleId = Convert.ToInt32(RoleID);
                var lstUserID = arrChecked.Select(Int32.Parse).ToList();
                var lstUserRole = _userRoleRepository.GetByIdUserIdRole(lstUserID, new List<int> { roleId });
                if (lstUserRole != null && lstUserRole.Any())
                {
                    _userRoleRepository.Delete(lstUserRole.ToList());
                    return Json(new { state = true, message = "Bỏ gán nhóm người dùng thành công" });
                }

                return Json(new { state = false, message = "Không tìm thấy thông tin người dùng trong nhóm" });
            }
            catch (Exception ex)
            {
                return Json(new { state = false, message = ex });
            }
        }
    }
}