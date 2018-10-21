using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataBatch
{
   public interface IDatabaseBatch<T>
    {
        int BatchInsert(List<T> lstData);
        //int BatchUpdate();
        //int BatchDelete();
    }
}
