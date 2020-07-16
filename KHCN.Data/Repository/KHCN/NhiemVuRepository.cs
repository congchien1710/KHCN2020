using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using KHCN.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface INhiemVuRepository : IGenericRepository<KHCN_NhiemVu>
    {
        KHCN_NhiemVu GetByMaSoNV(string manv);
        KHCN_NhiemVu GetByName(string name);
        List<KHCN_NhiemVu> GetByIdChuNhiemNV(int idChuNhiem);
        List<KHCN_NhiemVu> GetByIdLoaiNhiemVu(int idLoaiNhiemVu);
        List<KHCN_NhiemVu> GetByIdNganh(int idNganh);
        List<KHCN_NhiemVu> GetByIdCapQuanLy(int idCapQuanLy);
        List<KHCN_NhiemVu> GetByIdTienDo(int idTienDo);
        List<KHCN_NhiemVu> GetByIdDonViChuTri(int idDonVi);

        IQueryable<ThongTinNhiemVuVM> GetData(int? loainhiemvu, int? nganh, int? capquanly, int? donvichutri, int? nampheduyettu, int? nampheduyetden,
            int? namhoanthanhtu, int? namhoanthanhden, int? chunhiemnv, int? tiendothuchien, DateTime? tgbatdau, DateTime? tgketthuc, decimal? dutoanduyettu, decimal? dutoanduyetden, decimal? gtquyettoantu, decimal? gtquyettoanden, int? nguonkinhphi);
    }

    public class NhiemVuRepository : BaseRepository<KHCN_NhiemVu>, INhiemVuRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public NhiemVuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_NhiemVu GetByMaSoNV(string manv)
        {
            var res = _unitOfWork.Context.Set<KHCN_NhiemVu>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(manv, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public KHCN_NhiemVu GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_NhiemVu>().AsEnumerable().Where(p => p.TenNhiemVu.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public List<KHCN_NhiemVu> GetByIdChuNhiemNV(int idChuNhiem)
        {
            var res = _unitOfWork.Context.Set<KHCN_NhiemVu>().AsEnumerable().Where(p => p.IdChuNhiemNV == idChuNhiem).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_NhiemVu> GetByIdLoaiNhiemVu(int idLoaiNhiemVu)
        {
            var res = _unitOfWork.Context.Set<KHCN_NhiemVu>().AsEnumerable().Where(p => p.IdLoaiNhiemVu == idLoaiNhiemVu).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_NhiemVu> GetByIdNganh(int idNganh)
        {
            var res = _unitOfWork.Context.Set<KHCN_NhiemVu>().AsEnumerable().Where(p => p.IdNganh == idNganh).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_NhiemVu> GetByIdCapQuanLy(int idCapQuanLy)
        {
            var res = _unitOfWork.Context.Set<KHCN_NhiemVu>().AsEnumerable().Where(p => p.IdCapQuanLy == idCapQuanLy).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_NhiemVu> GetByIdTienDo(int idTienDo)
        {
            var res = _unitOfWork.Context.Set<KHCN_NhiemVu>().AsEnumerable().Where(p => p.IdTienDoThucHien == idTienDo).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_NhiemVu> GetByIdDonViChuTri(int idDonVi)
        {
            var res = _unitOfWork.Context.Set<KHCN_NhiemVu>().AsEnumerable().Where(p => p.IdDonViChuTri == idDonVi).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public IQueryable<ThongTinNhiemVuVM> GetData(int? loainhiemvu, int? nganh, int? capquanly, int? donvichutri, int? nampheduyettu, int? nampheduyetden,
            int? namhoanthanhtu, int? namhoanthanhden, int? chunhiemnv, int? tiendothuchien, DateTime? tgbatdau, DateTime? tgketthuc,
            decimal? dutoanduyettu, decimal? dutoanduyetden, decimal? gtquyettoantu, decimal? gtquyettoanden, int? nguonkinhphi)
        {
            var infoNhiemvu = new List<KHCN_NhiemVu>();
            var infoTGThucHien = new List<KHCN_ThoiGianThucHien>();
            var infoKPThucHien = new List<KHCN_KinhPhiThucHien>();
            var lstInfoNhiemvu = _unitOfWork.Context.Set<KHCN_NhiemVu>().AsEnumerable();
            var lstInfoTGThucHien = _unitOfWork.Context.Set<KHCN_ThoiGianThucHien>().AsEnumerable();
            var lstInfoKPThucHien = _unitOfWork.Context.Set<KHCN_KinhPhiThucHien>().AsEnumerable();

            if (infoNhiemvu == null || infoTGThucHien == null || infoKPThucHien == null ||
                !infoNhiemvu.Any() || !infoTGThucHien.Any() || !infoKPThucHien.Any())
                return null;

            #region Filter nhiệm vụ

            if (loainhiemvu != null && loainhiemvu.Value > 0)
                infoNhiemvu = lstInfoNhiemvu.Where(p => p.IdLoaiNhiemVu == loainhiemvu.Value).ToList();

            if (capquanly != null && capquanly.Value > 0)
                infoNhiemvu = infoNhiemvu.Where(p => p.IdCapQuanLy == capquanly.Value).ToList();

            if (donvichutri != null && donvichutri.Value > 0)
                infoNhiemvu = infoNhiemvu.Where(p => p.IdDonViChuTri == donvichutri.Value).ToList();

            if (nampheduyettu != null && nampheduyettu.Value > 0)
                infoNhiemvu = infoNhiemvu.Where(p => p.NamPheDuyet >= nampheduyettu.Value).ToList();

            if (nampheduyetden != null && nampheduyetden.Value > 0)
                infoNhiemvu = infoNhiemvu.Where(p => p.NamPheDuyet <= nampheduyetden.Value).ToList();

            if (namhoanthanhtu != null && namhoanthanhtu.Value > 0)
                infoNhiemvu = infoNhiemvu.Where(p => p.NamHoanThanh >= namhoanthanhtu.Value).ToList();

            if (namhoanthanhden != null && nampheduyetden.Value > 0)
                infoNhiemvu = infoNhiemvu.Where(p => p.NamHoanThanh <= namhoanthanhden.Value).ToList();

            if (chunhiemnv != null && chunhiemnv.Value > 0)
                infoNhiemvu = infoNhiemvu.Where(p => p.IdChuNhiemNV == chunhiemnv.Value).ToList();

            if (tiendothuchien != null && tiendothuchien.Value > 0)
                infoNhiemvu = infoNhiemvu.Where(p => p.IdTienDoThucHien == tiendothuchien.Value).ToList();

            #endregion Nhiệm vụ

            if (infoNhiemvu != null && infoNhiemvu.Any())
            {
                infoKPThucHien = infoKPThucHien.Where(p => infoNhiemvu.Select(m => m.MaSoNhiemVu).Contains(p.MaSoNhiemVu)).ToList();

                #region Filter thời gian thực hiện

                if (tgbatdau != null && tgbatdau.Value > new DateTime(1970, 1, 1))
                    infoTGThucHien = infoTGThucHien.Where(p => p.TGBatDau >= tgbatdau.Value).ToList();

                if (tgketthuc != null && tgketthuc.Value > new DateTime(1970, 1, 1))
                    infoTGThucHien = infoTGThucHien.Where(p => p.TGKetThuc <= tgketthuc.Value).ToList();

                #endregion Thời gian thực hiện

                if (infoKPThucHien != null && infoKPThucHien.Any())
                {
                    infoKPThucHien = infoKPThucHien.Where(p => infoKPThucHien.Select(m => m.MaSoNhiemVu).Contains(p.MaSoNhiemVu)).ToList();

                    #region Filter kinh phí thực hiện

                    if (dutoanduyettu != null && dutoanduyettu.Value > 0)
                        infoKPThucHien = infoKPThucHien.Where(p => p.DuToanDuocDuyet >= dutoanduyettu.Value).ToList();

                    if (dutoanduyetden != null && dutoanduyetden.Value > 0)
                        infoKPThucHien = infoKPThucHien.Where(p => p.DuToanDuocDuyet <= dutoanduyetden.Value).ToList();

                    if (gtquyettoantu != null && gtquyettoantu.Value > 0)
                        infoKPThucHien = infoKPThucHien.Where(p => p.GiaTriQuyetToan >= gtquyettoantu.Value).ToList();

                    if (gtquyettoanden != null && gtquyettoanden.Value > 0)
                        infoKPThucHien = infoKPThucHien.Where(p => p.GiaTriQuyetToan <= gtquyettoanden.Value).ToList();

                    //if (nguonkinhphi != null && nguonkinhphi.Value > 0)
                    //    infoKPThucHien = infoKPThucHien.Where(p => p.NguonKinhPhi <= nguonkinhphi.Value).ToList();

                    #endregion Kinh phí thực hiện
                }
            }

            if (infoKPThucHien != null && infoKPThucHien.Any())
            {
                var lstMSNhiemVu = infoKPThucHien.Select(m => m.MaSoNhiemVu).ToList();
                infoNhiemvu = lstInfoNhiemvu.Where(m => lstMSNhiemVu.Contains(m.MaSoNhiemVu)).ToList();
                infoTGThucHien = lstInfoTGThucHien.Where(m => lstMSNhiemVu.Contains(m.MaSoNhiemVu)).ToList();
                infoKPThucHien = lstInfoKPThucHien.Where(m => lstMSNhiemVu.Contains(m.MaSoNhiemVu)).ToList();

                var result = from nv in infoNhiemvu
                             join tg in infoTGThucHien on nv.MaSoNhiemVu equals tg.MaSoNhiemVu
                             join kp in infoKPThucHien on tg.MaSoNhiemVu equals kp.MaSoNhiemVu
                             select new ThongTinNhiemVuVM
                             {
                                 IdNhiemVu = nv.Id,
                                 MaSoNhiemVu = nv.MaSoNhiemVu,
                                 TenNhiemVu = nv.TenNhiemVu,
                                 IdLoaiNhiemVu = nv.IdLoaiNhiemVu,
                                 LoaiNhiemVu = nv.LoaiNhiemVu,
                                 IdNganh = nv.IdNganh,
                                 TenNganh = nv.TenNganh,
                                 IdCapQuanLy = nv.IdCapQuanLy,
                                 CapQuanLy = nv.CapQuanLy,
                                 IdDonViChuTri = nv.IdDonViChuTri,
                                 DonViChuTri = nv.DonViChuTri,
                                 NamPheDuyet = nv.NamPheDuyet,
                                 NamHoanThanh = nv.NamHoanThanh,
                                 IdChuNhiemNV = nv.IdChuNhiemNV,
                                 ChuNhiemNV = nv.ChuNhiemNV,
                                 EmailChuNhiem = nv.EmailChuNhiem,
                                 IdTienDoThucHien = nv.IdTienDoThucHien,
                                 TienDoThucHien = nv.TienDoThucHien,
                                 TGBatDau = tg.TGBatDau,
                                 TGKetThuc = tg.TGKetThuc,
                                 TGGiaHanMoiNhat = tg.TGGiaHanMoiNhat,
                                 TienTrinhGiaHan = tg.TienTrinhGiaHan,
                                 TongTGThucHien = tg.TongTGThucHien,
                                 DuToanDuocDuyet = kp.DuToanDuocDuyet,
                                 DuToanDieuChinh = kp.DuToanDieuChinh,
                                 TienTrinhDieuChinh = kp.TienTrinhDieuChinh,
                                 GiaTriQuyetToan = kp.GiaTriQuyetToan,
                                 NguonKinhPhi = kp.NguonKinhPhi,
                             };

                return result.AsQueryable();
            }


            return null;
        }
    }
}