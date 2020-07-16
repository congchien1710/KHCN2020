using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface INganhRepository : IGenericRepository<KHCN_Nganh>
    {
        KHCN_Nganh GetByName(string name);
    }

    public class NganhRepository : BaseRepository<KHCN_Nganh>, INganhRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public NganhRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_Nganh GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_Nganh>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}