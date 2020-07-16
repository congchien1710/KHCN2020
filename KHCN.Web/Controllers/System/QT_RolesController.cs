using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KHCN.Web.Controllers
{
    [CustomAuthorize]
    public class QT_RolesController : BaseController
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _roleuserRepository;
        private readonly IRoleFunctionRepository _rolefunctionRepository;
        private readonly IRolePageRepository _rolePageRepository;
        public QT_RolesController(IRoleRepository roleRepository, IUserRoleRepository roleuserfunctionRepositor, IRoleFunctionRepository rolefunctionRepository, IRolePageRepository rolePageRepository)
        {
            _roleRepository = roleRepository;
            _rolefunctionRepository = rolefunctionRepository;
            _rolePageRepository = rolePageRepository;
            _roleuserRepository = roleuserfunctionRepositor;
        }

        public IActionResult Index()
        {
            var data = _roleRepository.GetAll().Where(p => p.IsActive).ToList();
            return View(data);
        }

        public IActionResult AddOrUpdate(int? id)
        {
            var obj = new CMS_Role();

            if (id == null || id <= 0)
            {
                ViewBag.Title = "Thêm mới nhóm người dùng";
                return View(obj);
            }
            else
            {
                ViewBag.Title = "Cập nhật nhóm người dùng";
                var model = _roleRepository.Get(id.Value);
                if (model == null)
                    return File("~/wwwroot/html/Page404.html", "text/html");
                return View(obj);
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(CMS_Role obj)
        {
            try
            {
                if (obj.Id == null || obj.Id <= 0)
                    ViewBag.Title = "Thêm mới nhóm người dùng";
                else
                    ViewBag.Title = "Cập nhật nhóm người dùng";

                if (ModelState.IsValid)
                {
                    var checkExist = _roleRepository.GetByName(obj.Name);
                    if (checkExist != null && checkExist.Id != obj.Id)
                    {
                        log.Error("Đã tồn tại nhóm người dùng có tên = " + obj.Name);
                        SetError("Cập nhật dữ liệu không thành công. Tên nhóm người dùng đã tồn tại!");
                        return View(obj);
                    }

                    if (obj.Id > 0)
                    {
                        var model = _roleRepository.Get(obj.Id);
                        if (model == null)
                        {
                            SetError($"Không tìm thấy nhóm người dùng có Id = {obj.Id}");
                            return RedirectToAction("Index", "QT_Roles");
                        }

                        obj.CreatedBy = model.CreatedBy;
                        obj.CreatedDate = model.CreatedDate;
                        obj.UpdatedBy = "host";
                        obj.UpdatedDate = DateTime.Now;

                        _roleRepository.Update(obj, obj.Id);
                        SetSuccess("Cập nhật nhóm người dùng thành công");
                        return RedirectToAction("Index", "QT_Roles");
                    }
                    else
                    {
                        obj.IsActive = true;
                        obj.CreatedBy = obj.UpdatedBy = "host";
                        obj.CreatedDate = obj.UpdatedDate = DateTime.Now;

                        if (_roleRepository.Add(obj).Id > 0)
                        {
                            SetSuccess("Thêm nhóm người dùng thành công");
                            return RedirectToAction("Index", "QT_Roles");
                        }
                        else
                        {
                            SetError("Thêm nhóm người dùng không thành công");
                            log.Error("Thêm nhóm người dùng không thành công tên = " + obj.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SetError("Lỗi" + ex.Message.ToString());
                log.Error(ex);
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id > 0)
                {
                    var role = _roleRepository.Get(id);
                    if (role != null)
                    {
                        var existRoleUser = _roleuserRepository.GetByIdRole(role.Id);
                        var existRoleFunc = _rolefunctionRepository.GetByIdRole(role.Id);
                        var existRolePage = _rolePageRepository.GetByIdRole(role.Id);
                        if (!existRoleUser.Any() && !existRoleFunc.Any() && !existRolePage.Any())
                        {
                            _roleRepository.Delete(role);
                            return Json(new { status = true, message = "Xóa dữ liệu thành công!" });
                        }
                        else
                            return Json(new { status = false, message = "Lỗi: Không thể xóa do nhóm người dùng có ràng buộc dữ liệu!" });
                    }
                    else
                        return Json(new { status = false, message = "Lỗi: Không tìm thấy nhóm người dùng có Id = " + id });
                }

                return Json(new { status = false, message = "Lỗi: Id null" });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { status = false, message = "Lỗi: " + ex.Message });
            }
        }
    }
}