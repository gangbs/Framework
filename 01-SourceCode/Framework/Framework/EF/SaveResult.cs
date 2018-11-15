using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
   public class SaveResult
    {
        public bool IsSuccess { get; set; }
        public int Rows { get; set; }
        public string Message { get; set; }
    }
}
