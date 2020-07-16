using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface IHoSoXetDuyetRepository : IGenericRepository<HoSoXetDuyet>
    {
    }

    public class HoSoXetDuyetRepository : BaseRepository<HoSoXetDuyet>, IHoSoXetDuyetRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoXetDuyetRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}