using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Ado
{
   public class DbUtility
    {
        public static DbConnection CreateConnection(string providerName,string strConn)
        {
            //string providerName = ConfigurationManager.ConnectionStrings["DbConnection"].ProviderName;
            //string strConn = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
            DbConnection conn = factory.CreateConnection();
            conn.ConnectionString = strConn;
            return conn;
        }
    }
}
