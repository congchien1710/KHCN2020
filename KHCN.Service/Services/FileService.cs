using KHCN.Data.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace KHCN.Service.Services
{
    public class FileService
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public KeyValuePair<bool, List<AttachmentUpload>> SaveFileNhiemVu(string rootPath, string relativeRoot, LoaiTaiLieuEnum loaitailieu, string manhiemvu, string masanpham, List<IFormFile> files)
        {
            var res = true;
            var lstAttachment = new List<AttachmentUpload>();
            try
            {
                var relativePath = Path.Combine(relativeRoot, manhiemvu);
                var fullPath = Path.Combine(rootPath, manhiemvu);

                switch (loaitailieu)
                {
                    case LoaiTaiLieuEnum.NHIEMVU:
                        relativePath = Path.Combine(relativeRoot, manhiemvu, "ThongTinNhiemVu");
                        fullPath = Path.Combine(rootPath, manhiemvu, "ThongTinNhiemVu");
                        break;
                    case LoaiTaiLieuEnum.SANPHAM:
                        relativePath = Path.Combine(relativeRoot, manhiemvu, "SanPham", masanpham);
                        fullPath = Path.Combine(rootPath, manhiemvu, "SanPham", masanpham);
                        break;
                    case LoaiTaiLieuEnum.HOSODIEUCHINH:
                        relativePath = Path.Combine(relativeRoot, manhiemvu, "HoSoDieuChinh", DateTime.Now.ToString("yyyy-MM-dd"));
                        fullPath = Path.Combine(rootPath, manhiemvu, "HoSoDieuChinh", DateTime.Now.ToString("yyyy-MM-dd"));
                        break;
                    default:
                        break;
                }

                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);

                for (int k = 0; k < files.Count; k++)
                {
                    var file = files[k];
                    var fName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fExt = Path.GetExtension(file.FileName);
                    if (file.Length <= 0)
                        return new KeyValuePair<bool, List<AttachmentUpload>>(false, new List<AttachmentUpload>());

                    int i = 1;
                    var fileName = $"{fName}.{fExt}";
                    while (true)
                    {
                        if (File.Exists(Path.Combine(fullPath, fileName)))
                        {
                            fileName = $"{fName}({i.ToString()}).{fExt}";
                            i++;
                        }
                        else
                            break;
                    }

                    var filePath = Path.Combine(fullPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        lstAttachment.Add(new AttachmentUpload { 
                            ApsolutePath = filePath,
                            RelativePath = relativePath,
                            Keyword = file.Name,
                            FileName = fileName,
                            DisplayName = file.FileName,
                            FileSize = SizeConverter(file.Length)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                res = false;
            }
            finally
            {
                if (!res)
                    DeleteFile(lstAttachment.Select(m => m.ApsolutePath).ToList());
            }

            return new KeyValuePair<bool, List<AttachmentUpload>>(res, lstAttachment);
        }

        public KeyValuePair<bool, List<AttachmentUpload>> SaveFile(string rootPath, string relativeRoot, string manhiemvu, List<IFormFile> files)
        {
            var res = true;
            var lstAttachment = new List<AttachmentUpload>();
            try
            {
                var relativePath = Path.Combine(relativeRoot, manhiemvu, "HoSoDieuChinh", DateTime.Now.ToString("yyyy-MM-dd"));
                var fullPath = Path.Combine(rootPath, manhiemvu, "HoSoDieuChinh", DateTime.Now.ToString("yyyy-MM-dd"));

                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);

                for (int k = 0; k < files.Count; k++)
                {
                    var file = files[k];
                    var fName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fExt = Path.GetExtension(file.FileName);
                    if (file.Length <= 0)
                        return new KeyValuePair<bool, List<AttachmentUpload>>(false, new List<AttachmentUpload>());

                    int i = 1;
                    var fileName = $"{fName}.{fExt}";
                    while (true)
                    {
                        if (File.Exists(Path.Combine(fullPath, fileName)))
                        {
                            fileName = $"{fName}({i.ToString()}).{fExt}";
                            i++;
                        }
                        else
                            break;
                    }

                    var filePath = Path.Combine(fullPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        lstAttachment.Add(new AttachmentUpload
                        {
                            ApsolutePath = filePath,
                            RelativePath = relativePath,
                            Keyword = file.Name,
                            FileName = fileName,
                            DisplayName = file.FileName,
                            FileSize = SizeConverter(file.Length)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                res = false;
            }
            finally
            {
                if (!res)
                    DeleteFile(lstAttachment.Select(m => m.ApsolutePath).ToList());
            }

            return new KeyValuePair<bool, List<AttachmentUpload>>(res, lstAttachment);
        }

        public void DeleteFile(List<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        public void MoveFile(List<string> filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    var fileName = Path.GetFileName(filePath);
                    var folderPath = Path.GetFullPath(filePath);
                    var folderDel = folderPath.Replace("/DATA/RealData/NhiemVu/", "/DATA/DelData/NhiemVu/");
                    if (!Directory.Exists(folderDel))
                        Directory.CreateDirectory(folderDel);

                    File.Move(filePath, Path.Combine(folderDel, fileName), true);
                }
            }
        }

        public (string fileType, byte[] archiveData, string archiveName) FetechFiles(string subDirectory)
        {
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            var files = Directory.GetFiles(Path.Combine("D:\\webroot\\", subDirectory)).ToList();

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    files.ForEach(file =>
                    {
                        var theFile = archive.CreateEntry(file);
                        using (var streamWriter = new StreamWriter(theFile.Open()))
                        {
                            streamWriter.Write(File.ReadAllText(file));
                        }
                    });
                }

                return ("application/zip", memoryStream.ToArray(), zipName);
            }
        }

        public static string SizeConverter(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            switch (fileSize)
            {
                case var _ when fileSize < kilobyte:
                    return $"Less then 1KB";

                case var _ when fileSize < megabyte:
                    return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";

                case var _ when fileSize < gigabyte:
                    return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";

                case var _ when fileSize >= gigabyte:
                    return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";

                default:
                    return "n/a";
            }
        }
    }
}