using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IHoSoQuyetToanRepository : IGenericRepository<KHCN_HoSoQuyetToan>
    {
        List<KHCN_HoSoQuyetToan> GetByMaSoNhiemVu(string masonhiemvu);
    }

    public class HoSoQuyetToanRepository : BaseRepository<KHCN_HoSoQuyetToan>, IHoSoQuyetToanRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoQuyetToanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KHCN_HoSoQuyetToan> GetByMaSoNhiemVu(string masonhiemvu)
        {
            var res = _unitOfWork.Context.Set<KHCN_HoSoQuyetToan>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonhiemvu, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}