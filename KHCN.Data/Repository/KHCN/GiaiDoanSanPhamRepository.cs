using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IGiaiDoanSanPhamRepository : IGenericRepository<KHCN_GiaiDoanSanPham>
    {
        List<KHCN_GiaiDoanSanPham> GetByMaSoNhiemVu(string masonhiemvu);
        List<KHCN_GiaiDoanSanPham> GetByMaGiaiDoan(string magiaidoan);
    }

    public class GiaiDoanSanPhamRepository : BaseRepository<KHCN_GiaiDoanSanPham>, IGiaiDoanSanPhamRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public GiaiDoanSanPhamRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KHCN_GiaiDoanSanPham> GetByMaSoNhiemVu(string masonhiemvu)
        {
            var res = _unitOfWork.Context.Set<KHCN_GiaiDoanSanPham>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonhiemvu, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_GiaiDoanSanPham> GetByMaGiaiDoan(string magiaidoan)
        {
            var res = _unitOfWork.Context.Set<KHCN_GiaiDoanSanPham>().AsEnumerable().Where(p => p.MaGiaiDoan.Equals(magiaidoan, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}