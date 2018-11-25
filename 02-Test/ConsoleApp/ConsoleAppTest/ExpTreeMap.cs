using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class ExpTreeMap
    {
        private static Dictionary<string, object> _Dic = new Dictionary<string, object>();

        private static TOut TransExp<TIn, TOut>(TIn tIn)
        {
            string key = string.Format("trans_exp_{0}_{1}", typeof(TIn).FullName, typeof(TOut).FullName);
            if (!_Dic.ContainsKey(key))
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
                List<MemberBinding> memberBindingList = new List<MemberBinding>();

                foreach (var item in typeof(TOut).GetProperties())
                {

                    if (!item.CanWrite)
                        continue;
                    MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }

                MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
                Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });
                Func<TIn, TOut> func = lambda.Compile();

                _Dic[key] = func;
            }
            return ((Func<TIn, TOut>)_Dic[key])(tIn);
        }
    }

    public static class TransExpV2<TIn, TOut>
    {

        private static readonly Func<TIn, TOut> cache = GetFunc();
        private static Func<TIn, TOut> GetFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite)
                    continue;

                //typeof(TIn).GetProperty()

                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile();
        }

        public static TOut Trans(TIn tIn)
        {
            return cache(tIn);
        }

    }



    //public class Student
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public int Age { get; set; }
    //}

    //public class StudentSecond
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public int Age { get; set; }
    //}
}
