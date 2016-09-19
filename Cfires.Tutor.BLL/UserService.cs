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

        public Base_User GetById(int id)
        {
            return _userRepostory.GetById(id);
        }

        public Base_User GetByEmail(string email)
        {
            return _userRepostory.GetByEmail(email);
        }

        public void Create(Base_User user)
        {
            user.CreateDate = DateTime.Now;
            user.Password = SecurityHelper.EncryptAES(user.Password);
            _userRepostory.Insert(user);
        }

        #region 用户管理

        public IEnumerable<Base_User> GetUserList()
        {
            return _userRepostory.GetUserList();
        }
        #endregion
    }
}
