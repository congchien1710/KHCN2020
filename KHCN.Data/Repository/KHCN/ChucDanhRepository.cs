using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IChucDanhRepository : IGenericRepository<KHCN_ChucDanh>
    {
        KHCN_ChucDanh GetByName(string name);
    }

    public class ChucDanhRepository : BaseRepository<KHCN_ChucDanh>, IChucDanhRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChucDanhRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_ChucDanh GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_ChucDanh>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}