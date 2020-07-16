using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface ITrinhDoRepository : IGenericRepository<KHCN_TrinhDo>
    {
        KHCN_TrinhDo GetByName(string name);
    }

    public class TrinhDoRepository : BaseRepository<KHCN_TrinhDo>, ITrinhDoRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrinhDoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_TrinhDo GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_TrinhDo>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}