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
using KHCN.Data.Interfaces;
using KHCN.Service.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace KHCN.Api.Controllers.KHCN
{
    [ApiController]
    [ApiAuthorize]
    [Route("api/[controller]/[action]")]
    public class HoSoXetDuyetController : BaseController<HoSoXetDuyetController>
    {
        private readonly IHoSoXetDuyetRepository _hosoxetduyetRepository;
        private readonly ITaiLieuDinhKemRepository _taiLieuDinhKemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IPathProvider _pathProvider;
        private readonly IUserProvider _userProvider;

        public HoSoXetDuyetController(IHoSoXetDuyetRepository hosoxetduyetRepository, ITaiLieuDinhKemRepository taiLieuDinhKemRepository, IUnitOfWork unitOfWork, FileService fileService, IConfiguration configuration, IPathProvider pathProvider, IUserProvider userProvider)
        {
            _hosoxetduyetRepository = hosoxetduyetRepository;
            _taiLieuDinhKemRepository = taiLieuDinhKemRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _configuration = configuration;
            _pathProvider = pathProvider;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KHCN_HoSoXetDuyet>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _hosoxetduyetRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<KHCN_HoSoXetDuyet>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(KHCN_HoSoXetDuyet.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_hosoxetduyetRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KHCN_HoSoXetDuyet>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _hosoxetduyetRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromForm]KHCN_HoSoXetDuyet_DTO obj, List<IFormFile> lstFile)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var currrentObj = _hosoxetduyetRepository.Get(id);
                    if (currrentObj == null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                        return BadRequest();
                    }

                    var model = new KHCN_HoSoXetDuyet()
                    {
                        Id = obj.Id.Value,
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        PhieuDangKy = currrentObj.PhieuDangKy,
                        NgayDangKy = DateTime.ParseExact(obj.NgayDangKy.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDLapHDXDCapVien = currrentObj.QDLapHDXDCapVien,
                        NgayQDLapHDXDCapVien = DateTime.ParseExact(obj.NgayQDLapHDXDCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BBHopHDXDCapVien = currrentObj.BBHopHDXDCapVien,
                        NgayHopHDXDCapVien = DateTime.ParseExact(obj.NgayHopHDXDCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDLapHDXDCapTapDoan = currrentObj.QDLapHDXDCapTapDoan,
                        NgayQDLapHDXDCapTapDoan = DateTime.ParseExact(obj.NgayQDLapHDXDCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BBHopHDXDCapTapDoan = currrentObj.BBHopHDXDCapTapDoan,
                        NgayHopHDXDCapTapDoan = DateTime.ParseExact(obj.NgayHopHDXDCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ToTrinhXinPDChuTruong = currrentObj.ToTrinhXinPDChuTruong,
                        NgayTrinhXinPDChuTruong = DateTime.ParseExact(obj.NgayTrinhXinPDChuTruong.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ToTrinhXinPDNhiemVu = currrentObj.ToTrinhXinPDNhiemVu,
                        NgayTrinhXinPDNhiemVu = DateTime.ParseExact(obj.NgayTrinhXinPDNhiemVu.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDPheDuyet = currrentObj.QDPheDuyet,
                        NgayQDPheDuyet = DateTime.ParseExact(obj.NgayQDPheDuyet.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDGiaoNhiemVu = currrentObj.QDGiaoNhiemVu,
                        NgayQDGiaoNhiemVu = DateTime.ParseExact(obj.NgayQDGiaoNhiemVu.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ThuyetMinhNhiemVu = currrentObj.ThuyetMinhNhiemVu,
                        NgayThuyetMinhNhiemVu = DateTime.ParseExact(obj.NgayThuyetMinhNhiemVu.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        HoSoDuToan = currrentObj.HoSoDuToan,
                        NgayLapHoSoDuToan = DateTime.ParseExact(obj.NgayLapHoSoDuToan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        TaiLieuMRD = currrentObj.TaiLieuMRD,
                        NgayLapTaiLieuMRD = DateTime.ParseExact(obj.NgayLapTaiLieuMRD.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        TaiLieuPRD = currrentObj.TaiLieuPRD,
                        NgayLapTaiLieuPRD = DateTime.ParseExact(obj.NgayLapTaiLieuPRD.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        CreatedBy = currrentObj.CreatedBy,
                        CreatedDate = currrentObj.CreatedDate,
                        UpdatedBy = _userProvider.GetUserName(),
                        UpdatedDate = DateTime.Now
                    };

                    var lstInfoAttachmentAdd = new List<KHCN_TaiLieuDinhKem>();
                    var lstGuidAttachmentDel = new List<string>();

                    if (Request.Form.Files.Count > 0)
                    {
                        var files = Request.Form.Files.ToList();
                        if (files.Count > 0)
                        {
                            var relativeRoot = _configuration["Jwt:RootPathUpload"];
                            var rootPath = _pathProvider.MapPath(relativeRoot);
                            var res = _fileService.SaveFile(rootPath, relativeRoot, obj.MaSoNhiemVu, files);
                            if (res.Key)
                            {
                                var now = DateTime.Now;
                                var currentUser = _userProvider.GetUserName();
                                lstInfoAttachmentAdd = res.Value.Select(m => new KHCN_TaiLieuDinhKem
                                {
                                    Guid = Guid.NewGuid().ToString(),
                                    LoaiTaiLieu = (byte)LoaiTaiLieuEnum.HOSOXETDUYET,
                                    MaSoNhiemVu = obj.MaSoNhiemVu,
                                    MaSanPham = "",
                                    Keyword = m.Keyword,
                                    Description = m.Keyword,
                                    RelativePath = m.RelativePath,
                                    FileName = m.FileName,
                                    DisplayFileName = m.DisplayName,
                                    FileSize = m.FileSize,
                                    CreatedDate = now,
                                    UpdatedDate = now,
                                    CreatedBy = currentUser,
                                    UpdatedBy = currentUser
                                }).ToList();

                                var guidsPHIEUDANGKY = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("PHIEUDANGKY", StringComparison.OrdinalIgnoreCase));
                                var guidsQDLAPHDXDCAPVIEN = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDLAPHDXDCAPVIEN", StringComparison.OrdinalIgnoreCase));
                                var guidsBBHOPHDXDCAPVIEN = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("BBHOPHDXDCAPVIEN", StringComparison.OrdinalIgnoreCase));
                                var guidsQDLAPHDXDCAPTAPDOAN = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDLAPHDXDCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                                var guidsBBHOPHDXDCAPTAPDOAN = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("BBHOPHDXDCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                                var guidsTOTRINHXINPDCHUTRUONG = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TOTRINHXINPDCHUTRUONG", StringComparison.OrdinalIgnoreCase));
                                var guidsTOTRINHXINPDNHIEMVU = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TOTRINHXINPDNHIEMVU", StringComparison.OrdinalIgnoreCase));
                                var guidsQDPHEDUYET = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDPHEDUYET", StringComparison.OrdinalIgnoreCase));
                                var guidsQDGIAONHIEMVU = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDGIAONHIEMVU", StringComparison.OrdinalIgnoreCase));
                                var guidsTHUYETMINHNHIEMVU = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("THUYETMINHNHIEMVU", StringComparison.OrdinalIgnoreCase));
                                var guidsHOSODUTOAN = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("HOSODUTOAN", StringComparison.OrdinalIgnoreCase));
                                var guidsTAILIEUMRD = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TAILIEUMRD", StringComparison.OrdinalIgnoreCase));
                                var guidsTAILIEUPRD = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TAILIEUPRD", StringComparison.OrdinalIgnoreCase));

                                if (guidsPHIEUDANGKY != null)
                                {
                                    lstGuidAttachmentDel.Add(model.PhieuDangKy);
                                    model.PhieuDangKy = guidsPHIEUDANGKY.Guid;
                                }
                                if (guidsQDLAPHDXDCAPVIEN != null)
                                {
                                    lstGuidAttachmentDel.Add(model.QDLapHDXDCapVien);
                                    model.QDLapHDXDCapVien = guidsQDLAPHDXDCAPVIEN.Guid;
                                }
                                if (guidsBBHOPHDXDCAPVIEN != null)
                                {
                                    lstGuidAttachmentDel.Add(model.BBHopHDXDCapVien);
                                    model.BBHopHDXDCapVien = guidsBBHOPHDXDCAPVIEN.Guid;
                                }
                                if (guidsQDLAPHDXDCAPTAPDOAN != null)
                                {
                                    lstGuidAttachmentDel.Add(model.QDLapHDXDCapTapDoan);
                                    model.QDLapHDXDCapTapDoan = guidsQDLAPHDXDCAPTAPDOAN.Guid;
                                }
                                if (guidsBBHOPHDXDCAPTAPDOAN != null)
                                {
                                    lstGuidAttachmentDel.Add(model.BBHopHDXDCapTapDoan);
                                    model.BBHopHDXDCapTapDoan = guidsBBHOPHDXDCAPTAPDOAN.Guid;
                                }
                                if (guidsTOTRINHXINPDCHUTRUONG != null)
                                {
                                    lstGuidAttachmentDel.Add(model.ToTrinhXinPDChuTruong);
                                    model.ToTrinhXinPDChuTruong = guidsTOTRINHXINPDCHUTRUONG.Guid;
                                }
                                if (guidsTOTRINHXINPDNHIEMVU != null)
                                {
                                    lstGuidAttachmentDel.Add(model.ToTrinhXinPDNhiemVu);
                                    model.ToTrinhXinPDNhiemVu = guidsTOTRINHXINPDNHIEMVU.Guid;
                                }
                                if (guidsQDPHEDUYET != null)
                                {
                                    lstGuidAttachmentDel.Add(model.QDPheDuyet);
                                    model.QDPheDuyet = guidsQDPHEDUYET.Guid;
                                }
                                if (guidsQDGIAONHIEMVU != null)
                                {
                                    lstGuidAttachmentDel.Add(model.QDGiaoNhiemVu);
                                    model.QDGiaoNhiemVu = guidsQDGIAONHIEMVU.Guid;
                                }
                                if (guidsTHUYETMINHNHIEMVU != null)
                                {
                                    lstGuidAttachmentDel.Add(model.ThuyetMinhNhiemVu);
                                    model.ThuyetMinhNhiemVu = guidsTHUYETMINHNHIEMVU.Guid;
                                }
                                if (guidsHOSODUTOAN != null)
                                {
                                    lstGuidAttachmentDel.Add(model.HoSoDuToan);
                                    model.HoSoDuToan = guidsHOSODUTOAN.Guid;
                                }
                                if (guidsTAILIEUMRD != null)
                                {
                                    lstGuidAttachmentDel.Add(model.TaiLieuMRD);
                                    model.TaiLieuMRD = guidsTAILIEUMRD.Guid;
                                }
                                if (guidsTAILIEUPRD != null)
                                {
                                    lstGuidAttachmentDel.Add(model.TaiLieuPRD);
                                    model.TaiLieuPRD = guidsTAILIEUPRD.Guid;
                                }
                            }
                            else
                                return BadRequest($"Error: Upload file thất bại!");
                        }
                    }

                    _unitOfWork.Context.Set<KHCN_HoSoXetDuyet>().Update(model);
                    if (lstInfoAttachmentAdd != null && lstInfoAttachmentAdd.Any())
                        _unitOfWork.Context.Set<KHCN_TaiLieuDinhKem>().AddRange(lstInfoAttachmentAdd);

                    if (lstGuidAttachmentDel != null && lstGuidAttachmentDel.Any())
                    {
                        var lstInfoAttachmentDel = _taiLieuDinhKemRepository.GetByGuid(lstGuidAttachmentDel);
                        if (lstInfoAttachmentDel != null && lstInfoAttachmentDel.Any())
                        {
                            _unitOfWork.Context.Set<KHCN_TaiLieuDinhKemDel>().AddRange(lstInfoAttachmentDel.Select(m => new KHCN_TaiLieuDinhKemDel
                            {
                                Guid = m.Guid,
                                LoaiTaiLieu = m.LoaiTaiLieu,
                                MaSoNhiemVu = m.MaSoNhiemVu,
                                MaSanPham = m.MaSanPham,
                                Keyword = m.Keyword,
                                Description = m.Description,
                                RelativePath = m.RelativePath,
                                FileName = m.FileName,
                                DisplayFileName = m.DisplayFileName,
                                FileSize = m.FileSize,
                                CreatedDate = m.CreatedDate,
                                UpdatedDate = m.UpdatedDate,
                                CreatedBy = m.CreatedBy,
                                UpdatedBy = m.UpdatedBy
                            }).ToList());

                            _unitOfWork.Context.Set<KHCN_TaiLieuDinhKem>().RemoveRange(lstInfoAttachmentDel);

                            new FileService().MoveFile(lstInfoAttachmentDel.Select(m => $"{m.RelativePath}/{m.FileName}").ToList());
                        }
                    }

                    _unitOfWork.Commit();

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok(obj);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Put]. ModelState invalid. Detail: {errors}");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult<KHCN_HoSoXetDuyet> Post([FromForm]KHCN_HoSoXetDuyet_DTO obj, List<IFormFile> lstFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();

                    var model = new KHCN_HoSoXetDuyet()
                    {
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        PhieuDangKy = obj.PhieuDangKy,
                        NgayDangKy = DateTime.ParseExact(obj.NgayDangKy.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDLapHDXDCapVien = obj.QDLapHDXDCapVien,
                        NgayQDLapHDXDCapVien = DateTime.ParseExact(obj.NgayQDLapHDXDCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BBHopHDXDCapVien = obj.BBHopHDXDCapVien,
                        NgayHopHDXDCapVien = DateTime.ParseExact(obj.NgayHopHDXDCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDLapHDXDCapTapDoan = obj.QDLapHDXDCapTapDoan,
                        NgayQDLapHDXDCapTapDoan = DateTime.ParseExact(obj.NgayQDLapHDXDCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BBHopHDXDCapTapDoan = obj.BBHopHDXDCapTapDoan,
                        NgayHopHDXDCapTapDoan = DateTime.ParseExact(obj.NgayHopHDXDCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ToTrinhXinPDChuTruong = obj.ToTrinhXinPDChuTruong,
                        NgayTrinhXinPDChuTruong = DateTime.ParseExact(obj.NgayTrinhXinPDChuTruong.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ToTrinhXinPDNhiemVu = obj.ToTrinhXinPDNhiemVu,
                        NgayTrinhXinPDNhiemVu = DateTime.ParseExact(obj.NgayTrinhXinPDNhiemVu.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDPheDuyet = obj.QDPheDuyet,
                        NgayQDPheDuyet = DateTime.ParseExact(obj.NgayQDPheDuyet.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDGiaoNhiemVu = obj.QDGiaoNhiemVu,
                        NgayQDGiaoNhiemVu = DateTime.ParseExact(obj.NgayQDGiaoNhiemVu.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ThuyetMinhNhiemVu = obj.ThuyetMinhNhiemVu,
                        NgayThuyetMinhNhiemVu = DateTime.ParseExact(obj.NgayThuyetMinhNhiemVu.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        HoSoDuToan = obj.HoSoDuToan,
                        NgayLapHoSoDuToan = DateTime.ParseExact(obj.NgayLapHoSoDuToan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        TaiLieuMRD = obj.TaiLieuMRD,
                        NgayLapTaiLieuMRD = DateTime.ParseExact(obj.NgayLapTaiLieuMRD.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        TaiLieuPRD = obj.TaiLieuPRD,
                        NgayLapTaiLieuPRD = DateTime.ParseExact(obj.NgayLapTaiLieuPRD.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    };

                    model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                    model.CreatedDate = model.UpdatedDate = DateTime.Now;
                    var lstInfoAttachment = new List<KHCN_TaiLieuDinhKem>();
                    lstFile = new List<IFormFile>();

                    if (Request.Form.Files.Count == 0)
                    {
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKhac.Post]. ModelState invalid. Detail: File upload is null");
                        return ValidationProblem("File upload is null");
                    }
                    else
                    {
                        lstFile = Request.Form.Files.ToList();
                        if (!lstFile.Select(m => m.Name.ToUpper()).Contains("PHIEUDANGKY") || !lstFile.Select(m => m.Name.ToUpper()).Contains("QDLAPHDXDCAPVIEN") ||
                            !lstFile.Select(m => m.Name.ToUpper()).Contains("BBHOPHDXDCAPVIEN") || !lstFile.Select(m => m.Name.ToUpper()).Contains("QDLAPHDXDCAPTAPDOAN") ||
                            !lstFile.Select(m => m.Name.ToUpper()).Contains("BBHOPHDXDCAPTAPDOAN") || !lstFile.Select(m => m.Name.ToUpper()).Contains("TOTRINHXINPDCHUTRUONG") ||
                            !lstFile.Select(m => m.Name.ToUpper()).Contains("TOTRINHXINPDNHIEMVU") || !lstFile.Select(m => m.Name.ToUpper()).Contains("QDPHEDUYET") ||
                            !lstFile.Select(m => m.Name.ToUpper()).Contains("QDGIAONHIEMVU") || !lstFile.Select(m => m.Name.ToUpper()).Contains("THUYETMINHNHIEMVU") ||
                            !lstFile.Select(m => m.Name.ToUpper()).Contains("HOSODUTOAN") || !lstFile.Select(m => m.Name.ToUpper()).Contains("TAILIEUMRD") ||
                            !lstFile.Select(m => m.Name.ToUpper()).Contains("TAILIEUPRD"))
                        {
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("PHIEUDANGKY"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File PHIEUDANGKY is null");
                                return ValidationProblem("File PHIEUDANGKY is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("QDLAPHDXDCAPVIEN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File QDLAPHDXDCAPVIEN is null");
                                return ValidationProblem("File QDLAPHDXDCAPVIEN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("BBHOPHDXDCAPVIEN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File BBHOPHDXDCAPVIEN is null");
                                return ValidationProblem("File BBHOPHDXDCAPVIEN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("QDLAPHDXDCAPTAPDOAN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File QDLAPHDXDCAPTAPDOAN is null");
                                return ValidationProblem("File QDLAPHDXDCAPTAPDOAN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("BBHOPHDXDCAPTAPDOAN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File BBHOPHDXDCAPTAPDOAN is null");
                                return ValidationProblem("File BBHOPHDXDCAPTAPDOAN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("TOTRINHXINPDCHUTRUONG"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File TOTRINHXINPDCHUTRUONG is null");
                                return ValidationProblem("File TOTRINHXINPDCHUTRUONG is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("TOTRINHXINPDNHIEMVU"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File TOTRINHXINPDNHIEMVU is null");
                                return ValidationProblem("File TOTRINHXINPDNHIEMVU is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("QDPHEDUYET"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File QDPHEDUYET is null");
                                return ValidationProblem("File QDPHEDUYET is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("QDGIAONHIEMVU"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File QDGIAONHIEMVU is null");
                                return ValidationProblem("File QDGIAONHIEMVU is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("THUYETMINHNHIEMVU"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File THUYETMINHNHIEMVU is null");
                                return ValidationProblem("File THUYETMINHNHIEMVU is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("HOSODUTOAN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File HOSODUTOAN is null");
                                return ValidationProblem("File HOSODUTOAN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("TAILIEUMRD"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File TAILIEUMRD is null");
                                return ValidationProblem("File TAILIEUMRD is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("TAILIEUPRD"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: File TAILIEUPRD is null");
                                return ValidationProblem("File TAILIEUPRD is null");
                            }
                        }
                    }

                    var relativeRoot = _configuration["Jwt:RootPathUpload"];
                    var rootPath = _pathProvider.MapPath(relativeRoot);
                    var res = _fileService.SaveFile(rootPath, relativeRoot, obj.MaSoNhiemVu, lstFile);
                    if (res.Key)
                    {
                        var now = DateTime.Now;
                        var currentUser = _userProvider.GetUserName();
                        lstInfoAttachment = res.Value.Select(m => new KHCN_TaiLieuDinhKem
                        {
                            Guid = Guid.NewGuid().ToString(),
                            LoaiTaiLieu = (byte)LoaiTaiLieuEnum.HOSODIEUCHINH,
                            MaSoNhiemVu = obj.MaSoNhiemVu,
                            MaSanPham = "",
                            Keyword = m.Keyword,
                            Description = m.Keyword,
                            RelativePath = m.RelativePath,
                            FileName = m.FileName,
                            DisplayFileName = m.DisplayName,
                            FileSize = m.FileSize,
                            CreatedDate = now,
                            UpdatedDate = now,
                            CreatedBy = currentUser,
                            UpdatedBy = currentUser
                        }).ToList();

                        var guidsPHIEUDANGKY = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("PHIEUDANGKY", StringComparison.OrdinalIgnoreCase));
                        var guidsQDLAPHDXDCAPVIEN = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDLAPHDXDCAPVIEN", StringComparison.OrdinalIgnoreCase));
                        var guidsBBHOPHDXDCAPVIEN = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("BBHOPHDXDCAPVIEN", StringComparison.OrdinalIgnoreCase));
                        var guidsQDLAPHDXDCAPTAPDOAN = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDLAPHDXDCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                        var guidsBBHOPHDXDCAPTAPDOAN = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("BBHOPHDXDCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                        var guidsTOTRINHXINPDCHUTRUONG = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TOTRINHXINPDCHUTRUONG", StringComparison.OrdinalIgnoreCase));
                        var guidsTOTRINHXINPDNHIEMVU = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TOTRINHXINPDNHIEMVU", StringComparison.OrdinalIgnoreCase));
                        var guidsQDPHEDUYET = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDPHEDUYET", StringComparison.OrdinalIgnoreCase));
                        var guidsQDGIAONHIEMVU = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDGIAONHIEMVU", StringComparison.OrdinalIgnoreCase));
                        var guidsTHUYETMINHNHIEMVU = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("THUYETMINHNHIEMVU", StringComparison.OrdinalIgnoreCase));
                        var guidsHOSODUTOAN = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("HOSODUTOAN", StringComparison.OrdinalIgnoreCase));
                        var guidsTAILIEUMRD = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TAILIEUMRD", StringComparison.OrdinalIgnoreCase));
                        var guidsTAILIEUPRD = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TAILIEUPRD", StringComparison.OrdinalIgnoreCase));

                        model.PhieuDangKy = guidsPHIEUDANGKY != null ? guidsPHIEUDANGKY.Guid : string.Empty;
                        model.QDLapHDXDCapVien = guidsQDLAPHDXDCAPVIEN != null ? guidsQDLAPHDXDCAPVIEN.Guid : string.Empty;
                        model.BBHopHDXDCapVien = guidsBBHOPHDXDCAPVIEN != null ? guidsBBHOPHDXDCAPVIEN.Guid : string.Empty;
                        model.QDLapHDXDCapTapDoan = guidsQDLAPHDXDCAPTAPDOAN != null ? guidsQDLAPHDXDCAPTAPDOAN.Guid : string.Empty;
                        model.BBHopHDXDCapTapDoan = guidsBBHOPHDXDCAPTAPDOAN != null ? guidsBBHOPHDXDCAPTAPDOAN.Guid : string.Empty;
                        model.ToTrinhXinPDChuTruong = guidsTOTRINHXINPDCHUTRUONG != null ? guidsTOTRINHXINPDCHUTRUONG.Guid : string.Empty;
                        model.ToTrinhXinPDNhiemVu = guidsTOTRINHXINPDNHIEMVU != null ? guidsTOTRINHXINPDNHIEMVU.Guid : string.Empty;
                        model.QDPheDuyet = guidsQDPHEDUYET != null ? guidsQDPHEDUYET.Guid : string.Empty;
                        model.QDGiaoNhiemVu = guidsQDGIAONHIEMVU != null ? guidsQDGIAONHIEMVU.Guid : string.Empty;
                        model.ThuyetMinhNhiemVu = guidsTHUYETMINHNHIEMVU != null ? guidsTHUYETMINHNHIEMVU.Guid : string.Empty;
                        model.HoSoDuToan = guidsHOSODUTOAN != null ? guidsHOSODUTOAN.Guid : string.Empty;
                        model.TaiLieuMRD = guidsTAILIEUMRD != null ? guidsTAILIEUMRD.Guid : string.Empty;
                        model.TaiLieuPRD = guidsTAILIEUPRD != null ? guidsTAILIEUPRD.Guid : string.Empty;
                    }
                    else
                        return BadRequest($"Error: Upload file thất bại!");

                    _unitOfWork.Context.Set<KHCN_HoSoXetDuyet>().Add(model);
                    _unitOfWork.Context.Set<KHCN_TaiLieuDinhKem>().AddRange(lstInfoAttachment);
                    _unitOfWork.Commit();

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. Post by masonv = {obj.MaSoNhiemVu} success. Time process: {sw.Elapsed.Seconds} seconds.");

                    return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Post]. Post by masonv = {obj.MaSoNhiemVu} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KHCN_HoSoXetDuyet>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _hosoxetduyetRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                await _hosoxetduyetRepository.DeleteAsync(obj);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok();
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoXetDuyet.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _hosoxetduyetRepository.Get(id.Value) == null;
        }
    }
}