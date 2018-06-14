using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Framework
{
    public class TokenProtect
    {
        public static string Encryption(AuthenticationTicket ticket)
        {
            byte[] bt = new TicketSerializer().Serialize(ticket);//票证序列化器，即将AuthenticationTicket对象序列化成二进制数据
            byte[] sBt = MachineKey.Protect(bt);//将二进制数据加密
            string token = TextEncodings.Base64Url.Encode(sBt);//转化成base64url字符串（为什么不是base64）
            return token;
        }

        public static AuthenticationTicket Decrypt(string token)
        {
            byte[] sBt = TextEncodings.Base64Url.Decode(token);
            byte[] bt = MachineKey.Unprotect(sBt);//将二进制数据加密
            AuthenticationTicket ticket = new TicketSerializer().Deserialize(bt);
            return ticket;
        }

        public static bool TryEncryption(AuthenticationTicket ticket, out string token)
        {
            bool flag = false;
            try
            {
                token = Encryption(ticket);
                flag = true;
            }
            catch
            {
                token = null;
            }
            return flag;
        }

        public static bool TryDecrypt(string token, out AuthenticationTicket ticket)
        {
            bool flag = false;
            try
            {
                ticket = Decrypt(token);
                flag = true;
            }
            catch
            {
                ticket = null;
            }
            return flag;
        }
    }
}
