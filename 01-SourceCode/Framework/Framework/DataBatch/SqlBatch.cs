using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Framework.Extension;

namespace Framework.DataBatch
{
    public class SqlBatch<T> : IDatabaseBatch<T>
    {
        readonly string _connStr = "";
        readonly string _tableName = "";

        public SqlBatch(string connStr, string tableName)
        {
            this._connStr = connStr;
            this._tableName = tableName;
        }

        public string TableName => nameof(T);

        public DbConnection DbConn
        {
            get
            {
                string providerName = ConfigurationManager.ConnectionStrings["DbConnection"].ProviderName;
                string connStr= ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                var conn = factory.CreateConnection();
                conn.ConnectionString = connStr;
                return conn;
            }
        }

        /// <summary>
        /// 利用sqlserver的bcp功能批量插入，用到了SqlBulkCopy类
        /// </summary>
        /// <param name="lstData"></param>
        /// <returns></returns>
        public int BatchInsert(List<T> lstData)
        {
            var dt = lstData.ListToTable();
            using (SqlBulkCopy sbc = new SqlBulkCopy(this._connStr))
            {
                sbc.BatchSize = dt.Rows.Count;
                sbc.BulkCopyTimeout = 10;
                sbc.DestinationTableName = this._tableName;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sbc.ColumnMappings.Add(dt.Columns[i].ColumnName, i);
                }
                //全部写入数据库
                sbc.WriteToServer(dt);
            }
            return lstData.Count();
        }


    }
}
