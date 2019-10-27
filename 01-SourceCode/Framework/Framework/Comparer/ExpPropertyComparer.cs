using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public class ExpPropertyComparer<T> : IEqualityComparer<T>
    {
        private Func<T, Object> propertyValueFunc = null;

        public ExpPropertyComparer(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentException("参数不可为空！");ISet
            }
            this.propertyValueFunc = GenFun(propertyInfo);
        }

        public ExpPropertyComparer(string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);

            if (propertyInfo == null)
            {
                throw new ArgumentException(string.Format("{0} is not a property of type {1}.", propertyName, typeof(T)));
            }
            this.propertyValueFunc = GenFun(propertyInfo);
        }


        private Func<T, object> GenFun(PropertyInfo propertyInfo)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "obj");
            MemberExpression memberExp = Expression.Property(parameter, propertyInfo);
            var exp = Expression.Lambda<Func<T, object>>(memberExp, parameter);
            return exp.Compile();
        }

        public bool Equals(T x, T y)
        {
            object xValue = propertyValueFunc(x);
            object yValue = propertyValueFunc(y);

            if (xValue == null)
                return yValue == null;

            return xValue.Equals(yValue);
        }

        public int GetHashCode(T obj)
        {
            object propertyValue = propertyValueFunc(obj);

            if (propertyValue == null)
                return 0;
            else
                return propertyValue.GetHashCode();
        }
    }
}
