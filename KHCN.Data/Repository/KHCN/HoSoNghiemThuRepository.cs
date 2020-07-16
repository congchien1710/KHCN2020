using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IHoSoNghiemThuRepository : IGenericRepository<KHCN_HoSoNghiemThu>
    {
        List<KHCN_HoSoNghiemThu> GetByMaSoNhiemVu(string masonhiemvu);
    }

    public class HoSoNghiemThuRepository : BaseRepository<KHCN_HoSoNghiemThu>, IHoSoNghiemThuRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoNghiemThuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KHCN_HoSoNghiemThu> GetByMaSoNhiemVu(string masonhiemvu)
        {
            var res = _unitOfWork.Context.Set<KHCN_HoSoNghiemThu>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonhiemvu, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}