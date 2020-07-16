using KHCN.Data.Entities.System;
using KHCN.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace KHCN.Data.Entities.KHCN
{
    public class KHCN_ThanhVienDeTai : BaseEntity
    {
        [Display(Name = "Mã nhân viên")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string MaNV { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string HoTen { get; set; }

        [Display(Name = "Đơn vị")]
        public int IdDonVi { get; set; }

        [Display(Name = "Đơn vị")]
        public string DonVi { get; set; }

        [Display(Name = "Phòng ban")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdPhongBan { get; set; }

        [Display(Name = "Phòng ban")]
        public string PhongBan { get; set; }

        [Display(Name = "Đối tượng / cấp bậc")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdCapBac { get; set; }

        [Display(Name = "Đối tượng / cấp bậc")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string CapBac { get; set; }

        [Display(Name = "Chức danh")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdChucDanh { get; set; }

        [Display(Name = "Chức danh")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string ChucDanh { get; set; }

        [Display(Name = "Trình độ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public int IdTrinhDo { get; set; }

        [Display(Name = "Trình độ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public string TrinhDo { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Bắt buộc")]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Ngày ký HĐLĐ")]
        [Required(ErrorMessage = "Bắt buộc")]
        public DateTime NgayKyHD { get; set; }

        [Display(Name = "Giới tính")]
        public GioiTinhEnum GioiTinh { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Nhiệm vụ 1")]
        public string NhiemVu1 { get; set; }

        [Display(Name = "Trạng thái")]
        public int TrangThaiNV1 { get; set; }

        [Display(Name = "Nhiệm vụ 2")]
        public string NhiemVu2 { get; set; }

        [Display(Name = "Trạng thái")]
        public int TrangThaiNV2 { get; set; }

        [Display(Name = "Nhiệm vụ 3")]
        public string NhiemVu3 { get; set; }

        [Display(Name = "Trạng thái")]
        public int TrangThaiNV3 { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }
    }
}