using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface IHoSoDieuChinhRepository : IGenericRepository<HoSoDieuChinh>
    {
    }

    public class HoSoDieuChinhRepository : BaseRepository<HoSoDieuChinh>, IHoSoDieuChinhRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoDieuChinhRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}