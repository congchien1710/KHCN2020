using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IPhongBanRepository : IGenericRepository<KHCN_PhongBan>
    {
        KHCN_PhongBan GetByName(string name);
    }

    public class PhongBanRepository : BaseRepository<KHCN_PhongBan>, IPhongBanRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PhongBanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_PhongBan GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_PhongBan>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}