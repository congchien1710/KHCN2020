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

namespace KHCN.Api.Controllers.KHCN
{
    [ApiController]
    [ApiAuthorize]
    [Route("api/[controller]/[action]")]
    public class UserRoleController : BaseController<UserRoleController>
    {
        private readonly IUserRoleRepository _userroleRepository;
        private readonly IUserProvider _userProvider;

        public UserRoleController(IUserRoleRepository userroleRepository, IUserProvider userProvider)
        {
            _userroleRepository = userroleRepository;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CMS_UserRole>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _userroleRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<CMS_UserRole>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(CMS_UserRole.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_userroleRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CMS_UserRole>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _userroleRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]CMS_UserRole_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                sw.Restart();
                var res = _userroleRepository.Get(id);
                if (res == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                    return BadRequest();
                }

                var exist = _userroleRepository.GetByIdUserIdRole(obj.IdUser, obj.IdRole);
                if (exist != null && exist.Id != res.Id)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Post]. Put by id = {id} error. Detail: IdUser = {obj.IdUser}, IdRole = {obj.IdRole} is exist in system");
                    return BadRequest("Tên đã tồn tại trong hệ thống!");
                }

                var model = new CMS_UserRole()
                {
                    Id = obj.Id.Value,
                    IdUser = obj.IdUser,
                    IdRole = obj.IdRole,
                    CreatedBy = res.CreatedBy,
                    CreatedDate = res.CreatedDate,
                    UpdatedBy = _userProvider.GetUserName(),
                    UpdatedDate = DateTime.Now,
                };

                await _userroleRepository.UpdateAsync(model, model.Id);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CMS_UserRole>> Post([FromBody]CMS_UserRole_DTO obj)
        {
            try
            {
                sw.Restart();
                var exist = _userroleRepository.GetByIdUserIdRole(obj.IdUser, obj.IdRole);
                if (exist != null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Post]. Post by IdUser = {obj.IdUser}, IdRole = {obj.IdRole} error. Detail: RoleUser is exist in system");
                    return BadRequest("Tên đã tồn tại trong hệ thống!");
                }

                var model = new CMS_UserRole()
                {
                    IdUser = obj.IdUser,
                    IdRole = obj.IdRole,
                };

                model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                model.CreatedDate = model.UpdatedDate = DateTime.Now;
                await _userroleRepository.AddAsync(model);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Post]. Post by IdUser = {obj.IdUser}, IdRole = {obj.IdRole} success. Time process: {sw.Elapsed.Seconds} seconds.");

                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Post]. Post by IdUser = {obj.IdUser}, IdRole = {obj.IdRole} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CMS_UserRole>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _userroleRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                await _userroleRepository.DeleteAsync(obj);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok();
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_UserRole.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _userroleRepository.Get(id.Value) == null;
        }
    }
}