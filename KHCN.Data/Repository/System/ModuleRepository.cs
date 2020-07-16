using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IModuleRepository : IGenericRepository<CMS_Module>
    {
        CMS_Module GetByName(string name);
    }

    public class ModuleRepository : BaseRepository<CMS_Module>, IModuleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ModuleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CMS_Module GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<CMS_Module>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}