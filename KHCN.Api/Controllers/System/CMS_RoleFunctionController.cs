using KHCN.Api.Provider;
using KHCN.Data.DTOs.System;
using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Mvc;
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
    public class CMS_RoleFunctionController : BaseController<CMS_RoleFunctionController>
    {
        private readonly IRoleFunctionRepository _rolefunctionRepository;
        private readonly IUserProvider _userProvider;

        public CMS_RoleFunctionController(IRoleFunctionRepository rolefunctionRepository, IUserProvider userProvider)
        {
            _rolefunctionRepository = rolefunctionRepository;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CMS_RoleFunction>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _rolefunctionRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<CMS_RoleFunction>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(CMS_RoleFunction.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_rolefunctionRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CMS_RoleFunction>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _rolefunctionRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CMS_RoleFunction>> Post([FromBody] CMS_RoleFunction_DTO obj)
        {
            try
            {
                var exist = _rolefunctionRepository.GetByIdFunctionIdRole(obj.IdFunction, obj.IdRole);
                if (exist != null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.Post]. Post by IdFunction = {obj.IdFunction}, idRole = {obj.IdRole} error. Detail: Roles is exist");
                    return BadRequest("Chức năng đã được cấp quyền cho nhóm hiện tại!");
                }

                var model = new CMS_RoleFunction()
                {
                    IdFunction = obj.IdFunction,
                    IdRole = obj.IdRole
                };

                model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                model.CreatedDate = model.UpdatedDate = DateTime.Now;

                await _rolefunctionRepository.AddAsync(model);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.Post]. Post by IdFunction = {obj.IdFunction}, idRole = {obj.IdRole} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.Post]. Post by IdFunction = {obj.IdFunction}, idRole = {obj.IdRole} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CMS_RoleFunction>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _rolefunctionRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                await _rolefunctionRepository.DeleteAsync(obj);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok();
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_RoleFunction.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }
    }
}