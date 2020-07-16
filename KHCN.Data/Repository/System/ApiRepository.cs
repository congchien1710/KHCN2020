using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IApiRepository : IGenericRepository<CMS_Api>
    {
        CMS_Api GetByName(string name);
        IEnumerable<CMS_Api> GetByIdParent(int idParent);
        IEnumerable<CMS_Api> GetByModule(int idModule);
    }

    public class ApiRepository : BaseRepository<CMS_Api>, IApiRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApiRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CMS_Api> GetByIdParent(int idParent)
        {
            var res = _unitOfWork.Context.Set<CMS_Api>().AsEnumerable().Where(p => p.IdParent == idParent).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_Api>();
        }

        public IEnumerable<CMS_Api> GetByModule(int idModule)
        {
            var res = _unitOfWork.Context.Set<CMS_Api>().AsEnumerable().Where(p => p.IdModule == idModule).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_Api>();
        }

        public CMS_Api GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<CMS_Api>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}