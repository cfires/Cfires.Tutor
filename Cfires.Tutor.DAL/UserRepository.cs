using Cfires.Tutor.Model;
using NPoco;
using System.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Cfires.Tutor.DAL
{
    public class UserRepository
    {
        IDatabase db = new Database("connStr");

        public Base_User GetById(int id)
        {
            var sql = Sql.Builder.Append("SELECT * FROM Base_User WHERE Id = @0", id);
            return db.FirstOrDefault<Base_User>(sql);
        }

        public Base_User GetByEmail(string email)
        {
            var sql = Sql.Builder.Append("SELECT * FROM Base_User WHERE Email = @0", email);
            return db.FirstOrDefault<Base_User>(sql);
        }

        public void Insert(Base_User user)
        {
            db.Insert<Base_User>(user);
        }

        #region 用户管理

        public List<Base_User> GetUserList()
        {
            return null;
        }
        #endregion
    }
}
