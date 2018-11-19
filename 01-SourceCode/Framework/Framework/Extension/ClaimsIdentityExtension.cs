using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class ClaimsIdentityExtension
    {
        public static T ToUserInfo<T>(this ClaimsIdentity claimsIdentity) where T : new()
        {
            Type tp = typeof(T);
            var constructor = tp.GetConstructor(Type.EmptyTypes);
            T user = (T)constructor.Invoke(new Object[0]);

            foreach (var item in tp.GetProperties())
            {
                var claim = claimsIdentity.FindFirst(item.Name);
                if (claim == null) continue;
                item.SetValue(user, Convert.ChangeType(claim.Value, item.PropertyType));
            }
            return user;
        }
    }
}
