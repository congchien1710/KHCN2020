using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface ILoaiNhiemVuRepository : IGenericRepository<LoaiNhiemVu>
    {
    }

    public class LoaiNhiemVuRepository : BaseRepository<LoaiNhiemVu>, ILoaiNhiemVuRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoaiNhiemVuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}