using KHCN.Data.Entities;
using KHCN.Data.Interfaces;

namespace KHCN.Data.Repository
{
    public interface IKinhPhiThucHienRepository : IGenericRepository<KinhPhiThucHien>
    {
    }

    public class KinhPhiThucHienRepository : BaseRepository<KinhPhiThucHien>, IKinhPhiThucHienRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public KinhPhiThucHienRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}