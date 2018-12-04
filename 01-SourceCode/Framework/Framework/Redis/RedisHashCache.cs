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
            var val = this.DoSave(db => db.HashGet(key, field));
            return val;
        }

        public T HashGet<T>(string key,string field)
        {
            key = this.GenRealKey(key);
            var val = this.DoSave(db => db.HashGet(key,field));
            return this.ConvertObj<T>(val);
        }

        public Dictionary<string,object> All(string key)
        {
            var dic = new Dictionary<string, object>();
            key = this.GenRealKey(key);
            var fields = this.DoSave(db => db.HashGetAll(key));
            foreach(var field in fields)
            {
                dic.Add(field.Name,field.Value);
            }
            return dic;
        }

        public List<string> AllField(string key)
        {
            key = this.GenRealKey(key);
            var fields = this.DoSave(db => db.HashKeys(key));
            var lstField = (from f in fields
                            select f.ToString()).ToList();
            return lstField;
        }

        public long FieldCount(string key)
        {
            key = this.GenRealKey(key);
            return this.DoSave(db => db.HashLength(key));
        }

        public bool HashSet(string key,string field,object val)
        {
            key = this.GenRealKey(key);
            return this.DoSave(db => db.HashSet(key,field, (RedisValue)val));
        }

        public void HashSet(string key, Dictionary<string,object> dic)
        {
            List<HashEntry> lst = new List<HashEntry>();

            foreach(var kv in dic)
            {
                lst.Add(new HashEntry(kv.Key, (RedisValue)kv.Value));
            }

            var db = this.GetDatabase();
            db.HashSet(key, lst.ToArray());
        }

        public bool HashExist(string key,string field)
        {
            return this.DoSave(db => db.HashExists(key, field));
        }

        public bool HashDelete(string key, string field)
        {
            return this.DoSave(db => db.HashDelete(key, field));
        }

        public long HashDelete(string key, IEnumerable<string> fields)
        {
            var hashFields = from f in fields
                             select (RedisValue)f;
            return this.DoSave<long>(db => db.HashDelete(key, hashFields.ToArray()));
        }
    }
}
