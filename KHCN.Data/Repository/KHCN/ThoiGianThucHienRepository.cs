using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IThoiGianThucHienRepository : IGenericRepository<KHCN_ThoiGianThucHien>
    {
        List<KHCN_ThoiGianThucHien> GetByMaSoNhiemVu(string masonhiemvu);
    }

    public class ThoiGianThucHienRepository : BaseRepository<KHCN_ThoiGianThucHien>, IThoiGianThucHienRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThoiGianThucHienRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KHCN_ThoiGianThucHien> GetByMaSoNhiemVu(string masonhiemvu)
        {
            var res = _unitOfWork.Context.Set<KHCN_ThoiGianThucHien>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonhiemvu, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}