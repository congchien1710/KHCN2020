using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface ICapQuanLyRepository : IGenericRepository<CapQuanLy>
    {
    }

    public class CapQuanLyRepository : BaseRepository<CapQuanLy>, ICapQuanLyRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CapQuanLyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}