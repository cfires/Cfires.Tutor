using Cfires.Tutor.DAL;
using Cfires.Tutor.Model;
using Cfires.Tutor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfires.Tutor.BLL
{
    public class UserService
    {
        UserRepository _userRepostory = new UserRepository();

        public Base_User Get(int id)
        {
            return _userRepostory.Get(id);
        }

        public Base_User GetByEmail(string email)
        {
            return _userRepostory.GetByEmail(email);
        }

        public void Create(Base_User user)
        {
            user.CreateDate = DateTime.Now;
            user.Password = SecurityHelper.EncryptAES(user.Password);
            user.Enabled = true;
            _userRepostory.Insert(user);
        }

        #region 用户管理

        public IEnumerable<Base_User> GetUserList(int pageIndex, int pageSize)
        {
            int start = (pageIndex - 1) * pageSize + 1;
            int end = pageIndex * pageSize;
            return _userRepostory.GetList(start, end);
        }
        #endregion
    }
}
