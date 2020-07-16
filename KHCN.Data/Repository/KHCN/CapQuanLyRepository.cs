using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface ICapQuanLyRepository : IGenericRepository<KHCN_CapQuanLy>
    {
        KHCN_CapQuanLy GetByName(string name);
    }

    public class CapQuanLyRepository : BaseRepository<KHCN_CapQuanLy>, ICapQuanLyRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CapQuanLyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_CapQuanLy GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_CapQuanLy>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}