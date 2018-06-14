using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Redis
{
    public abstract class RedisBase
    {
        readonly ConnectionMultiplexer db = null;
        readonly string prefix = string.Empty;
        readonly int dbNumber = 0;

        public RedisBase(int dbnum, string prefix, string connectionString = null)
        {
            this.dbNumber = dbnum;
            this.db = string.IsNullOrWhiteSpace(connectionString) ? RedisManager.Instance : RedisManager.GetFromCache(connectionString);
            this.prefix = prefix;
        }

        #region 数据库操作

        /// <summary>
        /// 生成数据库中真实的key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GenRealKey(string key)
        {
            return prefix + "_" + key;
        }

        /// <summary>
        /// 执行保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        protected T DoSave<T>(Func<IDatabase, T> func)
        {
            db.GetDatabase(dbNumber);
            return func(db.GetDatabase(dbNumber));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Delete(string key)
        {
            key = GenRealKey(key);
            return db.GetDatabase(dbNumber).KeyDelete(key);
        }

        /// <summary>
        /// key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExist(string key)
        {
            key = GenRealKey(key);
            return db.GetDatabase(dbNumber).KeyExists(key);
        }

        /// <summary>
        /// 重命名key
        /// </summary>
        /// <param name="oldKey"></param>
        /// <param name="newKey"></param>
        /// <returns></returns>
        public bool KeyRename(string oldKey, string newKey)
        {
            oldKey = GenRealKey(oldKey);
            newKey = GenRealKey(newKey);
            return db.GetDatabase(dbNumber).KeyRename(oldKey, newKey);
        }

        /// <summary>
        /// 模糊匹配查找key
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public List<string> Keys(string pattern)
        {
            List<string> lstKey = new List<string>();
            var points = this.db.GetEndPoints();
            foreach (var p in points)
            {
                var s = this.db.GetServer(p);
                var keys = s.Keys(dbNumber, pattern).Select(x => (string)x).ToList();
                if (keys != null) lstKey.AddRange(keys);
            }
            return lstKey;
        }

        /// <summary>
        /// 模糊匹配查找符合条件的key数量
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public int KeyCount(string pattern)
        {
            int count = 0;
            var points = this.db.GetEndPoints();
            foreach (var p in points)
            {
                var s = this.db.GetServer(p);
                var keys = s.Keys(dbNumber, pattern).Select(x => (string)x).ToList();
                count += keys.Count();
            }
            return count;
        }


        #endregion

        #region 数据类型转换

        protected string ConvertJson<T>(T val)
        {
            return val is string ? val.ToString() : JsonConvert.SerializeObject(val);
        }

        protected T ConvertObj<T>(RedisValue val)
        {
            return JsonConvert.DeserializeObject<T>(val);
        }

        protected List<T> ConvertList<T>(RedisValue[] val)
        {
            List<T> result = new List<T>();
            foreach (var item in val)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }

        protected RedisKey[] ConvertRedisKeys(List<string> val)
        {
            return val.Select(k => (RedisKey)k).ToArray();
        }

        #endregion
    }
}
