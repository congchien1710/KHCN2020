using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IRoleApiRepository : IGenericRepository<CMS_RoleApi>
    {
        IEnumerable<CMS_RoleApi> GetByIdRole(int idRole);
        IEnumerable<CMS_RoleApi> GetByIdApi(int idApi);
        CMS_RoleApi GetByIdApiIdRole(int idApi, int idRole);
    }

    public class RoleApiRepository : BaseRepository<CMS_RoleApi>, IRoleApiRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleApiRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CMS_RoleApi> GetByIdRole(int idRole)
        {
            var res = _unitOfWork.Context.Set<CMS_RoleApi>().AsEnumerable().Where(p => p.IdRole == idRole).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_RoleApi>();
        }

        public IEnumerable<CMS_RoleApi> GetByIdApi(int idApi)
        {
            var res = _unitOfWork.Context.Set<CMS_RoleApi>().AsEnumerable().Where(p => p.IdApi == idApi).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_RoleApi>();
        }

        public CMS_RoleApi GetByIdApiIdRole(int idApi, int idRole)
        {
            var res = _unitOfWork.Context.Set<CMS_RoleApi>().AsEnumerable().Where(p => p.IdApi == idApi && p.IdRole == idRole).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();

            return new CMS_RoleApi();
        }
    }
}