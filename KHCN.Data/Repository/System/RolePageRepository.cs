using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IRolePageRepository : IGenericRepository<CMS_RolePage>
    {
        IEnumerable<CMS_RolePage> GetByIdRole(int idRole);
        IEnumerable<CMS_RolePage> GetByIdPage(int idPage);
        CMS_RolePage GetByIdPageIdRole(int idPage, int idRole);
    }

    public class RolePageRepository : BaseRepository<CMS_RolePage>, IRolePageRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolePageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CMS_RolePage> GetByIdRole(int idRole)
        {
            var res = _unitOfWork.Context.Set<CMS_RolePage>().AsEnumerable().Where(p => p.IdRole == idRole).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_RolePage>();
        }

        public IEnumerable<CMS_RolePage> GetByIdPage(int idPage)
        {
            var res = _unitOfWork.Context.Set<CMS_RolePage>().AsEnumerable().Where(p => p.IdPage == idPage).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_RolePage>();
        }

        public CMS_RolePage GetByIdPageIdRole(int idPage, int idRole)
        {
            var res = _unitOfWork.Context.Set<CMS_RolePage>().AsEnumerable().Where(p => p.IdPage == idPage && p.IdRole == idRole).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();

            return new CMS_RolePage();
        }
    }
}