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
    public class NhiemVuController : BaseController<NhiemVuController>
    {
        private readonly INhiemVuRepository _nhiemvuRepository;
        private readonly ISanPhamRepository _sanphamRepository;
        private readonly IUserProvider _userProvider;
        private readonly IHoSoDieuChinhKhacRepository _hoSoDieuChinhKhacRepository;
        private readonly IHoSoDieuChinhKinhPhiRepository _hoSoDieuChinhKinhPhiRepository;
        private readonly IHoSoDieuChinhThoiGianRepository _hoSoDieuChinhThoiGianRepository;
        private readonly IHoSoNghiemThuRepository _hoSoNghiemThuRepository;
        private readonly IHoSoQuyetToanRepository _hoSoQuyetToanRepository;
        private readonly IHoSoXetDuyetRepository _hoSoXetDuyetRepository;
        private readonly IKinhPhiThucHienRepository _kinhPhiThucHienRepository;
        private readonly IThoiGianThucHienRepository _thoiGianThucHienRepository;

        public NhiemVuController(IHoSoDieuChinhKhacRepository hoSoDieuChinhKhacRepository, IHoSoDieuChinhKinhPhiRepository hoSoDieuChinhKinhPhiRepository, IHoSoDieuChinhThoiGianRepository hoSoDieuChinhThoiGianRepository, IHoSoNghiemThuRepository hoSoNghiemThuRepository, IHoSoQuyetToanRepository hoSoQuyetToanRepository, IHoSoXetDuyetRepository hoSoXetDuyetRepository, IKinhPhiThucHienRepository kinhPhiThucHienRepository, IThoiGianThucHienRepository thoiGianThucHienRepository, ISanPhamRepository sanphamRepository, INhiemVuRepository nhiemvuRepository, IUserProvider userProvider)
        {
            _nhiemvuRepository = nhiemvuRepository;
            _sanphamRepository = sanphamRepository;
            _userProvider = userProvider;
            _hoSoDieuChinhKhacRepository = _ = hoSoDieuChinhKhacRepository;
            _hoSoDieuChinhKinhPhiRepository = hoSoDieuChinhKinhPhiRepository;
            _hoSoDieuChinhThoiGianRepository = hoSoDieuChinhThoiGianRepository;
            _hoSoNghiemThuRepository = hoSoNghiemThuRepository;
            _hoSoQuyetToanRepository = hoSoQuyetToanRepository;
            _hoSoXetDuyetRepository = hoSoXetDuyetRepository;
            _kinhPhiThucHienRepository = kinhPhiThucHienRepository;
            _thoiGianThucHienRepository = thoiGianThucHienRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KHCN_NhiemVu>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _nhiemvuRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<KHCN_NhiemVu>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(KHCN_NhiemVu.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_nhiemvuRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KHCN_NhiemVu>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _nhiemvuRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]KHCN_NhiemVu_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var res = _nhiemvuRepository.Get(id);
                    if (res == null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Put]. Put by id = {id} error. Detail: MasoNV [{obj.MaSoNhiemVu}] is exist in system");
                        return BadRequest();
                    }

                    var exist = _nhiemvuRepository.GetByMaSoNV(obj.MaSoNhiemVu);
                    if (exist != null && exist.Id != res.Id)
                        return BadRequest("Mã số nhiệm vụ đã tồn tại trong hệ thống!");

                    var model = new KHCN_NhiemVu()
                    {
                        Id = obj.Id.Value,
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        IdLoaiNhiemVu = obj.IdLoaiNhiemVu,
                        LoaiNhiemVu = obj.LoaiNhiemVu,
                        IdNganh = obj.IdNganh,
                        TenNganh = obj.TenNganh,
                        IdCapQuanLy = obj.IdCapQuanLy,
                        CapQuanLy = obj.CapQuanLy,
                        IdDonViChuTri = obj.IdDonViChuTri,
                        DonViChuTri = obj.DonViChuTri,
                        NamPheDuyet = obj.NamPheDuyet,
                        NamHoanThanh = obj.NamHoanThanh,
                        IdChuNhiemNV = obj.IdChuNhiemNV,
                        ChuNhiemNV = obj.ChuNhiemNV,
                        EmailChuNhiem = obj.EmailChuNhiem,
                        IdTienDoThucHien = obj.IdTienDoThucHien,
                        TienDoThucHien = obj.TienDoThucHien,
                        CreatedBy = res.CreatedBy,
                        CreatedDate = res.CreatedDate,
                        UpdatedBy = _userProvider.GetUserName(),
                        UpdatedDate = DateTime.Now
                    };

                    await _nhiemvuRepository.UpdateAsync(model, model.Id);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok(obj);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Put]. ModelState invalid. Detail: {errors}");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<KHCN_NhiemVu>> Post([FromBody]KHCN_NhiemVu_DTO obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var exist = _nhiemvuRepository.GetByMaSoNV(obj.MaSoNhiemVu);
                    if (exist != null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Post]. Post by masonv = {obj.MaSoNhiemVu} error. Detail: MaSoNhiemVu is exist in system");
                        return BadRequest("Mã số nhiệm vụ đã tồn tại trong hệ thống!");
                    }

                    var model = new KHCN_NhiemVu()
                    {
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        IdLoaiNhiemVu = obj.IdLoaiNhiemVu,
                        LoaiNhiemVu = obj.LoaiNhiemVu,
                        IdNganh = obj.IdNganh,
                        TenNganh = obj.TenNganh,
                        IdCapQuanLy = obj.IdCapQuanLy,
                        CapQuanLy = obj.CapQuanLy,
                        IdDonViChuTri = obj.IdDonViChuTri,
                        DonViChuTri = obj.DonViChuTri,
                        NamPheDuyet = obj.NamPheDuyet,
                        NamHoanThanh = obj.NamHoanThanh,
                        IdChuNhiemNV = obj.IdChuNhiemNV,
                        ChuNhiemNV = obj.ChuNhiemNV,
                        EmailChuNhiem = obj.EmailChuNhiem,
                        IdTienDoThucHien = obj.IdTienDoThucHien,
                        TienDoThucHien = obj.TienDoThucHien,
                    };

                    model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                    model.CreatedDate = model.UpdatedDate = DateTime.Now;
                    await _nhiemvuRepository.AddAsync(model);

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Post]. Post by masonv = {obj.MaSoNhiemVu} success. Time process: {sw.Elapsed.Seconds} seconds.");

                    return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Post]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Post]. Post by masonv = {obj.MaSoNhiemVu} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KHCN_NhiemVu>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _nhiemvuRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                var existSanPham = _sanphamRepository.GetByMaSoNhiemVu(obj.MaSoNhiemVu);
                var existHsDieuChinhKhac = _hoSoDieuChinhKhacRepository.GetByMaSoNhiemVu(obj.MaSoNhiemVu);
                var existHsDieuChinhKinhPhi = _hoSoDieuChinhKinhPhiRepository.GetByMaSoNhiemVu(obj.MaSoNhiemVu);
                var existHsDieuChinhThoiGian = _hoSoDieuChinhThoiGianRepository.GetByMaSoNhiemVu(obj.MaSoNhiemVu);
                var existHsNghiemThu = _hoSoNghiemThuRepository.GetByMaSoNhiemVu(obj.MaSoNhiemVu);
                var existHsQuyetToan = _hoSoQuyetToanRepository.GetByMaSoNhiemVu(obj.MaSoNhiemVu);
                var existHsXetDuyet = _hoSoXetDuyetRepository.GetByMaSoNhiemVu(obj.MaSoNhiemVu);
                var existKinhPhiThucHien = _kinhPhiThucHienRepository.GetByMaSoNhiemVu(obj.MaSoNhiemVu);
                var existThoiGianThucHien = _thoiGianThucHienRepository.GetByMaSoNhiemVu(obj.MaSoNhiemVu);

                if (!existSanPham.Any() && !existHsDieuChinhKhac.Any() && !existHsDieuChinhKinhPhi.Any() &&
                    !existHsDieuChinhThoiGian.Any() && !existHsNghiemThu.Any() && !existHsQuyetToan.Any() &&
                    !existHsXetDuyet.Any() && !existKinhPhiThucHien.Any() && !existThoiGianThucHien.Any())
                {
                    await _nhiemvuRepository.DeleteAsync(obj);
                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok();
                }
                else
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Delete]. Delete by id = {id} error. Detail:  NhiemVu have relationship data");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_NhiemVu.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _nhiemvuRepository.Get(id.Value) == null;
        }
    }
}