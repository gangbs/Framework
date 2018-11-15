using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EF
{
    public interface IBaseRepository<T> where T : DbEntity
    {
        IQueryable<T> DbSet { get; }

        #region 查询

        T Get(params object[] key);

        T Get(Expression<Func<T, bool>> fliter);

        IQueryable<T> GetAllIncluding<TProperty>(Expression<Func<T, TProperty>> propertySelectors);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetList(Expression<Func<T, bool>> filter);

        IEnumerable<T> GetList(string sql, params object[] parameters);

        IEnumerable<TReturn> GetList<TReturn>(string sql, params object[] parameters);

        IEnumerable<T> GetPaging<K>(Expression<Func<T, bool>> filter, Expression<Func<T, K>> orderFiled, int pageSize, int pageNum, out int count, bool isAsc = true);

        IEnumerable<TReturn> GetList<TReturn>(IQueryable<TReturn> linq) where TReturn : DbEntity;

        #endregion

        #region 增加

        SaveResult Insert(T entity, bool isSaveChange = true);

        SaveResult InsertMany(IEnumerable<T> lst, bool isSaveChange = true);

        //SaveResult BatchInsert(IEnumerable<T> lst);

        #endregion

        #region 编辑

        SaveResult Update(T entity, bool isSaveChange = true);

        SaveResult UpdateProperty(Expression<Func<T, bool>> filter, string filedName, object filedValue, bool isSaveChange = true);

        SaveResult UpdatePropertys(Expression<Func<T, bool>> filter, Dictionary<string, object> fileds, bool isSaveChange = true);

        SaveResult UpdatePropertys(Expression<Func<T, bool>> filter, Action<T> change, bool isSaveChange = true);

        #endregion

        #region 删除

        SaveResult Delete(bool isSaveChange = true, params object[] key);

        SaveResult Delete(T Entity, bool isSaveChange = true);

        SaveResult Delete(Expression<Func<T, bool>> filter, bool isSaveChange = true);

        #endregion

        #region 其它

        SaveResult ExcuteSqlCommand(string sql, object[] parameters);

        bool IsExist(Expression<Func<T, bool>> filter);

        int Count(Expression<Func<T, bool>> filter);

        #endregion                
    }
}
