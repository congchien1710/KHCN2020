using KHCN.Api.Provider;
using KHCN.Data.DTOs.KHCN;
using KHCN.Data.Entities.KHCN;
using KHCN.Data.Helpers;
using KHCN.Data.Interfaces;
using KHCN.Data.Repository.KHCN;
using KHCN.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class HoSoNghiemThuController : BaseController<HoSoNghiemThuController>
    {
        private readonly IHoSoNghiemThuRepository _hosonghiemthuRepository;
        private readonly ITaiLieuDinhKemRepository _taiLieuDinhKemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IPathProvider _pathProvider;
        private readonly IUserProvider _userProvider;

        public HoSoNghiemThuController(IHoSoNghiemThuRepository hosonghiemthuRepository, ITaiLieuDinhKemRepository taiLieuDinhKemRepository, IUnitOfWork unitOfWork, FileService fileService, IConfiguration configuration, IPathProvider pathProvider, IUserProvider userProvider)
        {
            _hosonghiemthuRepository = hosonghiemthuRepository;
            _taiLieuDinhKemRepository = taiLieuDinhKemRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _configuration = configuration;
            _pathProvider = pathProvider;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KHCN_HoSoNghiemThu>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _hosonghiemthuRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<KHCN_HoSoNghiemThu>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(KHCN_HoSoNghiemThu.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_hosonghiemthuRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KHCN_HoSoNghiemThu>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _hosonghiemthuRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromForm]KHCN_HoSoNghiemThu_DTO obj, List<IFormFile> lstFile)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var currentObj = _hosonghiemthuRepository.Get(id);
                    if (currentObj == null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                        return BadRequest();
                    }

                    var model = new KHCN_HoSoNghiemThu()
                    {
                        Id = obj.Id.Value,
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        PhieuDNNghiemThuCapVien = currentObj.PhieuDNNghiemThuCapVien,
                        NgayLapPhieuDNNghiemThuCapVien = DateTime.ParseExact(obj.NgayLapPhieuDNNghiemThuCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDLapHDNTCapVien = currentObj.QDLapHDNTCapVien,
                        NgayQDLapHDNTCapVien = DateTime.ParseExact(obj.NgayQDLapHDNTCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BBHopHDNTCapVien = currentObj.BBHopHDNTCapVien,
                        NgayHopHDXDCapVien = DateTime.ParseExact(obj.NgayHopHDXDCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        CongVanDNNTCapTapDoan = currentObj.CongVanDNNTCapTapDoan,
                        NgayLapCongVanDNNTCapTapDoan = DateTime.ParseExact(obj.NgayLapCongVanDNNTCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDLapHDNTCapTapDoan = currentObj.QDLapHDNTCapTapDoan,
                        NgayQDLapHDNTCapTapDoan = DateTime.ParseExact(obj.NgayQDLapHDNTCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BBHopHDNTCapTapDoan = currentObj.BBHopHDNTCapTapDoan,
                        NgayHopHDNTCapTapDoan = DateTime.ParseExact(obj.NgayHopHDNTCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDCongNhanKetQua = currentObj.QDCongNhanKetQua,
                        NgayQDCongNhanKetQua = DateTime.ParseExact(obj.NgayQDCongNhanKetQua.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        CreatedBy = currentObj.CreatedBy,
                        CreatedDate = currentObj.CreatedDate,
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

                                var guidsPhieuDNNghiemThuCapVien = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("PHIEUDNNGHIEMTHUCAPVIEN", StringComparison.OrdinalIgnoreCase));
                                var guidsQDLapHDNTCapVien = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDLAPHDNTCAPVIEN", StringComparison.OrdinalIgnoreCase));
                                var guidsBBHopHDNTCapVien = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("BBHOPHDNTCAPVIEN", StringComparison.OrdinalIgnoreCase));
                                var guidsCongVanDNNTCapTapDoan = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("CONGVANDNNTCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                                var guidsQDLapHDNTCapTapDoan = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDLAPHDNTCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                                var guidsBBHopHDNTCapTapDoan = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("BBHOPHDNTCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                                var guidsQDCongNhanKetQua = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDCONGNHANKETQUA", StringComparison.OrdinalIgnoreCase));


                                if (guidsPhieuDNNghiemThuCapVien != null)
                                {
                                    lstGuidAttachmentDel.Add(model.PhieuDNNghiemThuCapVien);
                                    model.PhieuDNNghiemThuCapVien = guidsPhieuDNNghiemThuCapVien.Guid;
                                }
                                if (guidsQDLapHDNTCapVien != null)
                                {
                                    lstGuidAttachmentDel.Add(model.QDLapHDNTCapVien);
                                    model.QDLapHDNTCapVien = guidsQDLapHDNTCapVien.Guid;
                                }
                                if (guidsBBHopHDNTCapVien != null)
                                {
                                    lstGuidAttachmentDel.Add(model.BBHopHDNTCapVien);
                                    model.BBHopHDNTCapVien = guidsBBHopHDNTCapVien.Guid;
                                }
                                if (guidsCongVanDNNTCapTapDoan != null)
                                {
                                    lstGuidAttachmentDel.Add(model.CongVanDNNTCapTapDoan);
                                    model.CongVanDNNTCapTapDoan = guidsCongVanDNNTCapTapDoan.Guid;
                                }
                                if (guidsQDLapHDNTCapTapDoan != null)
                                {
                                    lstGuidAttachmentDel.Add(model.QDLapHDNTCapTapDoan);
                                    model.QDLapHDNTCapTapDoan = guidsQDLapHDNTCapTapDoan.Guid;
                                }
                                if (guidsBBHopHDNTCapTapDoan != null)
                                {
                                    lstGuidAttachmentDel.Add(model.BBHopHDNTCapTapDoan);
                                    model.BBHopHDNTCapTapDoan = guidsBBHopHDNTCapTapDoan.Guid;
                                }
                                if (guidsQDCongNhanKetQua != null)
                                {
                                    lstGuidAttachmentDel.Add(model.QDCongNhanKetQua);
                                    model.QDCongNhanKetQua = guidsQDCongNhanKetQua.Guid;
                                }
                            }
                            else
                                return BadRequest($"Error: Upload file thất bại!");
                        }
                    }

                    _unitOfWork.Context.Set<KHCN_HoSoNghiemThu>().Update(model);
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
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok(obj);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Put]. ModelState invalid. Detail: {errors}");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult<KHCN_HoSoNghiemThu> Post([FromForm]KHCN_HoSoNghiemThu_DTO obj, List<IFormFile> lstFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();

                    var model = new KHCN_HoSoNghiemThu()
                    {
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        PhieuDNNghiemThuCapVien = obj.PhieuDNNghiemThuCapVien,
                        NgayLapPhieuDNNghiemThuCapVien = DateTime.ParseExact(obj.NgayLapPhieuDNNghiemThuCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDLapHDNTCapVien = obj.QDLapHDNTCapVien,
                        NgayQDLapHDNTCapVien = DateTime.ParseExact(obj.NgayQDLapHDNTCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BBHopHDNTCapVien = obj.BBHopHDNTCapVien,
                        NgayHopHDXDCapVien = DateTime.ParseExact(obj.NgayHopHDXDCapVien.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        CongVanDNNTCapTapDoan = obj.CongVanDNNTCapTapDoan,
                        NgayLapCongVanDNNTCapTapDoan = DateTime.ParseExact(obj.NgayLapCongVanDNNTCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDLapHDNTCapTapDoan = obj.QDLapHDNTCapTapDoan,
                        NgayQDLapHDNTCapTapDoan = DateTime.ParseExact(obj.NgayQDLapHDNTCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        BBHopHDNTCapTapDoan = obj.BBHopHDNTCapTapDoan,
                        NgayHopHDNTCapTapDoan = DateTime.ParseExact(obj.NgayHopHDNTCapTapDoan.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDCongNhanKetQua = obj.QDCongNhanKetQua,
                        NgayQDCongNhanKetQua = DateTime.ParseExact(obj.NgayQDCongNhanKetQua.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    };

                    model.CreatedBy = model.UpdatedBy = _userProvider.GetUserName();
                    model.CreatedDate = model.UpdatedDate = DateTime.Now;
                    var lstInfoAttachment = new List<KHCN_TaiLieuDinhKem>();
                    lstFile = new List<IFormFile>();

                    if (Request.Form.Files.Count == 0)
                    {
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. ModelState invalid. Detail: File upload is null");
                        return ValidationProblem("File upload is null");
                    }
                    else
                    {
                        lstFile = Request.Form.Files.ToList();
                        if (!lstFile.Select(m => m.Name.ToUpper()).Contains("PHIEUDNNGHIEMTHUCAPVIEN") || !lstFile.Select(m => m.Name.ToUpper()).Contains("QDLAPHDNTCAPVIEN") ||
                            !lstFile.Select(m => m.Name.ToUpper()).Contains("BBHOPHDNTCAPVIEN") || !lstFile.Select(m => m.Name.ToUpper()).Contains("CONGVANDNNTCAPTAPDOAN") ||
                            !lstFile.Select(m => m.Name.ToUpper()).Contains("QDLAPHDNTCAPTAPDOAN") || !lstFile.Select(m => m.Name.ToUpper()).Contains("BBHOPHDNTCAPTAPDOAN") ||
                            !lstFile.Select(m => m.Name.ToUpper()).Contains("QDCONGNHANKETQUA"))
                        {
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("PHIEUDNNGHIEMTHUCAPVIEN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. ModelState invalid. Detail: File PHIEUDNNGHIEMTHUCAPVIEN is null");
                                return ValidationProblem("File PHIEUDNNGHIEMTHUCAPVIEN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("QDLAPHDNTCAPVIEN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. ModelState invalid. Detail: File QDLAPHDNTCAPVIEN is null");
                                return ValidationProblem("File QDLAPHDNTCAPVIEN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("BBHOPHDNTCAPVIEN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. ModelState invalid. Detail: File BBHOPHDNTCAPVIEN is null");
                                return ValidationProblem("File BBHOPHDNTCAPVIEN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("CONGVANDNNTCAPTAPDOAN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. ModelState invalid. Detail: File CONGVANDNNTCAPTAPDOAN is null");
                                return ValidationProblem("File CONGVANDNNTCAPTAPDOAN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("QDLAPHDNTCAPTAPDOAN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. ModelState invalid. Detail: File QDLAPHDNTCAPTAPDOAN is null");
                                return ValidationProblem("File QDLAPHDNTCAPTAPDOAN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("BBHOPHDNTCAPTAPDOAN"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. ModelState invalid. Detail: File BBHOPHDNTCAPTAPDOAN is null");
                                return ValidationProblem("File BBHOPHDNTCAPTAPDOAN is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("QDCONGNHANKETQUA"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. ModelState invalid. Detail: File QDCONGNHANKETQUA is null");
                                return ValidationProblem("File QDCONGNHANKETQUA is null");
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

                        var guidsPhieuDNNghiemThuCapVien = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("PHIEUDNNGHIEMTHUCAPVIEN", StringComparison.OrdinalIgnoreCase));
                        var guidsQDLapHDNTCapVien = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDLAPHDNTCAPVIEN", StringComparison.OrdinalIgnoreCase));
                        var guidsBBHopHDNTCapVien = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("BBHOPHDNTCAPVIEN", StringComparison.OrdinalIgnoreCase));
                        var guidsCongVanDNNTCapTapDoan = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("CONGVANDNNTCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                        var guidsQDLapHDNTCapTapDoan = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDLAPHDNTCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                        var guidsBBHopHDNTCapTapDoan = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("BBHOPHDNTCAPTAPDOAN", StringComparison.OrdinalIgnoreCase));
                        var guidsQDCongNhanKetQua = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDCONGNHANKETQUA", StringComparison.OrdinalIgnoreCase));

                        model.PhieuDNNghiemThuCapVien = guidsPhieuDNNghiemThuCapVien != null ? guidsPhieuDNNghiemThuCapVien.Guid : string.Empty;
                        model.QDLapHDNTCapVien = guidsQDLapHDNTCapVien != null ? guidsQDLapHDNTCapVien.Guid : string.Empty;
                        model.BBHopHDNTCapVien = guidsBBHopHDNTCapVien != null ? guidsBBHopHDNTCapVien.Guid : string.Empty;
                        model.CongVanDNNTCapTapDoan = guidsCongVanDNNTCapTapDoan != null ? guidsCongVanDNNTCapTapDoan.Guid : string.Empty;
                        model.QDLapHDNTCapTapDoan = guidsQDLapHDNTCapTapDoan != null ? guidsQDLapHDNTCapTapDoan.Guid : string.Empty;
                        model.BBHopHDNTCapTapDoan = guidsBBHopHDNTCapTapDoan != null ? guidsBBHopHDNTCapTapDoan.Guid : string.Empty;
                        model.QDCongNhanKetQua = guidsQDCongNhanKetQua != null ? guidsQDCongNhanKetQua.Guid : string.Empty;
                    }
                    else
                        return BadRequest($"Error: Upload file thất bại!");

                    _unitOfWork.Context.Set<KHCN_HoSoNghiemThu>().Add(model);
                    _unitOfWork.Context.Set<KHCN_TaiLieuDinhKem>().AddRange(lstInfoAttachment);
                    _unitOfWork.Commit();

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. Post by masonv = {obj.MaSoNhiemVu} success. Time process: {sw.Elapsed.Seconds} seconds.");

                    return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Post]. Post by masonv = {obj.MaSoNhiemVu} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KHCN_HoSoNghiemThu>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _hosonghiemthuRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                await _hosonghiemthuRepository.DeleteAsync(obj);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok();
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoNghiemThu.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _hosonghiemthuRepository.Get(id.Value) == null;
        }
    }
}