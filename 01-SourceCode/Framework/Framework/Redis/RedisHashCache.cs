using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
   public class RedisHashCache : RedisBase
    {
        public RedisHashCache(int dbnum, string prefix, string connectionString = null) : base(dbnum, prefix, connectionString)
        {
        }

        public string HashGet(string key, string field)
        {
            key = this.GenRealKey(key);
            var val = this.DbHandler(db => db.HashGet(key, field));
            return val;
        }

        public T HashGet<T>(string key,string field)
        {
            key = this.GenRealKey(key);
            var val = this.DbHandler(db => db.HashGet(key,field));
            return this.ConvertObj<T>(val);
        }

        //public Dictionary<string,object> HashGetAll(string key)
        //{
        //    var dic = new Dictionary<string, object>();
        //    key = this.GenRealKey(key);
        //    var fields = this.DbHandler(db => db.HashGetAll(key));
        //    foreach(var field in fields)
        //    {
        //        dic.Add(field.Name,field.Value);
        //    }
        //    return dic;
        //}

        public List<HashEntry> HashGetAll(string key)
        {
            var dic = new Dictionary<string, object>();
            key = this.GenRealKey(key);
            return this.DbHandler(db => db.HashGetAll(key)).ToList();
        }

        public List<string> AllField(string key)
        {
            key = this.GenRealKey(key);
            var fields = this.DbHandler(db => db.HashKeys(key));
            var lstField = (from f in fields
                            select f.ToString()).ToList();
            return lstField;
        }

        public long FieldCount(string key)
        {
            key = this.GenRealKey(key);
            return this.DbHandler(db => db.HashLength(key));
        }

        public bool HashSet<T>(string key,string field,T obj)
        {
            key = this.GenRealKey(key);
            string json = ConvertJson<T>(obj);
            return this.DbHandler(db => db.HashSet(key,field, json));
        }

        //public void HashSet(string key, Dictionary<string,object> dic)
        //{
        //    List<HashEntry> lst = new List<HashEntry>();

        //    foreach(var kv in dic)
        //    {
        //        lst.Add(new HashEntry(kv.Key, (RedisValue)kv.Value));
        //    }

        //    var db = this.GetDatabase();
        //    db.HashSet(key, lst.ToArray());
        //}

        public void HashSet(string key, HashEntry[] arr)
        {
            key = this.GenRealKey(key);
            this.DbHandler(db => db.HashSet(key,arr));
        }

        public bool HashExist(string key,string field)
        {
            return this.DbHandler(db => db.HashExists(key, field));
        }

        public bool HashDelete(string key, string field)
        {
            return this.DbHandler(db => db.HashDelete(key, field));
        }

        public long HashDelete(string key, IEnumerable<string> fields)
        {
            var hashFields = from f in fields
                             select (RedisValue)f;
            return this.DbHandler<long>(db => db.HashDelete(key, hashFields.ToArray()));
        }
    }
}
