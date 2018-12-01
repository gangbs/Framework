using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
   public abstract class ExcelTableRead<T> where T : class, new()
    {
        private Dictionary<int, PropertyInfo> _colMap;
        protected abstract int _rowBegin { get; }
        /// <summary>
        /// 若值为-1则为NPOI认为的最后一行
        /// </summary>
        protected virtual int _rowEnd => -1;


        protected abstract Dictionary<int, PropertyInfo> DefineColumnMap();

        protected virtual IWorkbook ReadWorkbook(byte[] file)
        {
            Stream s = new MemoryStream(file);
            IWorkbook workbook = WorkbookFactory.Create(s);
            return workbook;
        }

        protected virtual ISheet ReadSheet(IWorkbook workbook)
        {
           return workbook.GetSheetAt(0);
        }

        protected virtual List<T> ReadTable(ISheet sheet,int rowBegin, int rowEnd, Dictionary<int, PropertyInfo> colMap)
        {
            List<T> lst = new List<T>();
            for (int i=rowBegin;i<=rowEnd;i++)
            {
                var row = sheet.GetRow(i);
                var obj = ReadRow(row, colMap);
                lst.Add(obj);
            }
            return lst;
        }

        protected virtual T ReadRow(IRow row,Dictionary<int,PropertyInfo> colMap)
        {
            var obj = (T)Activator.CreateInstance(typeof(T));
            foreach (var kv in colMap)
            {
                int colIndex = kv.Key;
                var property = kv.Value;
                var cell = row.GetCell(colIndex);
                
                try
                {
                    ReadCell(cell, property, ref obj);
                }
                catch(Exception exp)
                {
                    string errMsg = $"第{cell.RowIndex+1}行，第{cell.ColumnIndex + 1}列数据导入格式转化出错，错误详情：{exp.Message}";
                    throw new Exception(errMsg);
                }              
            }
            return obj;
        }

        protected virtual void ReadCell(ICell cell,PropertyInfo property,ref T obj)
        {
            if (property.PropertyType == typeof(DateTime) && cell.CellType == CellType.Numeric)
            {
                property.SetValue(obj, cell.DateCellValue);
            }
            else
            {
                var val = Convert.ChangeType(cell?.ToString(), property.PropertyType);//值需实现IConvertible
                property.SetValue(obj, val);
            }
        }

        public List<T> Parse(byte[] file)
        {
            this._colMap = DefineColumnMap();
            IWorkbook workbook = this.ReadWorkbook(file);
            ISheet sheet = this.ReadSheet(workbook);

            int rowEnd = this._rowEnd == -1 ? sheet.LastRowNum : this._rowEnd;
            List<T> lst = this.ReadTable(sheet, this._rowBegin, rowEnd, this._colMap);
            return lst;
        }


    }
}
