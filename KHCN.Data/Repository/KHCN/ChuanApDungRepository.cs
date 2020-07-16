using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IChuanApDungRepository : IGenericRepository<KHCN_ChuanApDung>
    {
        KHCN_ChuanApDung GetByName(string name);
    }

    public class ChuanApDungRepository : BaseRepository<KHCN_ChuanApDung>, IChuanApDungRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChuanApDungRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_ChuanApDung GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_ChuanApDung>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}