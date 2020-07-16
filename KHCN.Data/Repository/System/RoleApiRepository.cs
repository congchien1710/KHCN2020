using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IRoleFunctionRepository : IGenericRepository<CMS_RoleFunction>
    {
        IEnumerable<CMS_RoleFunction> GetByIdRole(int idRole);
        IEnumerable<CMS_RoleFunction> GetByIdFunction(int idFunction);
        CMS_RoleFunction GetByIdFunctionIdRole(int idFunction, int idRole);
    }

    public class RoleFunctionRepository : BaseRepository<CMS_RoleFunction>, IRoleFunctionRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleFunctionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CMS_RoleFunction> GetByIdRole(int idRole)
        {
            var res = _unitOfWork.Context.Set<CMS_RoleFunction>().AsEnumerable().Where(p => p.IdRole == idRole).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_RoleFunction>();
        }

        public IEnumerable<CMS_RoleFunction> GetByIdFunction(int idFunction)
        {
            var res = _unitOfWork.Context.Set<CMS_RoleFunction>().AsEnumerable().Where(p => p.IdFunction == idFunction).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_RoleFunction>();
        }

        public CMS_RoleFunction GetByIdFunctionIdRole(int idFunction, int idRole)
        {
            var res = _unitOfWork.Context.Set<CMS_RoleFunction>().AsEnumerable().Where(p => p.IdFunction == idFunction && p.IdRole == idRole).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();

            return new CMS_RoleFunction();
        }
    }
}