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
    public class HoSoQuyetToanController : BaseController<HoSoQuyetToanController>
    {
        private readonly IHoSoQuyetToanRepository _hosoquyettoanRepository;
        private readonly ITaiLieuDinhKemRepository _taiLieuDinhKemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IPathProvider _pathProvider;
        private readonly IUserProvider _userProvider;

        public HoSoQuyetToanController(IHoSoQuyetToanRepository hosoquyettoanRepository, ITaiLieuDinhKemRepository taiLieuDinhKemRepository, IUnitOfWork unitOfWork, FileService fileService, IConfiguration configuration, IPathProvider pathProvider, IUserProvider userProvider)
        {
            _hosoquyettoanRepository = hosoquyettoanRepository;
            _taiLieuDinhKemRepository = taiLieuDinhKemRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _configuration = configuration;
            _pathProvider = pathProvider;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KHCN_HoSoQuyetToan>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _hosoquyettoanRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<KHCN_HoSoQuyetToan>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(KHCN_HoSoQuyetToan.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_hosoquyettoanRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KHCN_HoSoQuyetToan>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _hosoquyettoanRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromForm]KHCN_HoSoQuyetToan_DTO obj, List<IFormFile> lstFile)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                var currentObj = _hosoquyettoanRepository.Get(id);
                if (currentObj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                    return BadRequest();
                }

                var model = new KHCN_HoSoQuyetToan()
                {
                    Id = obj.Id.Value,
                    MaSoNhiemVu = obj.MaSoNhiemVu,
                    TenNhiemVu = obj.TenNhiemVu,
                    PhieuNhapKhoSP = currentObj.PhieuNhapKhoSP,
                    NgayLapPhieuNhapKhoSP = DateTime.ParseExact(obj.NgayLapPhieuNhapKhoSP.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    QDPheDuyetHSQT = currentObj.QDPheDuyetHSQT,
                    NgayQDPheDuyetHSQT = DateTime.ParseExact(obj.NgayQDPheDuyetHSQT.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
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

                                var guidsPhieuNhapKho = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("PHIEUNHAPKHOSP", StringComparison.OrdinalIgnoreCase));
                                var guidsQuyetDinh = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDPHEDUYETHSQT", StringComparison.OrdinalIgnoreCase));

                                if (guidsPhieuNhapKho != null)
                                {
                                    lstGuidAttachmentDel.Add(model.PhieuNhapKhoSP);
                                    model.PhieuNhapKhoSP = guidsPhieuNhapKho.Guid;
                                }
                                if (guidsQuyetDinh != null)
                                {
                                    lstGuidAttachmentDel.Add(model.QDPheDuyetHSQT);
                                    model.QDPheDuyetHSQT = guidsQuyetDinh.Guid;
                                }
                            }
                            else
                                return BadRequest($"Error: Upload file thất bại!");
                        }
                    }

                    _unitOfWork.Context.Set<KHCN_HoSoQuyetToan>().Update(model);
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
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Put]. ModelState invalid. Detail: {errors}");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult<KHCN_HoSoQuyetToan> Post([FromForm]KHCN_HoSoQuyetToan_DTO obj, List<IFormFile> lstFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();

                    var model = new KHCN_HoSoQuyetToan()
                    {
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        PhieuNhapKhoSP = obj.PhieuNhapKhoSP,
                        NgayLapPhieuNhapKhoSP = DateTime.ParseExact(obj.NgayLapPhieuNhapKhoSP.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        QDPheDuyetHSQT = obj.QDPheDuyetHSQT,
                        NgayQDPheDuyetHSQT = DateTime.ParseExact(obj.NgayQDPheDuyetHSQT.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
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
                        if (!lstFile.Select(m => m.Name.ToUpper()).Contains("PHIEUNHAPKHOSP") || !lstFile.Select(m => m.Name.ToUpper()).Contains("QDPHEDUYETHSQT"))
                        {
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("PHIEUNHAPKHOSP"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKhac.Post]. ModelState invalid. Detail: File PHIEUNHAPKHOSP is null");
                                return ValidationProblem("File PHIEUNHAPKHOSP is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("QDPHEDUYETHSQT"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKhac.Post]. ModelState invalid. Detail: File QDPHEDUYETHSQT is null");
                                return ValidationProblem("File QDPHEDUYETHSQT is null");
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

                        var guidsPhieuNhapKho = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("PHIEUNHAPKHOSP", StringComparison.OrdinalIgnoreCase));
                        var guidsQuyetDinh = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDPHEDUYETHSQT", StringComparison.OrdinalIgnoreCase));

                        model.PhieuNhapKhoSP = guidsPhieuNhapKho != null ? guidsPhieuNhapKho.Guid : string.Empty;
                        model.QDPheDuyetHSQT = guidsQuyetDinh != null ? guidsQuyetDinh.Guid : string.Empty;
                    }
                    else
                        return BadRequest($"Error: Upload file thất bại!");

                    _unitOfWork.Context.Set<KHCN_HoSoQuyetToan>().Add(model);
                    _unitOfWork.Context.Set<KHCN_TaiLieuDinhKem>().AddRange(lstInfoAttachment);
                    _unitOfWork.Commit();

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Post]. Post by masonv = {obj.MaSoNhiemVu} success. Time process: {sw.Elapsed.Seconds} seconds.");

                    return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Post]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Post]. Post by masonv = {obj.MaSoNhiemVu} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KHCN_HoSoQuyetToan>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _hosoquyettoanRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                await _hosoquyettoanRepository.DeleteAsync(obj);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok();
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoQuyetToan.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _hosoquyettoanRepository.Get(id.Value) == null;
        }
    }
}