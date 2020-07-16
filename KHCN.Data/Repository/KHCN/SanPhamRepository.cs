using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface ISanPhamRepository : IGenericRepository<KHCN_SanPham>
    {
        KHCN_SanPham GetByMaSP(string masanpham);

        KHCN_SanPham GetByName(string name);

        List<KHCN_SanPham> GetByIdNganh(int idNganh);
        List<KHCN_SanPham> GetByMaGiaiDoanSP(string maGiaiDoan);
        List<KHCN_SanPham> GetByIdChuanApDung(int idChuan);
        List<KHCN_SanPham> GetByIdDonViCheTao(int idDonVi);
        List<KHCN_SanPham> GetByMaSoNhiemVu(string masonv);
    }

    public class SanPhamRepository : BaseRepository<KHCN_SanPham>, ISanPhamRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SanPhamRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_SanPham GetByMaSP(string masanpham)
        {
            var res = _unitOfWork.Context.Set<KHCN_SanPham>().AsEnumerable().Where(p => p.MaSanPham.Equals(masanpham, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public KHCN_SanPham GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_SanPham>().AsEnumerable().Where(p => p.TenSanPham.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public List<KHCN_SanPham> GetByIdNganh(int idNganh)
        {
            var res = _unitOfWork.Context.Set<KHCN_SanPham>().AsEnumerable().Where(p => p.IdNganh == idNganh).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_SanPham> GetByMaGiaiDoanSP(string maGiaiDoan)
        {
            var res = _unitOfWork.Context.Set<KHCN_SanPham>().AsEnumerable().Where(p => p.MaGiaiDoan.Equals(maGiaiDoan, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_SanPham> GetByIdChuanApDung(int idChuan)
        {
            var res = _unitOfWork.Context.Set<KHCN_SanPham>().AsEnumerable().Where(p => p.IdChuanApDung == idChuan).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_SanPham> GetByIdDonViCheTao(int idDonVi)
        {
            var res = _unitOfWork.Context.Set<KHCN_SanPham>().AsEnumerable().Where(p => p.IdDonViCheTao == idDonVi).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_SanPham> GetByMaSoNhiemVu(string masonv)
        {
            var res = _unitOfWork.Context.Set<KHCN_SanPham>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonv, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}