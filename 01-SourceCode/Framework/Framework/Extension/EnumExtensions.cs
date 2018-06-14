using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 读取枚举类型描述信息
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="item">枚举对象</param>
        /// <returns></returns>
        public static string GetDescribeInfo<T>(this T item)
        {
            var a = ReflectionMethod.GetFieldInfoAttribute<DescriptionAttribute>(item);
            return a == null ? string.Empty : a.Description;
        }
        /// <summary>
        /// 读取枚举类型描述信息
        /// </summary>
        /// <typeparam name="K">要创建的枚举类型</typeparam>
        /// <param name="item">枚举值</param>
        /// <returns></returns>
        public static string GetDescribeInfo<K>(this int item)
        {
            var t = typeof(K);
            if (t.IsEnum)
            {
                var a = (K)System.Enum.Parse(t, item.ToString());
                return EnumExtensions.GetDescribeInfo(a);
            }
            else
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// 创建枚举
        /// </summary>
        /// <typeparam name="K">要创建的枚举类型</typeparam>
        /// <param name="item">枚举值</param>
        /// <returns></returns>
        public static K GetEnum<K>(this int item)
        {
            var t = typeof(K);
            if (t.IsEnum)
            {
                return (K)System.Enum.Parse(t, item.ToString());

            }
            else
            {
                return default(K);
            }

        }

        /// <summary>
        /// 获取该枚举的所有描述的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<T, string> GetEnumDescriptionList<T>()
        {
            var description = new Dictionary<T, string>();
            var fis = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);
            if (fis != null && fis.Length > 0)
            {
                foreach (var item in fis)
                {
                    var attributes =
                   (DescriptionAttribute[])item.GetCustomAttributes(
                   typeof(DescriptionAttribute),
                   false);
                    var value = item.GetValue(null);
                    if (attributes.Length > 0)
                        description.Add((T)value, attributes[0].Description);
                }
            }
            return description;
        }


    }
}
