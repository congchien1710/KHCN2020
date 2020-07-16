using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IKinhPhiThucHienRepository : IGenericRepository<KHCN_KinhPhiThucHien>
    {
        List<KHCN_KinhPhiThucHien> GetByMaSoNhiemVu(string masonhiemvu);
    }

    public class KinhPhiThucHienRepository : BaseRepository<KHCN_KinhPhiThucHien>, IKinhPhiThucHienRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public KinhPhiThucHienRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<KHCN_KinhPhiThucHien> GetByMaSoNhiemVu(string masonhiemvu)
        {
            var res = _unitOfWork.Context.Set<KHCN_KinhPhiThucHien>().AsEnumerable().Where(p => p.MaSoNhiemVu.Equals(masonhiemvu, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}