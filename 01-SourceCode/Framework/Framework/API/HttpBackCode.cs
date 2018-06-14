using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public enum HttpBackCode
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        None = 1,
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 2,
        /// <summary>
        /// 参数错误
        /// </summary>
        [Description("参数错误")]
        Parameter = 3,
        /// <summary>
        /// HTTP请求错误
        /// </summary>
        [Description("HTTP错误")]
        Http = 4,
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 5,
        /// <summary>
        /// 系统异常
        /// </summary>
        [Description("系统异常")]
        SysError = 6,

        /// <summary>
        /// 身份验证失败
        /// </summary>
        [Description("身份验证失败")]
        UnAuthenticated = 7,

        /// <summary>
        /// 未授权
        /// </summary>
        [Description("未授权")]
        UnAuthorized = 8,
    }
}
