using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public interface IEmailSend
    {
        string FromAddress { get; }
        List<string> ToAddress { get; }

        string Subject { get; }
        string Content { get; }

        bool Send();
    }
}
