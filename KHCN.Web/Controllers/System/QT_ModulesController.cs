using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace KHCN.Web.Controllers
{
    [CustomAuthorize]
    public class QT_ModulesController : BaseController
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly IPageRepository _pageRepository;
        public QT_ModulesController(IModuleRepository moduleRepository, IFunctionRepository functionRepository, IPageRepository pageRepository)
        {
            _moduleRepository = moduleRepository;
            _functionRepository = functionRepository;
            _pageRepository = pageRepository;
        }

        public IActionResult Index()
        {
            var data = _moduleRepository.GetAll().Where(p => p.IsActive).ToList();
            return View(data);
        }

        public IActionResult AddOrUpdate(int? id)
        {
            var obj = new CMS_Module();

            if (id == null || id <= 0)
            {
                ViewBag.Title = "Thêm mới module";
                return View(obj);
            }
            else
            {
                ViewBag.Title = "Cập nhật module";
                var model = _moduleRepository.Get(id.Value);
                if (model == null)
                    return File("~/wwwroot/html/Page404.html", "text/html");
                return View(obj);
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(CMS_Module obj)
        {
            try
            {
                if (obj.Id <= 0)
                    ViewBag.Title = "Thêm mới module";
                else
                    ViewBag.Title = "Cập nhật module";

                if (ModelState.IsValid)
                {
                    var checkExist = _moduleRepository.GetByName(obj.Name);
                    if (checkExist != null && checkExist.Id != obj.Id)
                    {
                        log.Error("Đã tồn tại module có tên = " + obj.Name);
                        SetError("Cập nhật dữ liệu không thành công. Tên module đã tồn tại!");
                        return View(obj);
                    }

                    if (obj.Id > 0)
                    {
                        var model = _moduleRepository.Get(obj.Id);
                        if (model == null)
                        {
                            SetError($"Không tìm thấy module có Id = {obj.Id}");
                            return RedirectToAction("Index", "QT_Modules");
                        }

                        obj.CreatedBy = model.CreatedBy;
                        obj.CreatedDate = model.CreatedDate;
                        obj.UpdatedBy = "host";
                        obj.UpdatedDate = DateTime.Now;

                        _moduleRepository.Update(obj, obj.Id);
                        SetSuccess("Cập nhật module thành công");
                        return RedirectToAction("Index", "QT_Modules");
                    }
                    else
                    {
                        obj.IsActive = true;
                        obj.CreatedBy = obj.UpdatedBy = "host";
                        obj.CreatedDate = obj.UpdatedDate = DateTime.Now;

                        if (_moduleRepository.Add(obj).Id > 0)
                        {
                            SetSuccess("Thêm module thành công");
                            return RedirectToAction("Index", "QT_Modules");
                        }
                        else
                        {
                            SetError("Thêm module không thành công");
                            log.Error("Thêm module không thành công tên = " + obj.Name);
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
                    var module = _moduleRepository.Get(id);
                    if (module != null)
                    {
                        var existfunction = _functionRepository.GetByModule(module.Id);
                        var existpage = _pageRepository.GetByModule(module.Id);
                        if (!existfunction.Any() || !existpage.Any())
                        {
                            _moduleRepository.Delete(module);
                            return Json(new { status = true, message = "Xóa dữ liệu thành công!" });
                        }
                        else
                            return Json(new { status = false, message = "Lỗi: Không thể xóa do module có ràng buộc dữ liệu!" });
                    }
                    else
                        return Json(new { status = false, message = "Lỗi: Không tìm thấy module có Id = " + id });
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