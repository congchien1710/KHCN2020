using KHCN.Data.Entities.KHCN;
using KHCN.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KHCN.Data.Repository.KHCN
{
    public interface IThanhVienDeTaiRepository : IGenericRepository<KHCN_ThanhVienDeTai>
    {
        KHCN_ThanhVienDeTai GetByMaNV(string name);
        List<KHCN_ThanhVienDeTai> GetByIdTrinhDo(int idTrinhDo);
        List<KHCN_ThanhVienDeTai> GetByIdCapBac(int idCapBac);
        List<KHCN_ThanhVienDeTai> GetByIdChucDanh(int idChucDanh);
        List<KHCN_ThanhVienDeTai> GetByIdPhongBan(int idPhongBan);
    }

    public class ThanhVienDeTaiRepository : BaseRepository<KHCN_ThanhVienDeTai>, IThanhVienDeTaiRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThanhVienDeTaiRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public KHCN_ThanhVienDeTai GetByMaNV(string manv)
        {
            var res = _unitOfWork.Context.Set<KHCN_ThanhVienDeTai>().AsEnumerable().Where(p => p.MaNV.Equals(manv, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public List<KHCN_ThanhVienDeTai> GetByIdTrinhDo(int idTrinhDo)
        {
            var res = _unitOfWork.Context.Set<KHCN_ThanhVienDeTai>().AsEnumerable().Where(p => p.IdTrinhDo == idTrinhDo).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_ThanhVienDeTai> GetByIdCapBac(int idCapBac)
        {
            var res = _unitOfWork.Context.Set<KHCN_ThanhVienDeTai>().AsEnumerable().Where(p => p.IdCapBac == idCapBac).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_ThanhVienDeTai> GetByIdChucDanh(int idChucDanh)
        {
            var res = _unitOfWork.Context.Set<KHCN_ThanhVienDeTai>().AsEnumerable().Where(p => p.IdChucDanh == idChucDanh).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }

        public List<KHCN_ThanhVienDeTai> GetByIdPhongBan(int idPhongBan)
        {
            var res = _unitOfWork.Context.Set<KHCN_ThanhVienDeTai>().AsEnumerable().Where(p => p.IdPhongBan == idPhongBan).ToList();
            if (res != null && res.Any())
                return res.ToList();
            return null;
        }
    }
}