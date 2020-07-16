using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IHoSoDieuChinhKhacRepository : IGenericRepository<KHCN_HoSoDieuChinhKhac>
    {
        List<KHCN_HoSoDieuChinhKhac> GetByMaSoNhiemVu(string name);
    }

    public class HoSoDieuChinhKhacRepository : BaseRepository<KHCN_HoSoDieuChinhKhac>, IHoSoDieuChinhKhacRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoDieuChinhKhacRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KHCN_HoSoDieuChinhKhac> GetByMaSoNhiemVu(string masonhiemvu)
        {
            var res = _unitOfWork.Context.Set<KHCN_HoSoDieuChinhKhac>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonhiemvu, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}