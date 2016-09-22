using Cfires.Tutor.Model;
using NPoco;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Cfires.Tutor.DAL
{
    public class UserRepository : Repository<Base_User>
    {
        IDatabase db = new Database("connStr");

        public Base_User GetByEmail(string email)
        {
            var sql = Sql.Builder.Append("SELECT * FROM Base_User WHERE Email = @0", email);
            return db.FirstOrDefault<Base_User>(sql);
        }

        #region 用户管理

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<Base_User> GetList(int start, int end)
        {
            string sql = "SELECT * FROM (SELECT row_number() over(order by Id) as num,* from Base_User) as t where t.num >=@start and t.num <=@end";

            SqlParameter[] pars = {
                new SqlParameter("@start",start),
                new SqlParameter("@end",end)
            };
            List<Base_User> list = new List<Base_User>();
            DataTable dt = SqlHelper.ExecuteDataTable(sql, pars);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Base_User()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Password = row["Password"].ToString(),
                    Email = row["Email"].ToString(),
                    Type = (UserType)row["Type"],
                    CreateDate = (DateTime)row["CreateDate"]
                });
            }
            return list;
        }
        #endregion
    }
}
