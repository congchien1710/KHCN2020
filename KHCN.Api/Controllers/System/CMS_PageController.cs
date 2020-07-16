using KHCN.Api.Provider;
using KHCN.Data.DTOs.System;
using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Api.Controllers.System
{
    [ApiController]
    [ApiAuthorize]
    [Route("api/[controller]/[action]")]
    public class PageController : BaseController<PageController>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUserProvider _userProvider;
        private readonly IRolePageRepository _rolePageRepository;

        public PageController(IPageRepository pageRepository, IUserProvider userProvider, IRolePageRepository rolePageRepository)
        {
            _pageRepository = pageRepository;
            _userProvider = userProvider;
            _rolePageRepository = rolePageRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CMS_Page>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _pageRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<CMS_Page>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(CMS_Page.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_pageRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CMS_Page>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _pageRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]CMS_Page_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                sw.Restart();
                var res = _pageRepository.Get(id);
                if (res == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                    return BadRequest();
                }

                var exist = _pageRepository.GetByName(obj.Name);
                if (exist != null && exist.Id != res.Id)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Put]. Put by id = {id} error. Detail: Name [{obj.Name}] is exist in system");
                    return BadRequest("Trang đã tồn tại trong hệ thống!");
                }

                var model = new CMS_Page()
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    IsAdmin = obj.IsAdmin,
                    IdModule = obj.IdModule,
                    IdParent = obj.IdParent,
                    Icon = obj.Icon,
                    Key = obj.Key,
                    OrderHint = obj.OrderHint,
                    IsActive = obj.IsActive,
                    CreatedBy = res.CreatedBy,
                    CreatedDate = res.CreatedDate,
                    UpdatedBy = _userProvider.GetUserName(),
                    UpdatedDate = DateTime.Now,
                };

                await _pageRepository.UpdateAsync(model, model.Id);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjectExists(obj.Id))
                    return NotFound();
                else
                    throw;
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CMS_Page>> Post([FromBody]CMS_Page_DTO obj)
        {
            try
            {
                sw.Restart();
                var exist = _pageRepository.GetByName(obj.Name);
                if (exist != null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Post]. Post by name = {obj.Name} error. Detail: Name is exist in system");
                    return BadRequest("Tên đã tồn tại trong hệ thống!");
                }

                var model = new CMS_Page()
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    IsAdmin = obj.IsAdmin,
                    IdModule = obj.IdModule,
                    IdParent = obj.IdParent,
                    Icon = obj.Icon,
                    Key = obj.Key,
                    OrderHint = obj.OrderHint
                };

                model.IsActive = true;
                model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                model.CreatedDate = model.UpdatedDate = DateTime.Now;

                await _pageRepository.AddAsync(model);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Post]. Post by name = {obj.Name} success. Time process: {sw.Elapsed.Seconds} seconds.");

                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Post]. Post by name = {obj.Name} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CMS_Page>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _pageRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                var existRolePage = _rolePageRepository.GetByIdRole(obj.Id);
                if (!existRolePage.Any())
                {
                    await _pageRepository.DeleteAsync(obj);
                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok();
                }
                else
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Delete]. Delete by id = {id} error. Detail:  Function have relationship data");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Page.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _pageRepository.Get(id.Value) == null;
        }
    }
}