using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IPageRepository : IGenericRepository<CMS_Page>
    {
        CMS_Page GetByName(string name);
        IEnumerable<CMS_Page> GetByIdParent(int idParent);
        IEnumerable<CMS_Page> GetByModule(int idModule);
    }

    public class PageRepository : BaseRepository<CMS_Page>, IPageRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CMS_Page GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<CMS_Page>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public IEnumerable<CMS_Page> GetByIdParent(int idParent)
        {
            var res = _unitOfWork.Context.Set<CMS_Page>().AsEnumerable().Where(p => p.IdParent == idParent).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_Page>();
        }

        public IEnumerable<CMS_Page> GetByModule(int idModule)
        {
            var res = _unitOfWork.Context.Set<CMS_Page>().AsEnumerable().Where(p => p.IdModule == idModule).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_Page>();
        }
    }
}