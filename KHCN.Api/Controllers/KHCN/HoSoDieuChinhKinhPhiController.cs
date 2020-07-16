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
    public class HoSoDieuChinhKinhPhiController : BaseController<HoSoDieuChinhKinhPhiController>
    {
        private readonly IHoSoDieuChinhKinhPhiRepository _hosodieuchinhkinhphiRepository;
        private readonly ITaiLieuDinhKemRepository _taiLieuDinhKemRepository;
        private readonly IUserProvider _userProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IPathProvider _pathProvider;

        public HoSoDieuChinhKinhPhiController(IHoSoDieuChinhKinhPhiRepository hosodieuchinhkinhphiRepository, ITaiLieuDinhKemRepository taiLieuDinhKemRepository, IUnitOfWork unitOfWork, FileService fileService, IConfiguration configuration, IPathProvider pathProvider, IUserProvider userProvider)
        {
            _hosodieuchinhkinhphiRepository = hosodieuchinhkinhphiRepository;
            _taiLieuDinhKemRepository = taiLieuDinhKemRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _configuration = configuration;
            _pathProvider = pathProvider;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KHCN_HoSoDieuChinhKinhPhi>>> GetAll()
        {
            try
            {
                sw.Restart();
                var res = await _hosodieuchinhkinhphiRepository.GetAllAsyn();
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.GetAll]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res.ToList());
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.GetAll]. Get data error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResults<KHCN_HoSoDieuChinhKinhPhi>>> GetAllPaging(int? page, int? pageSize, string orderBy = nameof(KHCN_HoSoDieuChinhKinhPhi.Id), bool ascending = true)
        {
            try
            {
                sw.Restart();
                var res = await CreatePagedResults(_hosodieuchinhkinhphiRepository.GetAll(), page.Value, pageSize.Value, orderBy, ascending);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.GetAllPaging]. Get data success. Time process: {sw.Elapsed.Seconds} seconds.");

                return Ok(res);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.GetAllPaging].  Get data page {page} pagesize {pageSize} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KHCN_HoSoDieuChinhKinhPhi>> GetById(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _hosodieuchinhkinhphiRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.GetById]. Get by id = {id} error. Cannot find obj by id ={id}");
                    return NotFound();
                }

                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.GetById]. Get by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok(obj);
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.GetById]. Get by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]KHCN_HoSoDieuChinhKinhPhi_DTO obj)
        {
            if (obj == null)
            {
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Put]. Put by id = {id} error. Detail: Model put is null");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();
                    var currentObj = _hosodieuchinhkinhphiRepository.Get(id);
                    if (currentObj == null)
                    {
                        sw.Stop();
                        log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Put]. Put by id = {id} error. Detail: Cannot find object by id = {{id}}");
                        return BadRequest();
                    }

                    var model = new KHCN_HoSoDieuChinhKinhPhi()
                    {
                        Id = obj.Id.Value,
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        KinhPhiSauKhiDieuChinh = obj.KinhPhiSauKhiDieuChinh,
                        ToTrinhXinPDDieuChinh = currentObj.ToTrinhXinPDDieuChinh,
                        QDPheDuyetDieuChinh = currentObj.QDPheDuyetDieuChinh,
                        NgayLapToTrinhXinPDDieuChinh = DateTime.ParseExact(obj.NgayLapToTrinhXinPDDieuChinh.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        NgayQDPheDuyetDieuChinh = DateTime.ParseExact(obj.NgayQDPheDuyetDieuChinh.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
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

                                var guidsToTrinh = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TOTRINHXINPDDIEUCHINH", StringComparison.OrdinalIgnoreCase));
                                var guidsQuyetDinh = lstInfoAttachmentAdd.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDPHEDUYETDIEUCHINH", StringComparison.OrdinalIgnoreCase));

                                if (guidsToTrinh != null)
                                {
                                    lstGuidAttachmentDel.Add(model.ToTrinhXinPDDieuChinh);
                                    model.ToTrinhXinPDDieuChinh = guidsToTrinh.Guid;
                                }
                                if (guidsQuyetDinh != null)
                                {
                                    lstGuidAttachmentDel.Add(model.QDPheDuyetDieuChinh);
                                    model.QDPheDuyetDieuChinh = guidsQuyetDinh.Guid;
                                }
                            }
                            else
                                return BadRequest($"Error: Upload file thất bại!");
                        }
                    }

                    _unitOfWork.Context.Set<KHCN_HoSoDieuChinhKinhPhi>().Update(model);
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
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Put]. Put by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                    return Ok(obj);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Put]. ModelState invalid. Detail: {errors}");
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
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Put]. Put by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult<KHCN_HoSoDieuChinhKinhPhi> Post([FromForm]KHCN_HoSoDieuChinhKinhPhi_DTO obj, List<IFormFile> lstFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    sw.Restart();

                    var model = new KHCN_HoSoDieuChinhKinhPhi()
                    {
                        MaSoNhiemVu = obj.MaSoNhiemVu,
                        TenNhiemVu = obj.TenNhiemVu,
                        KinhPhiSauKhiDieuChinh = obj.KinhPhiSauKhiDieuChinh,
                        ToTrinhXinPDDieuChinh = obj.ToTrinhXinPDDieuChinh,
                        QDPheDuyetDieuChinh = obj.QDPheDuyetDieuChinh,
                        NgayLapToTrinhXinPDDieuChinh = DateTime.ParseExact(obj.NgayLapToTrinhXinPDDieuChinh.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        NgayQDPheDuyetDieuChinh = DateTime.ParseExact(obj.NgayQDPheDuyetDieuChinh.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
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
                        if (!lstFile.Select(m => m.Name.ToUpper()).Contains("TOTRINHXINPDDIEUCHINH") || !lstFile.Select(m => m.Name.ToUpper()).Contains("QDPHEDUYETDIEUCHINH"))
                        {
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("TOTRINHXINPDDIEUCHINH"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKhac.Post]. ModelState invalid. Detail: File TOTRINHXINPDDIEUCHINH is null");
                                return ValidationProblem("File TOTRINHXINPDDIEUCHINH is null");
                            }
                            if (!lstFile.Select(m => m.Name.ToUpper()).Contains("QDPHEDUYETDIEUCHINH"))
                            {
                                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKhac.Post]. ModelState invalid. Detail: File QDPHEDUYETDIEUCHINH is null");
                                return ValidationProblem("File QDPHEDUYETDIEUCHINH is null");
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

                        var guidsToTrinh = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("TOTRINHXINPDDIEUCHINH", StringComparison.OrdinalIgnoreCase));
                        var guidsQuyetDinh = lstInfoAttachment.FirstOrDefault(m => m.Keyword.ToUpper().Equals("QDPHEDUYETDIEUCHINH", StringComparison.OrdinalIgnoreCase));

                        model.ToTrinhXinPDDieuChinh = guidsToTrinh != null ? guidsToTrinh.Guid : string.Empty;
                        model.QDPheDuyetDieuChinh = guidsQuyetDinh != null ? guidsQuyetDinh.Guid : string.Empty;
                    }
                    else
                        return BadRequest($"Error: Upload file thất bại!");

                    _unitOfWork.Context.Set<KHCN_HoSoDieuChinhKinhPhi>().Add(model);
                    _unitOfWork.Context.Set<KHCN_TaiLieuDinhKem>().AddRange(lstInfoAttachment);
                    _unitOfWork.Commit();

                    sw.Stop();
                    log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Post]. Post by masonv = {obj.MaSoNhiemVu} success. Time process: {sw.Elapsed.Seconds} seconds.");

                    return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
                }
                else
                {
                    var errors = string.Join(",", ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage).ToList());
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Post]. ModelState invalid. Detail: {errors}");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Post]. Post by masonv = {obj.MaSoNhiemVu} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<KHCN_HoSoDieuChinhKinhPhi>> Delete(int id)
        {
            try
            {
                sw.Restart();
                var obj = await _hosodieuchinhkinhphiRepository.GetAsync(id);

                if (obj == null)
                {
                    sw.Stop();
                    log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Delete]. Delete by id = {id} error. Detail: Cannot find obj by id ={id}");
                    return NotFound();
                }

                await _hosodieuchinhkinhphiRepository.DeleteAsync(obj);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Delete]. Delete by id = {id} success. Time process: {sw.Elapsed.Seconds} seconds.");
                return Ok();
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[KHCN_HoSoDieuChinhKinhPhi.Delete]. Delete by id = {id} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [NonAction]
        private bool ObjectExists(int? id)
        {
            return _hosodieuchinhkinhphiRepository.Get(id.Value) == null;
        }
    }
}