using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IHoSoDieuChinhKinhPhiRepository : IGenericRepository<KHCN_HoSoDieuChinhKinhPhi>
    {
        List<KHCN_HoSoDieuChinhKinhPhi> GetByMaSoNhiemVu(string masonhiemvu);
    }

    public class HoSoDieuChinhKinhPhiRepository : BaseRepository<KHCN_HoSoDieuChinhKinhPhi>, IHoSoDieuChinhKinhPhiRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoDieuChinhKinhPhiRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KHCN_HoSoDieuChinhKinhPhi> GetByMaSoNhiemVu(string masonhiemvu)
        {
            var res = _unitOfWork.Context.Set<KHCN_HoSoDieuChinhKinhPhi>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonhiemvu, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}