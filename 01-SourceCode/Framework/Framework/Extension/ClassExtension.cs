using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
   public static class ClassExtension
    {
        public static TTarget MapTo<TSource, TTarget>(this TSource obj) //where TTarget : class, new() where TSource:class;
        {
            var sPros = typeof(TSource).GetProperties();
            var tPros = typeof(TTarget).GetProperties();

            var dic = (from p in sPros
                      select new KeyValuePair<string, object>(p.Name, p.GetValue(obj))).ToDictionary(x=>x.Key,y=>y.Value);


            var target = (TTarget)Activator.CreateInstance(typeof(TTarget)); 
            foreach (var p in tPros)
            {
                if (dic.ContainsKey(p.Name))
                {
                    p.SetValue(target, dic[p.Name], null);
                }
            }
            return target;
        }
    }
}
