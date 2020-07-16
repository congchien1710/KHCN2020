using KHCN.Data.DTOs.System;
using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KHCN.Web.Controllers
{
    [CustomAuthorize]
    public class QT_PagesController : BaseController
    {
        private readonly IPageRepository _pageRepository;
        private readonly IRolePageRepository _rolepageRepository;
        private readonly IModuleRepository _moduleRepository;

        public QT_PagesController(IPageRepository pageRepository, IRolePageRepository rolepageRepository, IModuleRepository moduleRepository)
        {
            _pageRepository = pageRepository;
            _rolepageRepository = rolepageRepository;
            _moduleRepository = moduleRepository;
        }

        public IActionResult Index()
        {
            var data = _pageRepository.GetAll().Where(p => p.IsActive).ToList();
            if (data == null || !data.Any())
            {
                GenerateData(data);
            }

            var result = new List<CMS_Page_DTO>();
            foreach (var item in data.Where(m => m.IdParent == 0))
            {
                var mapper = AutoMapper.Mapper.Map<CMS_Page_DTO>(item);
                mapper.NameParent = (mapper.IdParent != null && mapper.IdParent > 0) ? data.FirstOrDefault(m => m.Id == mapper.IdParent).Name : "";
                mapper.NameModule = (mapper.IdModule != null && mapper.IdModule > 0) ? _moduleRepository.Get(mapper.IdModule.Value).Name : "";
                result.Add(mapper);

                foreach (var vItem in data.Where(m => m.IdParent == item.Id).ToList())
                {
                    mapper = AutoMapper.Mapper.Map<CMS_Page_DTO>(vItem);
                    mapper.NameParent = (mapper.IdParent != null && mapper.IdParent > 0) ? data.FirstOrDefault(m => m.Id == mapper.IdParent).Name : "";
                    mapper.NameModule = (mapper.IdModule != null && mapper.IdModule > 0) ? _moduleRepository.Get(mapper.IdModule.Value).Name : "";
                    result.Add(mapper);
                }
            }

            return View(result);
        }

        private void GenerateData(List<CMS_Page> data)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.CustomAttributes.Select(p => p.AttributeType.Name).Contains("HttpPostAttribute"))
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
                    .Where(m => !m.CustomAttributes.Select(p => p.AttributeType.Name).Contains("HttpPostAttribute"))
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
                var pageRoot = _pageRepository.Add(new CMS_Page
                {
                    IdModule = 1,
                    IdParent = 0,
                    Name = "Trang " + item,
                    Description = "Trang " + item,
                    IsAdmin = true,
                    Icon = "<i class='fa fa-caret-right'></i>",
                    Key = "~",
                    OrderHint = 1,
                    IsActive = true,
                    CreatedBy = "host",
                    CreatedDate = now,
                    UpdatedBy = "host",
                    UpdatedDate = now,
                });

                if (pageRoot != null && pageRoot.Id > 0)
                {
                    var lstAction = list.Where(m => m.Controller == item).Select(m => m.Action).Distinct().ToList();
                    foreach (var action in lstAction)
                    {
                        var function = _pageRepository.Add(new CMS_Page
                        {
                            IdModule = pageRoot.IdModule,
                            IdParent = pageRoot.Id,
                            Name = $"Trang {item.Replace("Controller", "")}-{action}",
                            Description = $"Trang {item.Replace("Controller", "")}-{action}",
                            IsAdmin = true,
                            Icon = "<i class='fa fa-caret-right'></i>",
                            Key = $"{item.Replace("Controller", "")}/{action}",
                            OrderHint = 1,
                            IsActive = true,
                            CreatedBy = "host",
                            CreatedDate = now,
                            UpdatedBy = "host",
                            UpdatedDate = now,
                        });

                        if (function == null || function.Id <= 0)
                            log.Error($"Insert page failed. Function {item}-{action}");
                    }
                }
                else
                    log.Error($"Insert page failed. Function {item}");
            }

            data = _pageRepository.GetAll().Where(p => p.IsActive).ToList();
        }

        public IActionResult AddOrUpdate(int? id)
        {
            ViewBag.DDLMODULE = new SelectList(_moduleRepository.GetAll().Where(p => p.IsActive).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }), "Value", "Text");
            var dataFunction = BuildTreeFunctions(_pageRepository.GetAll().Where(p => p.IsActive).ToList());
            ViewBag.DDLPARENT = new SelectList(dataFunction, "Id", "Name");

            var obj = new CMS_Page();

            if (id == null || id <= 0)
            {
                ViewBag.Title = "Thêm mới trang";
                return View();
            }
            else
            {
                ViewBag.Title = "Cập nhật trang";
                var model = _pageRepository.Get(id.Value);
                if (model == null)
                    return File("~/wwwroot/html/Page404.html", "text/html");
                return View(obj);
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(CMS_Page obj)
        {
            try
            {
                ViewBag.DDLMODULE = new SelectList(_moduleRepository.GetAll().Where(p => p.IsActive).Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }), "Value", "Text");
                var dataFunction = BuildTreeFunctions(_pageRepository.GetAll().Where(p => p.IsActive).ToList());
                ViewBag.DDLPARENT = new SelectList(dataFunction, "Id", "Name");

                if (obj.Id <= 0)
                    ViewBag.Title = "Thêm mới trang";
                else
                    ViewBag.Title = "Cập nhật trang";

                if (ModelState.IsValid)
                {
                    var checkExist = _pageRepository.GetByName(obj.Name);
                    if (checkExist != null && checkExist.Id != obj.Id)
                    {
                        log.Error("Đã tồn tại trang có tên = " + obj.Name);
                        SetError("Cập nhật dữ liệu không thành công. Tên trang đã tồn tại!");
                        return View(obj);
                    }

                    if (obj.Id > 0)
                    {
                        var model = _pageRepository.Get(obj.Id);
                        if (model == null)
                        {
                            SetError($"Không tìm thấy trang có Id = {obj.Id}");
                            return RedirectToAction("Index", "QT_Pages");
                        }

                        obj.CreatedBy = model.CreatedBy;
                        obj.CreatedDate = model.CreatedDate;
                        obj.UpdatedBy = "host";
                        obj.UpdatedDate = DateTime.Now;

                        _pageRepository.Update(obj, obj.Id);
                        SetSuccess("Cập nhật trang thành công");
                        return RedirectToAction("Index", "QT_Pages");
                    }
                    else
                    {
                        obj.IsActive = true;
                        obj.CreatedBy = obj.UpdatedBy = "host";
                        obj.CreatedDate = obj.UpdatedDate = DateTime.Now;

                        if (_pageRepository.Add(obj).Id > 0)
                        {
                            SetSuccess("Thêm trang thành công");
                            return RedirectToAction("Index", "QT_Pages");
                        }
                        else
                        {
                            SetError("Thêm trang không thành công");
                            log.Error("Thêm trang không thành công tên = " + obj.Name);
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
                    var page = _pageRepository.Get(id);
                    if (page != null)
                    {
                        var existRolePage = _rolepageRepository.GetByIdRole(page.Id);
                        if (!existRolePage.Any())
                        {
                            _pageRepository.Delete(page);
                            return Json(new { status = true, message = "Xóa dữ liệu thành công!" });
                        }
                        else
                            return Json(new { status = false, message = "Lỗi: Không thể xóa do trang có ràng buộc dữ liệu!" });
                    }
                    else
                        return Json(new { status = false, message = "Lỗi: Không tìm thấy trang có Id = " + id });
                }

                return Json(new { status = false, message = "Lỗi: Id null" });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { status = false, message = "Lỗi: " + ex.Message });
            }
        }

        List<CMS_Page> listResult = new List<CMS_Page>();
        string t = " --- ";
        public List<CMS_Page> BuildTreeFunctions(List<CMS_Page> lstData, int parent = 0)
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
                return new List<CMS_Page>();
            }
        }
    }
}