using Cfires.Tutor.Model;
using NPoco;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Cfires.Tutor.DAL
{
    public class Repository<T> where T : class , new()
    {
        IDatabase db = new Database("connStr");

        /// <summary>
        /// 获取实体(by 主键)
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns>实体对象</returns>
        public T Get(object primaryKey)
        {
            return db.SingleOrDefaultById<T>(primaryKey);
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(T entity)
        {
            db.Insert(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(T entity)
        {
            return db.Delete(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public int Delete(object primaryKey)
        {
            return db.Delete<T>(primaryKey);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public  void Update(T entity)
        {
            db.Update(entity);
        }
    }
}
