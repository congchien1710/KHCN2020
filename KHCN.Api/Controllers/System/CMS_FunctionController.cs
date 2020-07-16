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
    public class FunctionController : BaseController<FunctionController>
    {
        private readonly IFunctionRepository _functionRepository;
        private readonly IUserProvider _userProvider;
        private readonly IRoleFunctionRepository _roleFunctionRepository;

        public FunctionController(IFunctionRepository functionRepository, IUserProvider userProvider, IRoleFunctionRepository roleFunctionRepository)
        {
            _functionRepository = functionRepository;
            _userProvider = userProvider;
            _roleFunctionRepository = roleFunctionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CMS_Function>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _functionRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<CMS_Function>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(CMS_Function.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_functionRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CMS_Function>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _functionRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]CMS_Function_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                sw.Restart();
                var res = _functionRepository.Get(id);
                if (res == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Put]. Put by name = {obj.Name} error. Detail: Name is exist in system");
                    return BadRequest();
                }

                var exist = _functionRepository.GetByName(obj.Name);
                if (exist != null && exist.Id != res.Id)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Put]. Put by id = {id} error. Detail: Name [{obj.Name}] is exist in system");
                    return BadRequest("Tên đã tồn tại trong hệ thống!");
                }

                var model = new CMS_Function()
                {
                    Id = obj.Id.Value,
                    Name = obj.Name,
                    Description = obj.Description,
                    IdParent = obj.IdParent,
                    IdModule = obj.IdModule,
                    ControllerAction = obj.ControllerAction,
                    Controller = obj.Controller,
                    Action = obj.Action,
                    IsActive = obj.IsActive,
                    CreatedBy = res.CreatedBy,
                    CreatedDate = res.CreatedDate,
                    UpdatedBy = _userProvider.GetUserName(),
                    UpdatedDate = DateTime.Now,
                };

                await _functionRepository.UpdateAsync(res, res.Id);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CMS_Function>> Post([FromBody]CMS_Function_DTO obj)
        {
            try
            {
                sw.Restart();
                var exist = _functionRepository.GetByName(obj.Name);
                if (exist != null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Post]. Post by name = {obj.Name} error. Detail: Name is exist in system");
                    return BadRequest("Tên đã tồn tại trong hệ thống!");
                }

                var model = new CMS_Function()
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    IdModule = obj.IdModule,
                    IdParent = obj.IdParent,
                    ControllerAction = obj.ControllerAction,
                    Controller = obj.Controller,
                    Action = obj.Action
                };

                model.IsActive = true;
                model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                model.CreatedDate = model.UpdatedDate = DateTime.Now;

                await _functionRepository.AddAsync(model);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Post]. Post by name = {obj.Name} success. Time process: {sw.Elapsed.Seconds} seconds.");

                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Post]. Post by name = {obj.Name} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CMS_Function>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _functionRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                var existRoleFunc = _roleFunctionRepository.GetByIdRole(obj.Id);
                if (!existRoleFunc.Any())
                {
                    await _functionRepository.DeleteAsync(obj);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok();
                }
                else
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Delete]. Delete by id = {id} error. Detail:  Function have relationship data");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_Function.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _functionRepository.Get(id.Value) == null;
        }
    }
}