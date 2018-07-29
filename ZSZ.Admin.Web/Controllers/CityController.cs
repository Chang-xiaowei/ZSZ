using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.IService;
using ZSZIService;

namespace ZSZ.Admin.Web.Controllers
{
    public class CityController : Controller
    {
        public ICityService cityService { get; set; }
        public IRegionService regionService { get; set; }
        public ActionResult List()
        {
            var city= cityService.GetAll();
            return View(city);
        }
        [HttpGet]
        public ActionResult Add()
        {            
            return View();
        }
        [HttpPost]
        public ActionResult Add(string name)
        {
            cityService.AddNew(name);
            return Json(new AjaxResult{ Status="ok" });
        }
    }
}