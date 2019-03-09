using Framework.Extension;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DataBatch
{
    public class MysqlBatch<T> : IDatabaseBatch<T>
    {
        readonly string _connStr = "";
        readonly string _tableName = "";

        public MysqlBatch(string connStr, string tableName)
        {
            this._connStr = connStr;
            this._tableName = tableName;
        }

        public string TableName => throw new NotImplementedException();

        public DbConnection DbConn => throw new NotImplementedException();

        public int BatchInsert(List<T> lstData)
        {
            var dt = lstData.ListToTable();

            string path;
            dt.ToCsv("",out path);

            var columns = dt.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList();
            MySqlBulkLoader bulk = new MySqlBulkLoader((MySqlConnection)this.DbConn)
            {
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                EscapeCharacter = '"',
                LineTerminator = "\r\n",
                FileName = dt.TableName + ".csv",
                NumberOfLinesToSkip = 0,
                TableName = dt.TableName,

            };

            bulk.Columns.AddRange(columns);
            return bulk.Load();
        }
    }
}
