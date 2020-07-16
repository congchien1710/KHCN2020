using KHCN.Data.Entities.System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.Entities.KHCN
{
    public class KHCN_NhiemVu : BaseEntity
    {
        [Display(Name = "Mã số nhiệm vụ")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Loại nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdLoaiNhiemVu { get; set; }

        [Display(Name = "Loại nhiệm vụ")]
        public string LoaiNhiemVu { get; set; }

        [Display(Name = "Ngành")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdNganh { get; set; }

        [Display(Name = "Ngành")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNganh { get; set; }

        [Display(Name = "Cấp quản lý")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdCapQuanLy { get; set; }

        [Display(Name = "Cấp quản lý")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string CapQuanLy { get; set; }

        [Display(Name = "Đơn vị chủ trì")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdDonViChuTri { get; set; }

        [Display(Name = "Đơn vị chủ trì")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string DonViChuTri { get; set; }

        [Display(Name = "Năm phê duyệt")]
        public int NamPheDuyet { get; set; }

        [Display(Name = "Năm hoàn thành")]
        public int NamHoanThanh { get; set; }

        [Display(Name = "Chủ nhiệm nhiệm vụ")]
        public int IdChuNhiemNV { get; set; }

        [Display(Name = "Chủ nhiệm nhiệm vụ")]
        public string ChuNhiemNV { get; set; }

        [Display(Name = "Email chủ nhiệm")]
        public string EmailChuNhiem { get; set; }

        [Display(Name = "Tiến độ thực hiện")]
        public int IdTienDoThucHien { get; set; }

        [Display(Name = "Tiến độ thực hiện")]
        public string TienDoThucHien { get; set; }
    }
}