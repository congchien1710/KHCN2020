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
    public class SanPhamController : BaseController<SanPhamController>
    {
        private readonly ISanPhamRepository _sanphamRepository;
        private readonly IUserProvider _userProvider;

        public SanPhamController(ISanPhamRepository sanphamRepository, IUserProvider userProvider)
        {
            _sanphamRepository = sanphamRepository;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KHCN_SanPham>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _sanphamRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<KHCN_SanPham>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(KHCN_SanPham.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_sanphamRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KHCN_SanPham>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _sanphamRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]KHCN_SanPham_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var res = _sanphamRepository.Get(id);
                    if (res == null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Put]. Put by masp = {id} error. Detail: Name [{obj.MaSanPham}] is exist in system");
                        return BadRequest();
                    }

                    var exist = _sanphamRepository.GetByMaSP(obj.MaSanPham);
                    if (exist != null && exist.Id != res.Id)
                        return BadRequest("Mã sản phẩm đã tồn tại trong hệ thống!");

                    var model = new KHCN_SanPham()
                    {
                        Id = obj.Id.Value,
                        MaSanPham = obj.MaSanPham,
                        SerialNumber = obj.SerialNumber,
                        TenSanPham = obj.TenSanPham,
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        IdNganh = obj.IdNganh,
                        TenNganh = obj.TenNganh,
                        IdDonViCheTao = obj.IdDonViCheTao,
                        DonViCheTao = obj.DonViCheTao,
                        MaGiaiDoan = obj.MaGiaiDoan,
                        TenGiaiDoan = obj.TenGiaiDoan,
                        IdChuanApDung = obj.IdChuanApDung,
                        ChuanApDung = obj.ChuanApDung,
                        TinhNangChinh = obj.TinhNangChinh,
                        SanPhamTuongDuong = obj.SanPhamTuongDuong,
                        CreatedBy = res.CreatedBy,
                        CreatedDate = res.CreatedDate,
                        UpdatedBy = _userProvider.GetUserName(),
                        UpdatedDate = DateTime.Now
                    };

                    await _sanphamRepository.UpdateAsync(model, model.Id);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Put]. Put by masp = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok(obj);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Put]. ModelState invalid. Detail: {errors}");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<KHCN_SanPham>> Post([FromBody]KHCN_SanPham_DTO obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var res = _sanphamRepository.GetByMaSP(obj.MaSanPham);
                    if (res != null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Post]. Post by masp = {obj.MaSanPham} error. Detail: Name is exist in system");
                        return BadRequest("Tên đã tồn tại trong hệ thống!");
                    }

                    var model = new KHCN_SanPham()
                    {
                        MaSanPham = obj.MaSanPham,
                        SerialNumber = obj.SerialNumber,
                        TenSanPham = obj.TenSanPham,
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        IdNganh = obj.IdNganh,
                        TenNganh = obj.TenNganh,
                        IdDonViCheTao = obj.IdDonViCheTao,
                        DonViCheTao = obj.DonViCheTao,
                        MaGiaiDoan = obj.MaGiaiDoan,
                        TenGiaiDoan = obj.TenGiaiDoan,
                        IdChuanApDung = obj.IdChuanApDung,
                        ChuanApDung = obj.ChuanApDung,
                        TinhNangChinh = obj.TinhNangChinh,
                        SanPhamTuongDuong = obj.SanPhamTuongDuong,
                        CreatedBy = res.CreatedBy,
                        CreatedDate = res.CreatedDate,
                        UpdatedBy = _userProvider.GetUserName(),
                        UpdatedDate = DateTime.Now
                    };

                    model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                    model.CreatedDate = model.UpdatedDate = DateTime.Now;
                    await _sanphamRepository.AddAsync(model);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Post]. Post by masp = {obj.MaSanPham} success. Time process: {sw.Elapsed.Seconds} seconds.");

                    return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Post]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Post]. Post by masp = {obj.MaSanPham} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KHCN_SanPham>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _sanphamRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                await _sanphamRepository.DeleteAsync(obj);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok();
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_SanPham.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _sanphamRepository.Get(id.Value) == null;
        }
    }
}