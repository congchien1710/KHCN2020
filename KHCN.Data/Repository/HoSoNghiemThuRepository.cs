using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface IHoSoNghiemThuRepository : IGenericRepository<HoSoNghiemThu>
    {
    }

    public class HoSoNghiemThuRepository : BaseRepository<HoSoNghiemThu>, IHoSoNghiemThuRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoSoNghiemThuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}