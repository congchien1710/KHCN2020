using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IGiaiDoanRepository : IGenericRepository<KHCN_GiaiDoan>
    {
        KHCN_GiaiDoan GetByMa(string magd);
        KHCN_GiaiDoan GetByName(string name);
    }

    public class GiaiDoanRepository : BaseRepository<KHCN_GiaiDoan>, IGiaiDoanRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public GiaiDoanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_GiaiDoan GetByMa(string magd)
        {
            var res = _unitOfWork.Context.Set<KHCN_GiaiDoan>().AsEnumerable().Where(p => p.Code.Equals(magd, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public KHCN_GiaiDoan GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_GiaiDoan>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}