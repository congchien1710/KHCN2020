using KHCN.Data.Entities.KHCN;
using KHCN.Data.Helpers;
using KHCN.Data.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository.KHCN
{
    public interface ICapBacRepository : IGenericRepository<KHCN_CapBac>
    {
        KHCN_CapBac GetByName(string name);
        PagedList<KHCN_CapBac> GetDataPaging(int? pageNumber, int? pageSize);
        Task<PagedList<KHCN_CapBac>> GetDataPagingAsync(int? pageNumber, int? pageSize);
    }

    public class CapBacRepository : BaseRepository<KHCN_CapBac>, ICapBacRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CapBacRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_CapBac GetByName(string name)
        {
            var res = _unitOfWork.Context.Set<KHCN_CapBac>().AsEnumerable().Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public PagedList<KHCN_CapBac> GetDataPaging(int? pageNumber, int? pageSize)
        {
            return PagedList<KHCN_CapBac>.ToPagedList(_unitOfWork.Context.Set<KHCN_CapBac>().AsQueryable().OrderBy(on => on.Id), pageNumber.Value, pageSize.Value);
        }

        public async Task<PagedList<KHCN_CapBac>> GetDataPagingAsync(int? pageNumber, int? pageSize)
        {
            return await PagedList<KHCN_CapBac>.ToPagedListAsync(_unitOfWork.Context.Set<KHCN_CapBac>().AsQueryable().OrderBy(on => on.Id), pageNumber.Value, pageSize.Value);
        }
    }
}