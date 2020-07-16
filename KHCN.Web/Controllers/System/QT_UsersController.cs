using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KHCN.Web.Controllers
{
    [CustomAuthorize]
    public class QT_UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        public QT_UsersController(IUserRepository userRepository, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public IActionResult Index()
        {
            var data = _userRepository.GetAll().Where(p => p.IsActive).ToList();
            return View(data);
        }

        public IActionResult AddOrUpdate(int? id)
        {
            var obj = new CMS_User();

            if (id == null || id <= 0)
            {
                ViewBag.Title = "Thêm mới người dùng";
                return View(obj);
            }
            else
            {
                ViewBag.Title = "Cập nhật người dùng";
                var model = _userRepository.Get(id.Value);
                model.Password = "******************";
                if (model == null)
                    return File("~/wwwroot/html/Page404.html", "text/html");
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(CMS_User obj)
        {
            try
            {
                if (obj.Id <= 0)
                    ViewBag.Title = "Thêm mới người dùng";
                else
                    ViewBag.Title = "Cập nhật người dùng";

                if (ModelState.IsValid)
                {
                    obj.Password = PasswordHasher.GenerateIdentityV3Hash(obj.Password);
                    var checkExist = _userRepository.GetByUsername(obj.UserName);
                    if (checkExist != null && checkExist.Id != obj.Id)
                    {
                        log.Error("Đã tồn tại người dùng có username = " + obj.UserName);
                        SetError("Cập nhật dữ liệu không thành công. Username đã tồn tại!");
                        return View(obj);
                    }

                    if (obj.Id > 0)
                    {
                        var model = _userRepository.Get(obj.Id);
                        if (model == null)
                        {
                            SetError($"Không tìm thấy user có Id = {obj.Id}");
                            return RedirectToAction("Index", "QT_Users");
                        }

                        obj.CreatedBy = model.CreatedBy;
                        obj.CreatedDate = model.CreatedDate;
                        obj.UpdatedBy = "host";
                        obj.UpdatedDate = DateTime.Now;

                        _userRepository.Update(obj, obj.Id);
                        SetSuccess("Cập nhật người dùng thành công");
                        return RedirectToAction("Index", "QT_Users");
                    }
                    else
                    {
                        obj.IsActive = true;
                        obj.CreatedBy = obj.UpdatedBy = "host";
                        obj.CreatedDate = obj.UpdatedDate = DateTime.Now;

                        if (_userRepository.Add(obj).Id > 0)
                        {
                            SetSuccess("Thêm người dùng thành công");
                            return RedirectToAction("Index", "QT_Users");
                        }
                        else
                        {
                            SetError("Thêm người dùng không thành công");
                            log.Error("Thêm người dùng không thành công username = " + obj.UserName);
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
                    var user = _userRepository.Get(id);
                    if (user != null)
                    {
                        var exist = _userRoleRepository.GetByIdUser(user.Id);
                        if (!exist.Any())
                        {
                            _userRepository.Delete(user);
                            return Json(new { status = true, message = "Xóa dữ liệu thành công!" });
                        }
                        else
                            return Json(new { status = false, message = "Lỗi: Không thể xóa do người dùng có ràng buộc dữ liệu!" });
                    }
                    else
                        return Json(new { status = false, message = "Lỗi: Không tìm thấy người dùng có Id = " + id });
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