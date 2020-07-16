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
    public class ModuleController : BaseController<ModuleController>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IUserProvider _userProvider;
        private readonly IFunctionRepository _functionRepository;
        private readonly IPageRepository _pageRepository;

        public ModuleController(IModuleRepository moduleRepository, IUserProvider userProvider, IFunctionRepository functionRepository, IPageRepository pageRepository)
        {
            _moduleRepository = moduleRepository;
            _userProvider = userProvider;
            _functionRepository = functionRepository;
            _pageRepository = pageRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CMS_Module>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _moduleRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<CMS_Module>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(CMS_Module.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_moduleRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CMS_Module>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _moduleRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]CMS_Module_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                sw.Restart();
                var res = _moduleRepository.Get(id);
                if (res == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Put]. Put by name = {obj.Name} error. Detail: Name is exist in system");
                    return BadRequest();
                }

                var exist = _moduleRepository.GetByName(obj.Name);
                if (exist != null && exist.Id != res.Id)
                    return BadRequest("Tên đã tồn tại trong hệ thống!");

                var model = new CMS_Module()
                {
                    Id = obj.Id.Value,
                    Name = obj.Name,
                    Description = obj.Description,
                    IsActive = obj.IsActive,
                    CreatedBy = res.CreatedBy,
                    CreatedDate = res.CreatedDate,
                    UpdatedBy = _userProvider.GetUserName(),
                    UpdatedDate = DateTime.Now,
                };

                await _moduleRepository.UpdateAsync(model, model.Id);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CMS_Module>> Post([FromBody]CMS_Module_DTO obj)
        {
            try
            {
                sw.Restart();
                var exist = _moduleRepository.GetByName(obj.Name);
                if (exist != null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Post]. Post by name = {obj.Name} error. Detail: Name is exist in system");
                    return BadRequest("Tên đã tồn tại trong hệ thống!");
                }

                var model = new CMS_Module()
                {
                    Name = obj.Name,
                    Description = obj.Description
                };

                model.IsActive = true;
                model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                model.CreatedDate = model.UpdatedDate = DateTime.Now;

                await _moduleRepository.AddAsync(model);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Post]. Post by name = {obj.Name} success. Time process: {sw.Elapsed.Seconds} seconds.");

                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Post]. Post by name = {obj.Name} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CMS_Module>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _moduleRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                var existfunction = _functionRepository.GetByModule(obj.Id);
                var existpage = _pageRepository.GetByModule(obj.Id);
                if (!existfunction.Any() && !existpage.Any())
                {
                    await _moduleRepository.DeleteAsync(obj);
                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok();
                }
                else
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Delete]. Delete by id = {id} error. Detail:  Module have relationship data");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Module.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _moduleRepository.Get(id.Value) == null;
        }
    }
}