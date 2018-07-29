using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZSZ.Admin.Web.Controllers
{
    public class Class1s:Controller
    {
        public ActionResult Add()
        {
            return Json(new { statis="ok"});
        }

    }
}