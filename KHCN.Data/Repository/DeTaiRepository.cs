using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface IDeTaiRepository : IGenericRepository<DeTai>
    {
    }

    public class DeTaiRepository : BaseRepository<DeTai>, IDeTaiRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeTaiRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}