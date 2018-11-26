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
                PropertyInfo memberInSource = null;
                if (!IsMemberNeedMap(m,out memberInSource, null)) continue;

                var MemberExp = Expression.Property(parameter, memberInSource);
                var ma = Expression.Bind(m, MemberExp);
                lstBindMember.Add(ma);
            }

            var newExp = Expression.New(typeof(TTarget));

            var memberInitExp = Expression.MemberInit(newExp, lstBindMember);

            var exp = Expression.Lambda<Func<TSource, TTarget>>(memberInitExp, parameter);
            return exp.Compile();
        }

        private static bool IsMemberNeedMap(PropertyInfo m,out PropertyInfo memberInSource, params string[] excludeMembers)
        {
            memberInSource = null;
            if (!m.CanWrite)
            {
                return false;
            }

            if(excludeMembers!=null&&excludeMembers.Contains(m.Name))
            {
                return false;
            }
            memberInSource = typeof(TSource).GetProperty(m.Name);
            if (memberInSource == null)
            {
                return false;
            }

            return true;
        }

        private static Func<TSource, TTarget> MapFun = CreateExpression();
    }
}
