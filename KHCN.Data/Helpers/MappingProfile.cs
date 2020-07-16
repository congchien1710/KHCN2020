using AutoMapper;
using KHCN.Data.DTOs.KHCN;
using KHCN.Data.DTOs.System;
using KHCN.Data.Entities.KHCN;
using KHCN.Data.Entities.System;
using System;

namespace KHCN.Data.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CMS_RoleApi, CMS_RoleApi_DTO>();
            CreateMap<CMS_Api, CMS_Api_DTO>();
            CreateMap<KHCN_TrinhDo, KHCN_TrinhDo_DTO>();
            CreateMap<KHCN_TienDoThucHien, KHCN_TienDoThucHien_DTO>();
            CreateMap<KHCN_TaiLieuDinhKemDel, KHCN_TaiLieuDinhKemDel_DTO>();
            CreateMap<KHCN_TaiLieuDinhKem, KHCN_TaiLieuDinhKem_DTO>();
            CreateMap<KHCN_SanPham, KHCN_SanPham_DTO>();
            CreateMap<KHCN_PhongBan, KHCN_PhongBan_DTO>();
            CreateMap<KHCN_NhiemVu, KHCN_NhiemVu_DTO>();
            CreateMap<KHCN_Nganh, KHCN_Nganh_DTO>();
            CreateMap<KHCN_LoaiNhiemVu, KHCN_LoaiNhiemVu_DTO>();
            CreateMap<KHCN_KinhPhiThucHien, KHCN_KinhPhiThucHien_DTO>();
            CreateMap<KHCN_GiaiDoan, KHCN_GiaiDoan_DTO>();
            CreateMap<KHCN_DonViCheTao, KHCN_DonViCheTao_DTO>();
            CreateMap<KHCN_ChucDanh, KHCN_ChucDanh_DTO>();
            CreateMap<KHCN_ChuanApDung, KHCN_ChuanApDung_DTO>();
            CreateMap<KHCN_CapQuanLy, KHCN_CapQuanLy_DTO>();
            CreateMap<KHCN_CapBac, KHCN_CapBac_DTO>();
            CreateMap<CMS_UserRole, CMS_UserRole_DTO>();
            CreateMap<CMS_User, CMS_User_DTO>();
            CreateMap<CMS_RolePage, CMS_RolePage_DTO>();
            CreateMap<CMS_RoleFunction, CMS_RoleFunction_DTO>();
            CreateMap<CMS_Role, CMS_Role_DTO>();
            CreateMap<CMS_Page, CMS_Page_DTO>();
            CreateMap<CMS_Module, CMS_Module_DTO>();
            CreateMap<CMS_Function, CMS_Function_DTO>();

            CreateMap<KHCN_GiaiDoanSanPham, KHCN_GiaiDoanSanPham_DTO>()
                .ForMember(x => x.ThoiGianBatDau, opt => opt.MapFrom(src => src.ThoiGianBatDau == null ? string.Empty : ((DateTime)src.ThoiGianBatDau).ToString("dd/MM/yyyy")))
                .ForMember(x => x.ThoiGianKetThuc, opt => opt.MapFrom(src => src.ThoiGianKetThuc == null ? string.Empty : ((DateTime)src.ThoiGianKetThuc).ToString("dd/MM/yyyy")))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? string.Empty : ((DateTime)src.CreatedDate).ToString("dd/MM/yyyy")))
                .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToString("dd/MM/yyyy")));

            CreateMap<KHCN_HoSoDieuChinhKhac, KHCN_HoSoDieuChinhKhac_DTO>()
                .ForMember(x => x.NgayLapToTrinhXinPDDieuChinh, opt => opt.MapFrom(src => src.NgayLapToTrinhXinPDDieuChinh == null ? string.Empty : ((DateTime)src.NgayLapToTrinhXinPDDieuChinh).ToString("dd/MM/yyyy")))
                .ForMember(x => x.NgayQDPheDuyetDieuChinh, opt => opt.MapFrom(src => src.NgayQDPheDuyetDieuChinh == null ? string.Empty : ((DateTime)src.NgayQDPheDuyetDieuChinh).ToString("dd/MM/yyyy")))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? string.Empty : ((DateTime)src.CreatedDate).ToString("dd/MM/yyyy")))
                .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToString("dd/MM/yyyy")));

            CreateMap<KHCN_HoSoDieuChinhKinhPhi, KHCN_HoSoDieuChinhKinhPhi_DTO>()
                .ForMember(x => x.NgayLapToTrinhXinPDDieuChinh, opt => opt.MapFrom(src => src.NgayLapToTrinhXinPDDieuChinh == null ? string.Empty : ((DateTime)src.NgayLapToTrinhXinPDDieuChinh).ToString("dd/MM/yyyy")))
                .ForMember(x => x.NgayQDPheDuyetDieuChinh, opt => opt.MapFrom(src => src.NgayQDPheDuyetDieuChinh == null ? string.Empty : ((DateTime)src.NgayQDPheDuyetDieuChinh).ToString("dd/MM/yyyy")))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? string.Empty : ((DateTime)src.CreatedDate).ToString("dd/MM/yyyy")))
                .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToString("dd/MM/yyyy")));

            CreateMap<KHCN_HoSoDieuChinhThoiGian, KHCN_HoSoDieuChinhThoiGian_DTO>()
                .ForMember(x => x.NgayLapToTrinhXinPDDieuChinh, opt => opt.MapFrom(src => src.NgayLapToTrinhXinPDDieuChinh == null ? string.Empty : ((DateTime)src.NgayLapToTrinhXinPDDieuChinh).ToString("dd/MM/yyyy")))
                .ForMember(x => x.NgayQDPheDuyetDieuChinh, opt => opt.MapFrom(src => src.NgayQDPheDuyetDieuChinh == null ? string.Empty : ((DateTime)src.NgayQDPheDuyetDieuChinh).ToString("dd/MM/yyyy")))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? string.Empty : ((DateTime)src.CreatedDate).ToString("dd/MM/yyyy")))
                .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToString("dd/MM/yyyy")));

            CreateMap<KHCN_HoSoNghiemThu, KHCN_HoSoNghiemThu_DTO>()
                .ForMember(x => x.NgayHopHDNTCapTapDoan, opt => opt.MapFrom(src => src.NgayHopHDNTCapTapDoan == null ? string.Empty : ((DateTime)src.NgayHopHDNTCapTapDoan).ToString("dd/MM/yyyy")))
                .ForMember(x => x.NgayHopHDXDCapVien, opt => opt.MapFrom(src => src.NgayHopHDXDCapVien == null ? string.Empty : ((DateTime)src.NgayHopHDXDCapVien).ToString("dd/MM/yyyy")))
                .ForMember(x => x.NgayLapCongVanDNNTCapTapDoan, opt => opt.MapFrom(src => src.NgayLapCongVanDNNTCapTapDoan == null ? string.Empty : ((DateTime)src.NgayLapCongVanDNNTCapTapDoan).ToString("dd/MM/yyyy")))
                .ForMember(x => x.NgayLapPhieuDNNghiemThuCapVien, opt => opt.MapFrom(src => src.NgayLapPhieuDNNghiemThuCapVien == null ? string.Empty : ((DateTime)src.NgayLapPhieuDNNghiemThuCapVien).ToString("dd/MM/yyyy")))
                .ForMember(x => x.NgayQDCongNhanKetQua, opt => opt.MapFrom(src => src.NgayQDCongNhanKetQua == null ? string.Empty : ((DateTime)src.NgayQDCongNhanKetQua).ToString("dd/MM/yyyy")))
                .ForMember(x => x.NgayQDLapHDNTCapTapDoan, opt => opt.MapFrom(src => src.NgayQDLapHDNTCapTapDoan == null ? string.Empty : ((DateTime)src.NgayQDLapHDNTCapTapDoan).ToString("dd/MM/yyyy")))
                .ForMember(x => x.NgayQDLapHDNTCapVien, opt => opt.MapFrom(src => src.NgayQDLapHDNTCapVien == null ? string.Empty : ((DateTime)src.NgayQDLapHDNTCapVien).ToString("dd/MM/yyyy")))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? string.Empty : ((DateTime)src.CreatedDate).ToString("dd/MM/yyyy")))
                .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToString("dd/MM/yyyy")));

            CreateMap<KHCN_HoSoQuyetToan, KHCN_HoSoQuyetToan_DTO>()
               .ForMember(x => x.NgayLapPhieuNhapKhoSP, opt => opt.MapFrom(src => src.NgayLapPhieuNhapKhoSP == null ? string.Empty : ((DateTime)src.NgayLapPhieuNhapKhoSP).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayQDPheDuyetHSQT, opt => opt.MapFrom(src => src.NgayQDPheDuyetHSQT == null ? string.Empty : ((DateTime)src.NgayQDPheDuyetHSQT).ToString("dd/MM/yyyy")))
               .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? string.Empty : ((DateTime)src.CreatedDate).ToString("dd/MM/yyyy")))
               .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToString("dd/MM/yyyy")));

            CreateMap<KHCN_HoSoXetDuyet, KHCN_HoSoXetDuyet_DTO>()
               .ForMember(x => x.NgayDangKy, opt => opt.MapFrom(src => src.NgayDangKy == null ? string.Empty : ((DateTime)src.NgayDangKy).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayHopHDXDCapTapDoan, opt => opt.MapFrom(src => src.NgayHopHDXDCapTapDoan == null ? string.Empty : ((DateTime)src.NgayHopHDXDCapTapDoan).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayHopHDXDCapVien, opt => opt.MapFrom(src => src.NgayHopHDXDCapVien == null ? string.Empty : ((DateTime)src.NgayHopHDXDCapVien).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayLapHoSoDuToan, opt => opt.MapFrom(src => src.NgayLapHoSoDuToan == null ? string.Empty : ((DateTime)src.NgayLapHoSoDuToan).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayLapTaiLieuMRD, opt => opt.MapFrom(src => src.NgayLapTaiLieuMRD == null ? string.Empty : ((DateTime)src.NgayLapTaiLieuMRD).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayLapTaiLieuPRD, opt => opt.MapFrom(src => src.NgayLapTaiLieuPRD == null ? string.Empty : ((DateTime)src.NgayLapTaiLieuPRD).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayQDGiaoNhiemVu, opt => opt.MapFrom(src => src.NgayQDGiaoNhiemVu == null ? string.Empty : ((DateTime)src.NgayQDGiaoNhiemVu).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayQDLapHDXDCapTapDoan, opt => opt.MapFrom(src => src.NgayQDLapHDXDCapTapDoan == null ? string.Empty : ((DateTime)src.NgayQDLapHDXDCapTapDoan).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayQDLapHDXDCapVien, opt => opt.MapFrom(src => src.NgayQDLapHDXDCapVien == null ? string.Empty : ((DateTime)src.NgayQDLapHDXDCapVien).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayQDPheDuyet, opt => opt.MapFrom(src => src.NgayQDPheDuyet == null ? string.Empty : ((DateTime)src.NgayQDPheDuyet).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayThuyetMinhNhiemVu, opt => opt.MapFrom(src => src.NgayThuyetMinhNhiemVu == null ? string.Empty : ((DateTime)src.NgayThuyetMinhNhiemVu).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayTrinhXinPDChuTruong, opt => opt.MapFrom(src => src.NgayTrinhXinPDChuTruong == null ? string.Empty : ((DateTime)src.NgayTrinhXinPDChuTruong).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgayTrinhXinPDNhiemVu, opt => opt.MapFrom(src => src.NgayTrinhXinPDNhiemVu == null ? string.Empty : ((DateTime)src.NgayTrinhXinPDNhiemVu).ToString("dd/MM/yyyy")))
               .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? string.Empty : ((DateTime)src.CreatedDate).ToString("dd/MM/yyyy")))
               .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToString("dd/MM/yyyy")));

            CreateMap<KHCN_ThanhVienDeTai, KHCN_ThanhVienDeTai_DTO>()
               .ForMember(x => x.NgayKyHD, opt => opt.MapFrom(src => src.NgayKyHD == null ? string.Empty : ((DateTime)src.NgayKyHD).ToString("dd/MM/yyyy")))
               .ForMember(x => x.NgaySinh, opt => opt.MapFrom(src => src.NgaySinh == null ? string.Empty : ((DateTime)src.NgaySinh).ToString("dd/MM/yyyy")))
               .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? string.Empty : ((DateTime)src.CreatedDate).ToString("dd/MM/yyyy")))
               .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToString("dd/MM/yyyy")));

            CreateMap<KHCN_ThoiGianThucHien, KHCN_ThoiGianThucHien_DTO>()
               .ForMember(x => x.TGBatDau, opt => opt.MapFrom(src => src.TGBatDau == null ? string.Empty : ((DateTime)src.TGBatDau).ToString("dd/MM/yyyy")))
               .ForMember(x => x.TGKetThuc, opt => opt.MapFrom(src => src.TGKetThuc == null ? string.Empty : ((DateTime)src.TGKetThuc).ToString("dd/MM/yyyy")))
               .ForMember(x => x.TGGiaHanMoiNhat, opt => opt.MapFrom(src => src.TGKetThuc == null ? string.Empty : ((DateTime)src.TGGiaHanMoiNhat).ToString("dd/MM/yyyy")))
               .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? string.Empty : ((DateTime)src.CreatedDate).ToString("dd/MM/yyyy")))
               .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate == null ? string.Empty : ((DateTime)src.UpdatedDate).ToString("dd/MM/yyyy")));
        }
    }
}