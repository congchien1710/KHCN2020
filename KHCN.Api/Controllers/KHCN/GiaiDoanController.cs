using KHCN.Api.Provider;
using KHCN.Data.DTOs.KHCN;
using KHCN.Data.Entities.KHCN;
using KHCN.Data.Helpers;
using KHCN.Data.Repository.KHCN;
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
    public class GiaiDoanController : BaseController<GiaiDoanController>
    {
        private readonly IGiaiDoanRepository _giaidoanRepository;
        private readonly IGiaiDoanSanPhamRepository _giaidoanSanPhamRepository;
        private readonly IUserProvider _userProvider;

        public GiaiDoanController(IGiaiDoanSanPhamRepository giaidoanSanPhamRepository, IGiaiDoanRepository giaidoanRepository, IUserProvider userProvider)
        {
            _giaidoanRepository = giaidoanRepository;
            _giaidoanSanPhamRepository = giaidoanSanPhamRepository;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KHCN_GiaiDoan>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _giaidoanRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<KHCN_GiaiDoan>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(KHCN_GiaiDoan.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_giaidoanRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KHCN_GiaiDoan>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _giaidoanRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]KHCN_GiaiDoan_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var res = _giaidoanRepository.Get(id);
                    if (res == null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                        return BadRequest();
                    }

                    var exist = _giaidoanRepository.GetByName(obj.Name);
                    if (exist != null && exist.Id != res.Id)
                        return BadRequest("Tên đã tồn tại trong hệ thống!");

                    var model = new KHCN_GiaiDoan()
                    {
                        Id = obj.Id.Value,
                        Code = obj.Code,
                        Name = obj.Name,
                        Description = obj.Description,
                        IsActive = obj.IsActive,
                        CreatedBy = res.CreatedBy,
                        CreatedDate = res.CreatedDate,
                        UpdatedBy = _userProvider.GetUserName(),
                        UpdatedDate = DateTime.Now
                    };

                    await _giaidoanRepository.UpdateAsync(model, model.Id);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok(obj);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Put]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<KHCN_GiaiDoan>> Post([FromBody]KHCN_GiaiDoan_DTO obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var exist = _giaidoanRepository.GetByName(obj.Name);
                    if (exist != null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Post]. Post by name = {obj.Name} error. Detail: Name is exist in system");
                        return BadRequest("Tên đã tồn tại trong hệ thống!");
                    }

                    var model = new KHCN_GiaiDoan()
                    {
                        Code = obj.Code,
                        Name = obj.Name,
                        Description = obj.Description
                    };

                    model.IsActive = true;
                    model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                    model.CreatedDate = model.UpdatedDate = DateTime.Now;
                    await _giaidoanRepository.AddAsync(model);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Post]. Post by name = {obj.Name} success. Time process: {sw.Elapsed.Seconds} seconds.");

                    return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Post]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Post]. Post by name = {obj.Name} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KHCN_GiaiDoan>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _giaidoanRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                var exist = _giaidoanSanPhamRepository.GetByMaGiaiDoan(obj.Code);
                if (!exist.Any())
                {
                    await _giaidoanRepository.DeleteAsync(obj);
                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok();
                }
                else
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Delete]. Delete by id = {id} error. Detail:  GiaiDoan have relationship data");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoan.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _giaidoanRepository.Get(id.Value) == null;
        }
    }
}