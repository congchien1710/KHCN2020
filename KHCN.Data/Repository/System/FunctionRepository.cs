using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IFunctionRepository : IGenericRepository<CMS_Function>
    {
        CMS_Function GetByName(string name);
        IEnumerable<CMS_Function> GetByIdParent(int idParent);
        IEnumerable<CMS_Function> GetByModule(int idModule);
    }

    public class FunctionRepository : BaseRepository<CMS_Function>, IFunctionRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public FunctionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CMS_Function> GetByIdParent(int idParent)
        {
            var res = _unitOfWork.Context.Set<CMS_Function>().AsEnumerable().Where(p => p.IdParent == idParent).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_Function>();
        }

        public IEnumerable<CMS_Function> GetByModule(int idModule)
        {
            var res = _unitOfWork.Context.Set<CMS_Function>().AsEnumerable().Where(p => p.IdModule == idModule).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return new List<CMS_Function>();
        }

        public CMS_Function GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<CMS_Function>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}