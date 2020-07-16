using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IDonViCheTaoRepository : IGenericRepository<KHCN_DonViCheTao>
    {
        KHCN_DonViCheTao GetByName(string name);
    }

    public class DonViCheTaoRepository : BaseRepository<KHCN_DonViCheTao>, IDonViCheTaoRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public DonViCheTaoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_DonViCheTao GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_DonViCheTao>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}