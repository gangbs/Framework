using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
   //public interface IObjectMap<TSource, TTarget>
   // {
   //     TTarget MapTo<TSource, TTarget>(TSource source);

   //     IEnumerable<TTarget> MapTo<TSource, TTarget>(IEnumerable<TSource> source);
   // }


    public class ObjectMap<TSource, TTarget> where TTarget:class,new() //: IObjectMap<TSource, TTarget>
    {
        public static TTarget MapTo(TSource source)
        {
            return MapFun(source);
        }

        public static IEnumerable<TTarget> MapTo(IEnumerable<TSource> source)
        {
            if (source == null) return null;
            var lst = from item in source
                      select MapFun(item);
            return lst;
        }

        private static Func<TSource, TTarget> CreateExpression()//params string[] excludeMembers
        {
            var parameter = Expression.Parameter(typeof(TSource), "p");
            List<MemberAssignment> lstBindMember = new List<MemberAssignment>();


            foreach (var m in typeof(TTarget).GetProperties())
            {
                if (!m.CanWrite) continue;
                var memberInSource = typeof(TSource).GetProperty(m.Name);
                if (memberInSource == null) continue;

                var MemberExp = Expression.Property(parameter, memberInSource);
                var ma = Expression.Bind(m, MemberExp);
                lstBindMember.Add(ma);
            }

            var newExp = Expression.New(typeof(TTarget));

            var memberInitExp = Expression.MemberInit(newExp, lstBindMember);

            var exp = Expression.Lambda<Func<TSource, TTarget>>(memberInitExp, parameter);
            return exp.Compile();
        }

        private static bool IsMemberNeedMap(PropertyInfo m, params string[] excludeMembers)
        {
            if (!m.CanWrite)
            {
                return false;
            }

            if(excludeMembers!=null&&excludeMembers.Contains(m.Name))
            {
                return false;
            }
            var memberInSource = typeof(TSource).GetProperty(m.Name);
            if (memberInSource == null)
            {
                return false;
            }


            return true;
        }

        private static Func<TSource, TTarget> MapFun = CreateExpression();
    }

    public static class ClassExtensions
    {
        public static T InEntityFrom<T, K>(this T target, K source) where T : class where K : class
        {
            var pros = typeof(T).GetProperties();
            var spros = typeof(K).GetProperties();
            var dic = (from t in spros select new KeyValuePair<string, object>(t.Name, t.GetValue(source, null))).ToDictionary(m => m.Key, m => m.Value);

            foreach (var p in pros)
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
