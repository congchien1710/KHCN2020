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
    public class ThanhVienDeTaiController : BaseController<ThanhVienDeTaiController>
    {
        private readonly IThanhVienDeTaiRepository _thanhviendetaiRepository;
        private readonly INhiemVuRepository _nhiemvuRepository;
        private readonly IUserProvider _userProvider;

        public ThanhVienDeTaiController(IThanhVienDeTaiRepository thanhviendetaiRepository, INhiemVuRepository nhiemvuRepository, IUserProvider userProvider)
        {
            _thanhviendetaiRepository = thanhviendetaiRepository;
            _nhiemvuRepository = nhiemvuRepository;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KHCN_ThanhVienDeTai>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _thanhviendetaiRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<KHCN_ThanhVienDeTai>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(KHCN_ThanhVienDeTai.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_thanhviendetaiRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KHCN_ThanhVienDeTai>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _thanhviendetaiRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]KHCN_ThanhVienDeTai_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var res = _thanhviendetaiRepository.Get(id);
                    if (res == null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                        return BadRequest();
                    }

                    var exist = _thanhviendetaiRepository.GetByMaNV(obj.MaNV);
                    if (exist != null && exist.Id != res.Id)
                        return BadRequest("Mã nhân viên đã tồn tại trong hệ thống!");

                    var model = new KHCN_ThanhVienDeTai()
                    {
                        Id = obj.Id.Value,
                        MaNV = obj.MaNV,
                        HoTen = obj.HoTen,
                        IdDonVi = obj.IdDonVi,
                        DonVi = obj.DonVi,
                        IdPhongBan = obj.IdPhongBan,
                        PhongBan = obj.PhongBan,
                        IdCapBac = obj.IdCapBac,
                        CapBac = obj.CapBac,
                        IdChucDanh = obj.IdChucDanh,
                        ChucDanh = obj.ChucDanh,
                        TrinhDo = obj.TrinhDo,
                        IdTrinhDo = obj.IdTrinhDo,
                        NgaySinh = DateTime.ParseExact(obj.NgaySinh.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        NgayKyHD = DateTime.ParseExact(obj.NgayKyHD.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        GioiTinh = obj.GioiTinh,
                        Email = obj.Email,
                        NhiemVu1 = obj.NhiemVu1,
                        TrangThaiNV1 = obj.TrangThaiNV1,
                        NhiemVu2 = obj.NhiemVu2,
                        TrangThaiNV2 = obj.TrangThaiNV2,
                        NhiemVu3 = obj.NhiemVu3,
                        TrangThaiNV3 = obj.TrangThaiNV3,
                        IsActive = obj.IsActive,
                        CreatedBy = res.CreatedBy,
                        CreatedDate = res.CreatedDate,
                        UpdatedBy = _userProvider.GetUserName(),
                        UpdatedDate = DateTime.Now
                    };

                    await _thanhviendetaiRepository.UpdateAsync(model, model.Id);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok(obj);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Put]. ModelState invalid. Detail: {errors}");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<KHCN_ThanhVienDeTai>> Post([FromBody]KHCN_ThanhVienDeTai_DTO obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var exist = _thanhviendetaiRepository.GetByMaNV(obj.MaNV);
                    if (exist != null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Post]. Post by manv = {obj.MaNV} error. Detail: Name is exist in system");
                        return BadRequest("Tên đã tồn tại trong hệ thống!");
                    }

                    var model = new KHCN_ThanhVienDeTai()
                    {
                        MaNV = obj.MaNV,
                        HoTen = obj.HoTen,
                        IdDonVi = obj.IdDonVi,
                        DonVi = obj.DonVi,
                        IdPhongBan = obj.IdPhongBan,
                        PhongBan = obj.PhongBan,
                        IdCapBac = obj.IdCapBac,
                        CapBac = obj.CapBac,
                        IdChucDanh = obj.IdChucDanh,
                        ChucDanh = obj.ChucDanh,
                        TrinhDo = obj.TrinhDo,
                        IdTrinhDo = obj.IdTrinhDo,
                        NgaySinh = DateTime.ParseExact(obj.NgaySinh.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        NgayKyHD = DateTime.ParseExact(obj.NgayKyHD.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        GioiTinh = obj.GioiTinh,
                        Email = obj.Email,
                        NhiemVu1 = obj.NhiemVu1,
                        TrangThaiNV1 = obj.TrangThaiNV1,
                        NhiemVu2 = obj.NhiemVu2,
                        TrangThaiNV2 = obj.TrangThaiNV2,
                        NhiemVu3 = obj.NhiemVu3,
                        TrangThaiNV3 = obj.TrangThaiNV3
                    };

                    model.IsActive = true;
                    model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                    model.CreatedDate = model.UpdatedDate = DateTime.Now;
                    await _thanhviendetaiRepository.AddAsync(model);
                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Post]. Post by manv = {obj.MaNV} success. Time process: {sw.Elapsed.Seconds} seconds.");

                    return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Post]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Post]. Post by manv = {obj.MaNV} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KHCN_ThanhVienDeTai>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _thanhviendetaiRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                var exist = _nhiemvuRepository.GetByIdChuNhiemNV(obj.Id);
                if (!exist.Any())
                {
                    await _thanhviendetaiRepository.DeleteAsync(obj);
                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok();
                }
                else
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Delete]. Delete by id = {id} error. Detail:  KHCN_ThanhVienDeTai have relationship data");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_ThanhVienDeTai.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _thanhviendetaiRepository.Get(id.Value) == null;
        }
    }
}