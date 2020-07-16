using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface IHoSoQuyetToanRepository : IGenericRepository<HoSoQuyetToan>
    {
    }

    public class HoSoQuyetToanRepository : BaseRepository<HoSoQuyetToan>, IHoSoQuyetToanRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoQuyetToanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}