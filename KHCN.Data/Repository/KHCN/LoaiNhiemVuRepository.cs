using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface ILoaiNhiemVuRepository : IGenericRepository<KHCN_LoaiNhiemVu>
    {
        KHCN_LoaiNhiemVu GetByName(string name);
    }

    public class LoaiNhiemVuRepository : BaseRepository<KHCN_LoaiNhiemVu>, ILoaiNhiemVuRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoaiNhiemVuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_LoaiNhiemVu GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_LoaiNhiemVu>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}