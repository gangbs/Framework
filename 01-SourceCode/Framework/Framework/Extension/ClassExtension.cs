﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
   public static class ClassExtension
    {
        public static TTarget MapTo<TSource, TTarget>(this TSource obj) where TTarget : class, new() where TSource:class
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

        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> lstSource) where TTarget : class, new() where TSource : class
        {
            if (lstSource == null) return null;

            var lstTarget = from item in lstSource
                            select item.MapTo<TSource, TTarget>();
            return lstTarget;
        }

        public static TTarget MapTo<TSource, TTarget>(this TSource obj,Action<TTarget> action) where TTarget : class, new() where TSource : class
        {
           var toObj= obj.MapTo<TSource, TTarget>();
            action(toObj);
            return toObj;
        }

        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> lstSource, Action<TTarget> action) where TTarget : class, new() where TSource : class
        {
            if (lstSource == null) return null;

            var lstTarget = from item in lstSource
                            select item.MapTo<TSource, TTarget>(action);
            return lstTarget;
        }
    }
}
