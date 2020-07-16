using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IHoSoDieuChinhThoiGianRepository : IGenericRepository<KHCN_HoSoDieuChinhThoiGian>
    {
        List<KHCN_HoSoDieuChinhThoiGian> GetByMaSoNhiemVu(string masonhiemvu);
    }

    public class HoSoDieuChinhThoiGianRepository : BaseRepository<KHCN_HoSoDieuChinhThoiGian>, IHoSoDieuChinhThoiGianRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoDieuChinhThoiGianRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KHCN_HoSoDieuChinhThoiGian> GetByMaSoNhiemVu(string masonhiemvu)
        {
            var res = _unitOfWork.Context.Set<KHCN_HoSoDieuChinhThoiGian>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonhiemvu, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}