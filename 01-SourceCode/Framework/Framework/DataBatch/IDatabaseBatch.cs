﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataBatch
{
   public interface IDatabaseBatch<T>
    {
        string TableName { get; }
        DbConnection DbConn { get;}
        int BatchInsert(List<T> lstData);
        //int BatchUpdate();
        //int BatchDelete();
    }
}
