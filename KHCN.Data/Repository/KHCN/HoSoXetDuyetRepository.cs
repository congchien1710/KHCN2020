using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IHoSoXetDuyetRepository : IGenericRepository<KHCN_HoSoXetDuyet>
    {
        List<KHCN_HoSoXetDuyet> GetByMaSoNhiemVu(string masonhiemvu);
    }

    public class HoSoXetDuyetRepository : BaseRepository<KHCN_HoSoXetDuyet>, IHoSoXetDuyetRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoXetDuyetRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KHCN_HoSoXetDuyet> GetByMaSoNhiemVu(string masonhiemvu)
        {
            var res = _unitOfWork.Context.Set<KHCN_HoSoXetDuyet>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonhiemvu, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}