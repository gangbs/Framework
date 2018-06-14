using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EF
{
    public class BaseRepository<T> : IBaseRepository<T> where T : DbEntity
    {
        protected readonly DbContext context;
        protected readonly DbSet<T> dbSet;

        public BaseRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public IQueryable<T> DbSet
        {
            get
            {
                return this.dbSet.AsNoTracking();
            }
        }

        #region 查询

        public T Get(params object[] key)
        {
            T obj = dbSet.Find(key);

            if (obj != null)
            {
                context.Entry<T>(obj).State = EntityState.Detached;
            }

            return obj;
        }
        public T Get(Expression<Func<T, bool>> filter)
        {
            return this.dbSet.Where(filter).AsNoTracking().FirstOrDefault();
        }
        public IQueryable<T> GetAllIncluding<TProperty>(Expression<Func<T, TProperty>> propertySelectors)
        {
            return this.dbSet.Include<T, TProperty>(propertySelectors);
        }
        public IEnumerable<T> GetAll()
        {
            return this.dbSet.AsNoTracking();
        }
        public IEnumerable<T> GetList(Expression<Func<T, bool>> filter)
        {
            return this.dbSet.Where(filter).AsNoTracking();
        }
        public IEnumerable<TReturn> GetList<TReturn>(IQueryable<TReturn> linq) where TReturn : DbEntity
        {
            return linq.AsNoTracking();
        }
        public IEnumerable<T> GetList(string sql, params object[] parameters)
        {
            return this.dbSet.SqlQuery(sql, parameters).AsNoTracking();
        }
        public IEnumerable<TReturn> GetList<TReturn>(string sql, params object[] parameters)
        {
            return this.context.Database.SqlQuery<TReturn>(sql, parameters);
        }
        public IEnumerable<T> GetPaging<K>(Expression<Func<T, bool>> filter, Expression<Func<T, K>> orderFiled, int pageSize, int pageNum, out int count, bool isAsc = true)
        {
            count = dbSet.Count(filter);
            IEnumerable<T> lstReturn;

            if (isAsc)
            {
                lstReturn = dbSet.Where(filter).OrderBy(orderFiled).Skip(pageSize * (pageNum - 1)).Take(pageSize).AsNoTracking();
            }
            else
            {
                lstReturn = dbSet.Where(filter).OrderByDescending(orderFiled).Skip(pageSize * (pageNum - 1)).Take(pageSize).AsNoTracking();
            }
            return lstReturn;
        }

        #endregion

        #region 增加

        public int Insert(T entity, bool isSaveChange = true)
        {
            ////第一种方法
            //dbSet.Attach(entity);
            //context.Entry<T>(entity).State = EntityState.Added;

            //第二种方法
            dbSet.Add(entity); //EntityState.Detached

            return isSaveChange ? this.Save() : 0;
        }
        public int InsertMany(IEnumerable<T> lst, bool isSaveChange = true)
        {
            dbSet.AddRange(lst);

            return isSaveChange ? this.Save() : 0;
        }
        public int BatchInsert(IEnumerable<T> lst)
        {
            int count = 0;
            try
            {
                this.context.Configuration.AutoDetectChangesEnabled = false;
                dbSet.AddRange(lst);
                count = this.context.SaveChanges();
            }
            catch (Exception e)
            {
                count = 0;
            }
            finally
            {
                this.context.Configuration.AutoDetectChangesEnabled = true;
            }
            return count;
        }

        #endregion

        #region 编辑

        public int Update(T entity, bool isSaveChange = true)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;

            return isSaveChange ? this.Save() : 0;
        }

        public int UpdateProperty(Expression<Func<T, bool>> filter, string filedName, object filedValue, bool isSaveChange = true)
        {
            var lstEntity = this.dbSet.Where(filter);
            try
            {
                foreach (var entity in lstEntity)
                {
                    typeof(T).GetProperty(filedName).SetValue(entity, filedValue);
                }
            }
            catch
            {
                return 0;
            }
            return isSaveChange ? this.Save() : 0;
        }

        public int UpdatePropertys(Expression<Func<T, bool>> filter, Dictionary<string, object> fileds, bool isSaveChange = true)
        {
            var lstEntity = this.dbSet.Where(filter);
            try
            {
                foreach (var entity in lstEntity)
                {
                    foreach (KeyValuePair<string, object> kv in fileds)
                    {
                        typeof(T).GetProperty(kv.Key).SetValue(entity, kv.Value);
                    }
                }
            }
            catch
            {
                return 0;
            }
            return isSaveChange ? this.Save() : 0;
        }

        public int UpdatePropertys(Expression<Func<T, bool>> filter, Action<T> change, bool isSaveChange = true)
        {
            var lstEntity = this.dbSet.Where(filter);
            foreach (var entity in lstEntity)
            {
                change(entity);
            }
            return isSaveChange ? this.Save() : 0;
        }

        #endregion

        #region 删除

        public int Delete(bool isSaveChange = true, params object[] key)
        {
            T entity = this.dbSet.Find(key);
            return this.Delete(entity, isSaveChange);
        }

        public int Delete(T entity, bool isSaveChange = true)
        {
            dbSet.Remove(entity);
            return isSaveChange ? this.Save() : 0;
        }

        public int Delete(Expression<Func<T, bool>> filter, bool isSaveChange = true)
        {
            var lst = this.dbSet.Where(filter);
            dbSet.RemoveRange(lst);
            return isSaveChange ? this.Save() : 0;
        }

        #endregion

        #region 保存变更

        protected int Save()
        {
            string errMsg = "";
            return Save(out errMsg);
        }
        protected int Save(out string errMsg)
        {
            errMsg = "";
            int count = 0;
            try
            {
                count = context.SaveChanges();
            }
            catch (DbUpdateException exp)
            {
                errMsg = exp.InnerException.InnerException.Message;
            }
            return count;
        }

        #endregion

        #region 其它

        public int ExcuteSqlCommand(string sql, object[] parameters)
        {
            return this.context.Database.ExecuteSqlCommand(sql, parameters);
        }

        public bool IsExist(Expression<Func<T, bool>> filter)
        {
            return this.dbSet.Any(filter);
        }

        public int Count(Expression<Func<T, bool>> filter)
        {
            return this.dbSet.Count(filter);
        }

        #endregion

    }
}
