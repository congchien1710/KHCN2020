using KHCN.Data.DTOs.System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.DTOs.KHCN
{
    public class KHCN_SanPham_DTO : BaseEntity_DTO
    {
        [Display(Name = "Mã sản phẩm")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSanPham { get; set; }

        [Display(Name = "Serial number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenSanPham { get; set; }

        [Display(Name = "Mã số nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaSoNhiemVu { get; set; }

        [Display(Name = "Tên nhiệm vụ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNhiemVu { get; set; }

        [Display(Name = "Ngành")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdNganh { get; set; }

        [Display(Name = "Ngành")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenNganh { get; set; }

        [Display(Name = "Đơn vị chế tạo")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdDonViCheTao { get; set; }

        [Display(Name = "Đơn vị chế tạo")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string DonViCheTao { get; set; }

        [Display(Name = "Giai đoạn")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaGiaiDoan { get; set; }

        [Display(Name = "Giai đoạn")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TenGiaiDoan { get; set; }

        [Display(Name = "Chuẩn áp dụng")]
        public int IdChuanApDung { get; set; }

        [Display(Name = "Chuẩn áp dụng")]
        public string ChuanApDung { get; set; }

        [Display(Name = "Tính năng, CTKT chính")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TinhNangChinh { get; set; }

        [Display(Name = "Sản phẩm tương đương")]
        public string SanPhamTuongDuong { get; set; }
    }
}