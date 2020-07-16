using KHCN.Data.DTOs.System;
using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KHCN.Web.Controllers
{
    [CustomAuthorize]
    public class QT_ApiController : BaseController
    {
        private readonly IApiRepository _apiRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IRoleApiRepository _roleapiRepository;
        public QT_ApiController(IApiRepository apiRepository, IModuleRepository moduleRepository, IRoleApiRepository roleapiRepository)
        {
            _apiRepository = apiRepository;
            _moduleRepository = moduleRepository;
            _roleapiRepository = roleapiRepository;
        }

        public IActionResult Index()
        {
            var data = _apiRepository.GetAll().Where(p => p.IsActive).ToList();
            if (data == null || !data.Any())
            {
                GenerateData(data);
            }

            var result = new List<CMS_Api_DTO>();
            foreach (var item in data.Where(m => m.IdParent == 0))
            {
                var mapper = AutoMapper.Mapper.Map<CMS_Api_DTO>(item);
                mapper.NameParent = (mapper.IdParent != null && mapper.IdParent > 0) ? data.FirstOrDefault(m => m.Id == mapper.IdParent).Name : "";
                mapper.NameModule = (mapper.IdModule != null && mapper.IdModule > 0) ? _moduleRepository.Get(mapper.IdModule.Value).Name : "";
                result.Add(mapper);

                foreach (var vItem in data.Where(m => m.IdParent == item.Id).ToList())
                {
                    mapper = AutoMapper.Mapper.Map<CMS_Api_DTO>(vItem);
                    mapper.NameParent = (mapper.IdParent != null && mapper.IdParent > 0) ? data.FirstOrDefault(m => m.Id == mapper.IdParent).Name : "";
                    mapper.NameModule = (mapper.IdModule != null && mapper.IdModule > 0) ? _moduleRepository.Get(mapper.IdModule.Value).Name : "";
                    result.Add(mapper);
                }
            }

            return View(result);
        }

        private void GenerateData(List<CMS_Api> data)
        {
            var list = KHCN.Api.Provider.AssemblyHelper.GetAllControllerAction();

            foreach (var item in list.Select(m => m.Api).Distinct().ToList())
            {
                var apiName = item.Replace("Controller", "");
                var now = DateTime.Now;
                var functionRoot = _apiRepository.Add(new CMS_Api
                {
                    IdModule = 0,
                    IdParent = 0,
                    Name = $"{apiName} Api",
                    Controller = item,
                    Action = "",
                    ControllerAction = item,
                    Description = $"{apiName} Api",
                    IsActive = true,
                    CreatedBy = "host",
                    CreatedDate = now,
                    UpdatedBy = "host",
                    UpdatedDate = now,
                });

                if (functionRoot != null && functionRoot.Id > 0)
                {
                    var lstAction = list.Where(m => m.Api == item).Select(m => m.Action).Distinct().ToList();
                    foreach (var action in lstAction)
                    {
                        var function = _apiRepository.Add(new CMS_Api
                        {
                            IdModule = functionRoot.IdModule,
                            IdParent = functionRoot.Id,
                            Name = $"{apiName}-{action}",
                            Controller = apiName,
                            Action = $"{action}",
                            ControllerAction = $"{item.Replace("Controller", "")}-{action}",
                            Description = $"{item.Replace("Controller", "")}-{action}",
                            IsActive = true,
                            CreatedBy = "host",
                            CreatedDate = now,
                            UpdatedBy = "host",
                            UpdatedDate = now,
                        });

                        if (function == null || function.Id <= 0)
                            log.Error($"Insert function failed. Function {item}-{action}");
                    }
                }
                else
                    log.Error($"Insert functionRoot failed. Function {item}");
            }

            data = _apiRepository.GetAll().Where(p => p.IsActive).ToList();
        }

        public IActionResult AddOrUpdate(int? id)
        {
            var lstData = KHCN.Api.Provider.AssemblyHelper.GetAllControllerAction();
            List<string> lstController = lstData.Select(m => m.Api).Distinct().ToList();
            List<string> lstAction = new List<string>();

            ViewBag.DDLCONTROLLER = lstController.Select(x => new SelectListItem() { Text = x, Value = x });
            ViewBag.DDLACTION = new SelectList(lstAction, "Value", "Text");
            ViewBag.DDLMODULE = new SelectList(_moduleRepository.GetAll().Where(p => p.IsActive).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }), "Value", "Text");
            var dataFunction = BuildTreeFunctions(_apiRepository.GetAll().Where(p => p.IsActive).ToList());
            ViewBag.DDLPARENT = new SelectList(dataFunction, "Id", "Name");

            var obj = new CMS_Api();

            if (id == null || id <= 0)
            {
                ViewBag.Title = "Thêm mới api";
                return View(obj);
            }
            else
            {
                ViewBag.Title = "Cập nhật api";
                var model = _apiRepository.Get(id.Value);
                if (model == null)
                    return File("~/wwwroot/html/Page404.html", "text/html");
                return View(obj);
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(CMS_Api obj)
        {
            try
            {
                var lstData = KHCN.Api.Provider.AssemblyHelper.GetAllControllerAction();
                List<string> lstController = lstData.Select(m => m.Api).Distinct().ToList();
                List<string> lstAction = new List<string>();

                ViewBag.DDLCONTROLLER = lstController.Select(x => new SelectListItem() { Text = x, Value = x });
                ViewBag.DDLACTION = new SelectList(lstAction, "Value", "Text");
                ViewBag.DDLMODULE = new SelectList(_moduleRepository.GetAll().Where(p => p.IsActive).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }), "Value", "Text");
                var dataFunction = BuildTreeFunctions(_apiRepository.GetAll().Where(p => p.IsActive).ToList());
                ViewBag.DDLPARENT = new SelectList(dataFunction, "Id", "Name");

                if (obj.Id <= 0)
                    ViewBag.Title = "Thêm mới api";
                else
                    ViewBag.Title = "Cập nhật api";

                if (ModelState.IsValid)
                {
                    var checkExist = _apiRepository.GetByName(obj.Name);
                    if (checkExist != null && checkExist.Id != obj.Id)
                    {
                        log.Error("Đã tồn tại api có tên = " + obj.Name);
                        SetError("Cập nhật dữ liệu không thành công. Tên api đã tồn tại!");
                        return View(obj);
                    }

                    if (obj.Id > 0)
                    {
                        var model = _apiRepository.Get(obj.Id);
                        if (model == null)
                        {
                            SetError($"Không tìm thấy api có Id = {obj.Id}");
                            return RedirectToAction("Index", "QT_Api");
                        }

                        obj.CreatedBy = model.CreatedBy;
                        obj.CreatedDate = model.CreatedDate;
                        obj.UpdatedBy = "host";
                        obj.UpdatedDate = DateTime.Now;

                        _apiRepository.Update(obj, obj.Id);
                        SetSuccess("Cập nhật api thành công");
                        return RedirectToAction("Index", "QT_Api");
                    }
                    else
                    {
                        obj.IsActive = true;
                        obj.CreatedBy = obj.UpdatedBy = "host";
                        obj.CreatedDate = obj.UpdatedDate = DateTime.Now;

                        if (_apiRepository.Add(obj).Id > 0)
                        {
                            SetSuccess("Thêm api thành công");
                            return RedirectToAction("Index", "QT_Api");
                        }
                        else
                        {
                            SetError("Thêm api không thành công");
                            log.Error("Thêm api không thành công tên = " + obj.Name);
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
                    var function = _apiRepository.Get(id);
                    if (function != null)
                    {
                        var existRoleFunc = _roleapiRepository.GetByIdRole(function.Id);
                        if (!existRoleFunc.Any())
                        {
                            _apiRepository.Delete(function);
                            return Json(new { status = true, message = "Xóa dữ liệu thành công!" });
                        }
                        else
                            return Json(new { status = false, message = "Lỗi: Không thể xóa do api có ràng buộc dữ liệu!" });
                    }
                    else
                        return Json(new { status = false, message = "Lỗi: Không tìm thấy api có Id = " + id });
                }

                return Json(new { status = false, message = "Lỗi: Id null" });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { status = false, message = "Lỗi: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetActionByController(string controllerName)
        {
            if (!string.IsNullOrEmpty(controllerName))
            {
                var lstData = KHCN.Api.Provider.AssemblyHelper.GetAllControllerAction();
                var lstAction = lstData.Where(m => m.Api == controllerName).Select(m => m.Action).ToList();
                string result = JsonConvert.SerializeObject(lstAction.Distinct());

                return Json(new { status = true, data = result });
            }
            else
                return Json(new { status = false });
        }

        List<CMS_Api> listResult = new List<CMS_Api>();
        string t = " --- ";

        public List<CMS_Api> BuildTreeFunctions(List<CMS_Api> lstData, int parent = 0)
        {
            try
            {
                foreach (var item in lstData.Where(i => i.IdParent == parent).OrderBy(m => m.Id).ToList())
                {
                    var subItem = lstData.Where(s => s.IdParent == item.Id).Count();
                    if (subItem > 0)
                    {
                        if (item.IdParent > 0)
                        {
                            item.Name = t + item.Name;
                            t += "--";
                        }

                        listResult.Add(item);
                        BuildTreeFunctions(lstData, item.Id);
                    }
                    else
                    {
                        if (item.IdParent > 0)
                            item.Name = t + item.Name;

                        listResult.Add(item);
                    }
                }

                return listResult;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new List<CMS_Api>();
            }
        }
    }
}