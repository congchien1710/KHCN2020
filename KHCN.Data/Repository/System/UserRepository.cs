using KHCN.Data.Entities.System;
using KHCN.Data.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Repository
{
    public interface IUserRepository : IGenericRepository<CMS_User>
    {
        CMS_User GetByUsername(string name);
        bool Login(string username, out CMS_User userInfo);
        bool UpdatePassWord(int id, CMS_User model);
    }

    public class UserRepository : BaseRepository<CMS_User>, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public CMS_User GetByUsername(string username)
        {
            var res = _unitOfWork.Context.Set<CMS_User>().AsEnumerable().Where(p => p.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)).ToList();
            if (res != null && res.Any())
                return res.FirstOrDefault();
            return null;
        }

        public bool Login(string username, out CMS_User userInfo)
        {
            CMS_User model = _unitOfWork.Context.Set<CMS_User>().AsEnumerable().FirstOrDefault(m => m.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (model != null && model.Id > 0)
            {
                userInfo = model;
                return true;
            }

            userInfo = null;
            return false;
        }

        public bool UpdatePassWord(int id, CMS_User obj)
        {
            try
            {
                CMS_User model = _unitOfWork.Context.Set<CMS_User>().AsEnumerable().SingleOrDefault(m => m.Id == id);
                if (model != null && model.Id > 0)
                {
                    model.Password = obj.Password;
                    model.UpdatedBy = obj.UpdatedBy;
                    model.UpdatedDate = obj.UpdatedDate;

                    _unitOfWork.Context.Set<CMS_User>().Update(model);
                    _unitOfWork.Commit();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}