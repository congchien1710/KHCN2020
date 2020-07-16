using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface IThoiGianThucHienRepository : IGenericRepository<ThoiGianThucHien>
    {
    }

    public class ThoiGianThucHienRepository : BaseRepository<ThoiGianThucHien>, IThoiGianThucHienRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThoiGianThucHienRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}