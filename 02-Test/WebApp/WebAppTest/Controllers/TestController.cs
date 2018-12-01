using Framework;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebAppTest.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public JsonResult Upload(FileUpdateModel model)
        {
            var file = model.excel;
            byte[] bytes = new byte[file.InputStream.Length];
            file.InputStream.Seek(0, System.IO.SeekOrigin.Begin);
            file.InputStream.Read(bytes, 0, bytes.Length);

            try
            {
                var table1 = new Table1Read();
                var lst = table1.Parse(bytes);
            }
            catch(Exception exp)
            {

            }



            return null;
        }


    }

    public class FileUpdateModel
    {
        public HttpPostedFileBase excel { get; set; }
    }

    public class ExcelTable
    {
        public int No { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birth { get; set; }
        public string Address { get; set; }
        public double Height { get; set; }
        public string Remark { get; set; }
    }

    public class Table1Read : ExcelTableRead<ExcelTable>
    {

        protected override int _rowBegin => 4;

        protected override Dictionary<int, PropertyInfo> DefineColumnMap()
        {
            var map = new Dictionary<int, PropertyInfo>();
            map.Add(3, typeof(ExcelTable).GetProperty("No"));
            map.Add(4, typeof(ExcelTable).GetProperty("Name"));
            map.Add(5, typeof(ExcelTable).GetProperty("Age"));
            map.Add(6, typeof(ExcelTable).GetProperty("Birth"));
            map.Add(7, typeof(ExcelTable).GetProperty("Address"));
            map.Add(8, typeof(ExcelTable).GetProperty("Height"));
            map.Add(9, typeof(ExcelTable).GetProperty("Remark"));
            return map;
        }

    }

}