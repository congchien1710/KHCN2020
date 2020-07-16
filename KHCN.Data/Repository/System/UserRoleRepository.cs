using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IUserRoleRepository : IGenericRepository<CMS_UserRole>
    {
        CMS_UserRole GetByIdUserIdRole(int idUser, int idRole);
        IEnumerable<CMS_UserRole> GetByIdRole(int idRole);
        IEnumerable<CMS_UserRole> GetByIdUser(int idUser);
        IEnumerable<CMS_UserRole> GetByIdUserIdRole(List<int> lstIdUser, List<int> lstIdRole);

        Task<CMS_UserRole> GetByIdUserIdRoleAsync(int idUser, int idRole);
        Task<IEnumerable<CMS_UserRole>> GetByIdRoleAsync(int idRole);
        Task<IEnumerable<CMS_UserRole>> GetByIdUserAsync(int idUser);
        Task<IEnumerable<CMS_UserRole>> GetByIdUserIdRoleAsync(List<int> lstIdUser, List<int> lstIdRole);
    }

    public class UserRoleRepository : BaseRepository<CMS_UserRole>, IUserRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<CMS_UserRole> GetByIdRole(int idRole)
        {
            return _unitOfWork.Context.Set<CMS_UserRole>().Where(p => p.IdRole == idRole).ToList();
        }
        public IEnumerable<CMS_UserRole> GetByIdUser(int idUser)
        {
            return _unitOfWork.Context.Set<CMS_UserRole>().Where(p => p.IdUser == idUser).ToList();
        }
        public CMS_UserRole GetByIdUserIdRole(int idUser, int idRole)
        {
            return _unitOfWork.Context.Set<CMS_UserRole>().FirstOrDefault(p => p.IdUser == idUser && p.IdRole == idRole);
        }
        public IEnumerable<CMS_UserRole> GetByIdUserIdRole(List<int> lstIdUser, List<int> lstIdRole)
        {
            return _unitOfWork.Context.Set<CMS_UserRole>().Where(p => lstIdUser.Contains(p.IdUser) && lstIdRole.Contains(p.IdRole)).ToList();
        }

        public async Task<IEnumerable<CMS_UserRole>> GetByIdRoleAsync(int idRole)
        {
            return await _unitOfWork.Context.Set<CMS_UserRole>().Where(p => p.IdRole == idRole).ToListAsync();
        }
        public async Task<IEnumerable<CMS_UserRole>> GetByIdUserAsync(int idUser)
        {
            return await _unitOfWork.Context.Set<CMS_UserRole>().Where(p => p.IdRole == idUser).ToListAsync();
        }
        public async Task<CMS_UserRole> GetByIdUserIdRoleAsync(int idUser, int idRole)
        {
            return await _unitOfWork.Context.Set<CMS_UserRole>().Where(p => p.IdUser == idUser && p.IdRole == idRole).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<CMS_UserRole>> GetByIdUserIdRoleAsync(List<int> lstIdUser, List<int> lstIdRole)
        {
            return await _unitOfWork.Context.Set<CMS_UserRole>().Where(p => lstIdUser.Contains(p.IdUser) && lstIdRole.Contains(p.IdRole)).ToListAsync();
        }
    }
}