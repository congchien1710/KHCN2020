using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;

namespace KHCN.Data.Entities.KHCN
{
    public class KHCN_TaiLieuDinhKemDel : BaseEntity
    {
        public string Guid { get; set; }
        public int LoaiTaiLieu { get; set; }
        public string MaSoNhiemVu { get; set; }
        public string MaSanPham { get; set; }
        public string Keyword { get; set; }
        public string Description { get; set; }
        public string RelativePath { get; set; }
        public string FileName { get; set; }
        public string DisplayFileName { get; set; }
        public string FileSize { get; set; }
    }
}