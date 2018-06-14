using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Framework
{
    /// <summary>
    /// 常用正则表达式
    /// </summary>
    public class RegularConstants
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public const string Mobile = @"^((13[0-9])|(15[^4,\D])|(18[0-9])|(14[57])|(17[013678]))\d{8}$";

        /// <summary>
        /// 日期
        /// </summary>
        public const string Date = @"^\d{4}-\d{1,2}-\d{1,2}";

        /// <summary>
        /// 国内电话号码
        /// </summary>
        public const string Telephone = @"^(0\\d{2}-\\d{8}(-\\d{1,4})?)|(0\\d{3}-\\d{7,8}(-\\d{1,4})?)$";

        /// <summary>
        /// 身份证号码
        /// </summary>
        public const string CardNo = @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$|^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|[x|X])$";

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public const string Email = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";


        public static bool IsMatch(string regular, string str)
        {
            Regex regex = new Regex(regular);
            return regex.IsMatch(str);
        }
    }
}
