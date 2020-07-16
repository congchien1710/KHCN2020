using KHCN.Api.Provider;
using KHCN.Data.Entities.KHCN;
using KHCN.Data.Helpers;
using KHCN.Data.Interfaces;
using KHCN.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Api.Controllers
{
    [ApiController]
    //[ApiAuthorize]
    [Route("api/[controller]/[action]")]
    public class FileController : BaseController<FileController>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IPathProvider _pathProvider;
        private readonly IUserProvider _userProvider;

        public FileController(IUnitOfWork unitOfWork, FileService fileService, IConfiguration configuration, IPathProvider pathProvider, IUserProvider userProvider)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _configuration = configuration;
            _pathProvider = pathProvider;
            _userProvider = userProvider;
        }

        // Download file(s) to client according path: rootDirectory/subDirectory with single zip file
        [HttpGet]
        public IActionResult DownloadFiles(string subDirectory)
        {
            try
            {
                var (fileType, archiveData, archiveName) = _fileService.FetechFiles(subDirectory);

                return File(archiveData, fileType, archiveName);
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult UploadFile([FromForm]int loai, [FromForm]string manhiemvu, [FromForm]string masanpham)
        {
            try
            {
                var relativeRoot = _configuration["Jwt:RootPathUpload"];
                var rootPath = _pathProvider.MapPath(relativeRoot);
                var loaitailieu = (LoaiTaiLieuEnum)loai;
                var files = Request.Form.Files.ToList();

                if (files.Count > 0)
                {
                    var res = _fileService.SaveFileNhiemVu(rootPath, relativeRoot, loaitailieu, manhiemvu, masanpham, files);
                    if (res.Key)
                    {
                        var now = DateTime.Now;
                        var currentUser = _userProvider.GetUserName();
                        var lstInfoAttachment = res.Value.Select(m => new KHCN_TaiLieuDinhKem
                        {
                            LoaiTaiLieu = (byte)loaitailieu,
                            MaSoNhiemVu = manhiemvu,
                            MaSanPham = loaitailieu == LoaiTaiLieuEnum.SANPHAM ? masanpham : "",
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

                        _unitOfWork.Context.Set<KHCN_TaiLieuDinhKem>().AddRange(lstInfoAttachment);
                        _unitOfWork.Commit();

                        return Ok(new { files.Count, Size = FileService.SizeConverter(files.Sum(f => f.Length)) });
                    }
                    else
                        return BadRequest($"Error: Upload file thất bại!");
                }
                else
                    return BadRequest($"Error: File upload trống!");
            }
            catch (Exception exception)
            {
                return BadRequest($"Error: {exception.Message}");
            }
        }
    }
}