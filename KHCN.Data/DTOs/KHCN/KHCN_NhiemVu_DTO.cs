using KHCN.Data.DTOs.System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_NhiemVu_DTO : BaseEntity_DTO
    {
        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Loại nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdLoaiNhiemVu { get; set; }

        [Display(Name = "Loại nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
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
        [Required(ErrorMessage = "Bắt buộc")]
        public int NamPheDuyet { get; set; }

        [Display(Name = "Năm hoàn thành")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int NamHoanThanh { get; set; }

        [Display(Name = "Chủ nhiệm nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdChuNhiemNV { get; set; }

        [Display(Name = "Chủ nhiệm nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string ChuNhiemNV { get; set; }

        [Display(Name = "Email chủ nhiệm")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string EmailChuNhiem { get; set; }

        [Display(Name = "Tiến độ thực hiện")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdTienDoThucHien { get; set; }

        [Display(Name = "Tiến độ thực hiện")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TienDoThucHien { get; set; }
    }
}