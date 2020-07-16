using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IRoleRepository : IGenericRepository<CMS_Role>
    {
        CMS_Role GetByName(string name);
    }

    public class RoleRepository : BaseRepository<CMS_Role>, IRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CMS_Role GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<CMS_Role>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}