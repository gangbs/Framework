using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppTest.Controllers
{
    public class FormTestController : Controller
    {
        // GET: FormTest
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Submit(FormModel model)
        {
            return null;
        }
    }

    public class FormModel
    {
        public List<int> Ids { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }
}