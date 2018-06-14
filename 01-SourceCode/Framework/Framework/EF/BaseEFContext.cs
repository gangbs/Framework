using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EF
{
   public class BaseEFContext : DbContext
    {
        public BaseEFContext() : base("DbConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
