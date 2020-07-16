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
    public class QT_FunctionsController : BaseController
    {
        private readonly IFunctionRepository _functionRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IRoleFunctionRepository _rolefunctionRepository;
        public QT_FunctionsController(IFunctionRepository functionRepository, IModuleRepository moduleRepository, IRoleFunctionRepository rolefunctionRepository)
        {
            _functionRepository = functionRepository;
            _moduleRepository = moduleRepository;
            _rolefunctionRepository = rolefunctionRepository;
        }

        public IActionResult Index()
        {
            var data = _functionRepository.GetAll().Where(p => p.IsActive).ToList();
            if (data == null || !data.Any())
            {
                GenerateData(data);
            }

            var result = new List<CMS_Function_DTO>();
            foreach (var item in data.Where(m => m.IdParent == 0))
            {
                var mapper = AutoMapper.Mapper.Map<CMS_Function_DTO>(item);
                mapper.NameParent = (mapper.IdParent != null && mapper.IdParent > 0) ? data.FirstOrDefault(m => m.Id == mapper.IdParent).Name : "";
                mapper.NameModule = (mapper.IdModule != null && mapper.IdModule > 0) ? _moduleRepository.Get(mapper.IdModule.Value).Name : "";
                result.Add(mapper);

                foreach (var vItem in data.Where(m => m.IdParent == item.Id).ToList())
                {
                    mapper = AutoMapper.Mapper.Map<CMS_Function_DTO>(vItem);
                    mapper.NameParent = (mapper.IdParent != null && mapper.IdParent > 0) ? data.FirstOrDefault(m => m.Id == mapper.IdParent).Name : "";
                    mapper.NameModule = (mapper.IdModule != null && mapper.IdModule > 0) ? _moduleRepository.Get(mapper.IdModule.Value).Name : "";
                    result.Add(mapper);
                }
            }

            return View(result);
        }

        private void GenerateData(List<CMS_Function> data)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(method => method.IsPublic
                    && !method.IsDefined(typeof(NonControllerAttribute))
                    && !method.IsDefined(typeof(NonActionAttribute)))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))
                    }).ToList();

            var lstController = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(method => method.IsPublic
                    && !method.IsDefined(typeof(NonControllerAttribute)))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name
                    }).Distinct().ToList();

            var list = new List<ControllerActions>();
            foreach (var item in controlleractionlist)
            {
                if (item.Area.Count() != 0)
                {
                    list.Add(new ControllerActions()
                    {
                        Controller = item.Controller,
                        Action = item.Action,
                        Area = item.Area.Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault()
                    });
                }
                else
                {
                    list.Add(new ControllerActions()
                    {
                        Controller = item.Controller,
                        Action = item.Action,
                        Area = null,
                    });
                }
            }

            foreach (var item in list.Select(m => m.Controller).Distinct().ToList())
            {
                var now = DateTime.Now;
                var functionRoot = _functionRepository.Add(new CMS_Function
                {
                    IdModule = 1,
                    IdParent = 0,
                    Name = "Quản trị " + item,
                    Controller = item,
                    Action = "",
                    ControllerAction = item,
                    Description = "Quản trị " + item,
                    IsActive = true,
                    CreatedBy = "host",
                    CreatedDate = now,
                    UpdatedBy = "host",
                    UpdatedDate = now,
                });

                if (functionRoot != null && functionRoot.Id > 0)
                {
                    var lstAction = list.Where(m => m.Controller == item).Select(m => m.Action).Distinct().ToList();
                    foreach (var action in lstAction)
                    {
                        var function = _functionRepository.Add(new CMS_Function
                        {
                            IdModule = functionRoot.IdModule,
                            IdParent = functionRoot.Id,
                            Name = $"{item.Replace("Controller", "")}-{action}",
                            Controller = $"{item.Replace("Controller", "")}",
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

            data = _functionRepository.GetAll().Where(p => p.IsActive).ToList();
        }

        public IActionResult AddOrUpdate(int? id)
        {
            var result = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(method => method.IsPublic
                    && !method.IsDefined(typeof(NonControllerAttribute))
                    && !method.IsDefined(typeof(NonActionAttribute)))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))
                    }).ToList();

            List<string> lstController = result.Select(m => m.Controller).Distinct().ToList();
            List<string> lstAction = new List<string>();

            ViewBag.DDLCONTROLLER = lstController.Select(x => new SelectListItem() { Text = x, Value = x });
            ViewBag.DDLACTION = new SelectList(lstAction, "Value", "Text");
            ViewBag.DDLMODULE = new SelectList(_moduleRepository.GetAll().Where(p => p.IsActive).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }), "Value", "Text");
            var dataFunction = BuildTreeFunctions(_functionRepository.GetAll().Where(p => p.IsActive).ToList());
            ViewBag.DDLPARENT = new SelectList(dataFunction, "Id", "Name");

            var obj = new CMS_Function();

            if (id == null || id <= 0)
            {
                ViewBag.Title = "Thêm mới chức năng";
                return View(obj);
            }
            else
            {
                ViewBag.Title = "Cập nhật chức năng";
                var model = _functionRepository.Get(id.Value);
                if (model == null)
                    return File("~/wwwroot/html/Page404.html", "text/html");
                return View(obj);
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(CMS_Function obj)
        {
            try
            {
                var result = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(method => method.IsPublic
                    && !method.IsDefined(typeof(NonControllerAttribute))
                    && !method.IsDefined(typeof(NonActionAttribute)))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))
                    }).ToList();

                List<string> lstController = result.Select(m => m.Controller).Distinct().ToList();
                List<string> lstAction = new List<string>();

                ViewBag.DDLCONTROLLER = lstController.Select(x => new SelectListItem() { Text = x, Value = x });
                ViewBag.DDLACTION = new SelectList(lstAction, "Value", "Text");
                ViewBag.DDLMODULE = new SelectList(_moduleRepository.GetAll().Where(p => p.IsActive).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }), "Value", "Text");
                var dataFunction = BuildTreeFunctions(_functionRepository.GetAll().Where(p => p.IsActive).ToList());
                ViewBag.DDLPARENT = new SelectList(dataFunction, "Id", "Name");

                if (obj.Id <= 0)
                    ViewBag.Title = "Thêm mới chức năng";
                else
                    ViewBag.Title = "Cập nhật chức năng";

                if (ModelState.IsValid)
                {
                    var checkExist = _functionRepository.GetByName(obj.Name);
                    if (checkExist != null && checkExist.Id != obj.Id)
                    {
                        log.Error("Đã tồn tại chức năng có tên = " + obj.Name);
                        SetError("Cập nhật dữ liệu không thành công. Tên chức năng đã tồn tại!");
                        return View(obj);
                    }

                    if (obj.Id > 0)
                    {
                        var model = _functionRepository.Get(obj.Id);
                        if (model == null)
                        {
                            SetError($"Không tìm thấy chức năng có Id = {obj.Id}");
                            return RedirectToAction("Index", "QT_Functions");
                        }

                        obj.CreatedBy = model.CreatedBy;
                        obj.CreatedDate = model.CreatedDate;
                        obj.UpdatedBy = User.Identity.Name;
                        obj.UpdatedDate = DateTime.Now;

                        _functionRepository.Update(obj, obj.Id);
                        SetSuccess("Cập nhật chức năng thành công");
                        return RedirectToAction("Index", "QT_Functions");
                    }
                    else
                    {
                        obj.CreatedBy = obj.UpdatedBy = User.Identity.Name;
                        obj.CreatedDate = obj.UpdatedDate = DateTime.Now;
                        obj.IsActive = true;

                        if (_functionRepository.Add(obj).Id > 0)
                        {
                            SetSuccess("Thêm chức năng thành công");
                            return RedirectToAction("Index", "QT_Functions");
                        }
                        else
                        {
                            SetError("Thêm chức năng không thành công");
                            log.Error("Thêm chức năng không thành công tên = " + obj.Name);
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
                    var function = _functionRepository.Get(id);
                    if (function != null)
                    {
                        var existRoleFunc = _rolefunctionRepository.GetByIdRole(function.Id);
                        if (!existRoleFunc.Any())
                        {
                            _functionRepository.Delete(function);
                            return Json(new { status = true, message = "Xóa dữ liệu thành công!" });
                        }
                        else
                            return Json(new { status = false, message = "Lỗi: Không thể xóa do chức năng có ràng buộc dữ liệu!" });
                    }
                    else
                        return Json(new { status = false, message = "Lỗi: Không tìm thấy chức năng có Id = " + id });
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
        public ActionResult GetApiControllerAction()
        {
            var lstData = KHCN.Api.Provider.AssemblyHelper.GetAllControllerAction();
            List<string> lstController = lstData.Select(m => m.Api).Distinct().ToList();

            return Json(new { status = true, data = JsonConvert.SerializeObject(lstController) });
        }

        [HttpPost]
        public ActionResult GetActionByController(string controllerName)
        {
            if (!string.IsNullOrEmpty(controllerName))
            {
                var lstData = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(method => method.IsPublic
                    && !method.IsDefined(typeof(NonControllerAttribute))
                    && !method.IsDefined(typeof(NonActionAttribute)))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        Area = x.DeclaringType.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))
                    }).ToList();

                var lstAction = lstData.Where(m => m.Controller == controllerName).Select(m => m.Action).ToList();
                string result = JsonConvert.SerializeObject(lstAction.Distinct());

                return Json(new { status = true, data = result });
            }
            else
                return Json(new { status = false });
        }

        List<CMS_Function> listResult = new List<CMS_Function>();
        string t = " --- ";

        public List<CMS_Function> BuildTreeFunctions(List<CMS_Function> lstData, int parent = 0)
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
                return new List<CMS_Function>();
            }
        }
    }
}