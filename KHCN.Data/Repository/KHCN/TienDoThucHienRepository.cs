using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface ITienDoThucHienRepository : IGenericRepository<KHCN_TienDoThucHien>
    {
        KHCN_TienDoThucHien GetByName(string name);
    }

    public class TienDoThucHienRepository : BaseRepository<KHCN_TienDoThucHien>, ITienDoThucHienRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public TienDoThucHienRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_TienDoThucHien GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_TienDoThucHien>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }
    }
}