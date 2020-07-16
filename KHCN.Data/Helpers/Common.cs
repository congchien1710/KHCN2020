using ServiceStack.DataAnnotations;

namespace KHCN.Data.Helpers
{
    public static class AlertType
    {
        public static string Success = "success";
        public static string Warning = "warning";
        public static string Error = "error";
    }

    public static class LoaiTaiLieu
    {
        public static string NHIEMVU = "NHIEMVU";
        public static string SANPHAM = "SANPHAM";
    }

    public enum LoaiTaiLieuEnum
    {
        [Description("Nhiệm vụ")]
        NHIEMVU = 1,
        [Description("Sản phẩm")]
        SANPHAM = 2,
        [Description("Hồ sơ điều chỉnh")]
        HOSODIEUCHINH = 3,
        [Description("Hồ sơ xét duyệt")]
        HOSOXETDUYET = 4,
        [Description("Hồ sơ quyết toán")]
        HOSOQUYETTOAN = 5,
        [Description("Hồ sơ nghiệm thu")]
        HOSONGHIEMTHU = 6,
    }

    public enum LoaiDieuChinhEnum
    {
        [Description("Điều chỉnh kinh phí")]
        DIEUCHINHKINHPHI = 1,
        [Description("Điều chỉnh thời gian")]
        DIEUCHINHTHOIGIAN = 2,
        [Description("Điều chỉnh khác")]
        DIEUCHINHKHAC = 3,
    }

    public enum GioiTinhEnum
    {
        [Description("Khác")]
        KHAC = 0,
        [Description("Nam")]
        NAM = 1,
        [Description("Nữ")]
        NU = 2
    }
}
