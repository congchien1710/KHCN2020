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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Api.Controllers.KHCN
{
    [ApiController]
    [ApiAuthorize]
    [Route("api/[controller]/[action]")]
    public class GiaiDoanSanPhamController : BaseController<GiaiDoanSanPhamController>
    {
        private readonly IGiaiDoanSanPhamRepository _giaidoansanphamRepository;
        private readonly ISanPhamRepository _sanphamRepository;
        private readonly IUserProvider _userProvider;

        public GiaiDoanSanPhamController(ISanPhamRepository sanphamRepository, IGiaiDoanSanPhamRepository giaidoansanphamRepository, IUserProvider userProvider)
        {
            _giaidoansanphamRepository = giaidoansanphamRepository;
            _sanphamRepository = sanphamRepository;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KHCN_GiaiDoanSanPham>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _giaidoansanphamRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<KHCN_GiaiDoanSanPham>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(KHCN_GiaiDoanSanPham.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_giaidoansanphamRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KHCN_GiaiDoanSanPham>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _giaidoansanphamRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]KHCN_GiaiDoanSanPham_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var res = _giaidoansanphamRepository.Get(id);
                    if (res == null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                        return BadRequest();
                    }

                    var model = new KHCN_GiaiDoanSanPham()
                    {
                        Id = obj.Id.Value,
                        MaGiaiDoan = obj.MaGiaiDoan,
                        TenGiaiDoan = obj.TenGiaiDoan,
                        ThoiGianBatDau = DateTime.ParseExact(obj.ThoiGianBatDau.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ThoiGianKetThuc = DateTime.ParseExact(obj.ThoiGianKetThuc.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        SoLuong = obj.SoLuong,
                        ChiPhiNVL = obj.ChiPhiNVL,
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        CreatedBy = res.CreatedBy,
                        CreatedDate = res.CreatedDate,
                        UpdatedBy = _userProvider.GetUserName(),
                        UpdatedDate = DateTime.Now
                    };

                    await _giaidoansanphamRepository.UpdateAsync(model, model.Id);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok(obj);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Put]. ModelState invalid. Detail: {errors}");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<KHCN_GiaiDoanSanPham>> Post([FromBody]KHCN_GiaiDoanSanPham_DTO obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();

                    var model = new KHCN_GiaiDoanSanPham()
                    {
                        MaGiaiDoan = obj.MaGiaiDoan,
                        TenGiaiDoan = obj.TenGiaiDoan,
                        ThoiGianBatDau = DateTime.ParseExact(obj.ThoiGianBatDau.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ThoiGianKetThuc = DateTime.ParseExact(obj.ThoiGianKetThuc.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        SoLuong = obj.SoLuong,
                        ChiPhiNVL = obj.ChiPhiNVL,
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                    };

                    model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                    model.CreatedDate = model.UpdatedDate = DateTime.Now;
                    await _giaidoansanphamRepository.AddAsync(model);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Post]. Post by masonv = {obj.MaSoNhiemVu} success. Time process: {sw.Elapsed.Seconds} seconds.");

                    return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Post]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Post]. Post by masonv = {obj.MaSoNhiemVu} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KHCN_GiaiDoanSanPham>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _giaidoansanphamRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                var exist = _sanphamRepository.GetByMaGiaiDoanSP(obj.MaGiaiDoan);
                if (!exist.Any())
                {
                    await _giaidoansanphamRepository.DeleteAsync(obj);
                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok();
                }
                else
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Delete]. Delete by id = {id} error. Detail:  GiaiDoanSanPham have relationship data");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_GiaiDoanSanPham.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _giaidoansanphamRepository.Get(id.Value) == null;
        }
    }
}