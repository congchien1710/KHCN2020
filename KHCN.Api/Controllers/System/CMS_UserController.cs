using KHCN.Api.Provider;
using KHCN.Api.Services;
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
    public class UserController : BaseController<UserController>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProvider _userProvider;
        private readonly IUserRoleRepository _roleuserRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserController(IUserRepository userRepository, IUserProvider userProvider, IUserRoleRepository roleuserRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _userProvider = userProvider;
            _roleuserRepository = roleuserRepository;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CMS_User>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _userRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<CMS_User>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(CMS_User.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_userRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CMS_User>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _userRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                obj.Password = "*********************";
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]CMS_User_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                sw.Restart();
                var res = _userRepository.Get(id);
                if (res == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                    return BadRequest();
                }

                var exist = _userRepository.GetByUsername(obj.UserName);
                if (exist != null && exist.Id != res.Id)
                {
                    sw.Stop();
                    return BadRequest("Username đã tồn tại trong hệ thống!");
                }

                var model = new CMS_User()
                {
                    Id = obj.Id.Value,
                    FullName = obj.FullName,
                    UserName = obj.UserName,
                    Email = obj.Email,
                    Password = _passwordHasher.GenerateIdentityV3Hash(obj.Password),
                    Address = obj.Address,
                    Mobile = obj.Mobile,
                    IsActive = obj.IsActive,
                    ActiveCode = res.ActiveCode,
                    RefreshToken = res.RefreshToken,
                    CreatedBy = res.CreatedBy,
                    CreatedDate = res.CreatedDate,
                    UpdatedBy = _userProvider.GetUserName(),
                    UpdatedDate = DateTime.Now,
                };

                await _userRepository.UpdateAsync(res, res.Id);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<CMS_User>> Post([FromBody]CMS_User_DTO obj)
        {
            try
            {
                sw.Restart();
                var exist = _userRepository.GetByUsername(obj.UserName);
                if (exist != null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Post]. Post by username = {obj.UserName} error. Detail: Name is exist in system");
                    return BadRequest("Tên đã tồn tại trong hệ thống!");
                }

                var model = new CMS_User()
                {
                    FullName = obj.FullName,
                    UserName = obj.UserName,
                    Email = obj.Email,
                    Password = _passwordHasher.GenerateIdentityV3Hash(obj.Password),
                    Address = obj.Address,
                    ActiveCode = obj.ActiveCode,
                    Mobile = obj.Mobile,
                    RefreshToken = "",
                };

                model.IsActive = true;
                model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                model.CreatedDate = model.UpdatedDate = DateTime.Now;

                await _userRepository.AddAsync(model);

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Post]. Post by username = {obj.UserName} success. Time process: {sw.Elapsed.Seconds} seconds.");

                return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Post]. Post by username = {obj.UserName} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CMS_User>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _userRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                var existRoleUser = _roleuserRepository.GetByIdRole(obj.Id);
                if (!existRoleUser.Any())
                {
                    await _userRepository.DeleteAsync(obj);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok();
                }
                else
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Delete]. Delete by id = {id} error. Detail:  User have relationship data");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[CMS_User.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _userRepository.Get(id.Value) == null;
        }
    }
}