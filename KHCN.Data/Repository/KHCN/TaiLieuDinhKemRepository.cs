using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using KHCN.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository.KHCN
{
    public interface ITaiLieuDinhKemRepository : IGenericRepository<KHCN_TaiLieuDinhKem>
    {
        KHCN_TaiLieuDinhKem GetByGuid(string guid);
        List<KHCN_TaiLieuDinhKem> GetByGuid(List<string> lstGuid);
    }

    public class TaiLieuDinhKemRepository : BaseRepository<KHCN_TaiLieuDinhKem>, ITaiLieuDinhKemRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaiLieuDinhKemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_TaiLieuDinhKem GetByGuid(string guid)
        {
            var res = _unitOfWork.Context.Set<KHCN_TaiLieuDinhKem>().AsEnumerable().Where(p => p.Guid == guid).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public List<KHCN_TaiLieuDinhKem> GetByGuid(List<string> lstGuid)
        {
            var res = _unitOfWork.Context.Set<KHCN_TaiLieuDinhKem>().AsEnumerable().Where(p => lstGuid.Contains(p.Guid)).ToList();
            if (res != null && res.Any())
                return res;
            return null;
        }
    }
}