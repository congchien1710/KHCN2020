using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface INganhRepository : IGenericRepository<Nganh>
    {
    }

    public class NganhRepository : BaseRepository<Nganh>, INganhRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public NganhRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}