using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDemo;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PersonController : Controller
    {
       [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(PersonModel model)
        {
            
            string  name = model.Name;
            int age = model.Age;
            int height = model.Height;
            int weight = model.Weight;
           // string idkey = model.Idkey;
            SqlParameter[] pms = new SqlParameter[] {
                new SqlParameter("@name",SqlDbType.VarChar) { Value=name},
                new SqlParameter("@age",SqlDbType.Int) { Value=age},
                new SqlParameter("@height",SqlDbType.Int) { Value=height},
                new SqlParameter("@weight",SqlDbType.Int) { Value=weight}
               // new SqlParameter("@idkey",SqlDbType.VarChar) { Value=idkey}

            };
            string sql = "insert into person(name,age,height,weight) values(@name,@age,@height,@weight)";
            //string sql = "delete from person where name=@name";
            int r=  SqlHelper.ExecuteNonQuery(sql,pms);
            if (r>0)
            {
                return Json(new { status = "ok" });
            }           
            {
                return Json(new {Error="插入失败" });
            }
           
        }
        public ActionResult Find()
        {
            string sql = "select * from person";
            List<PersonModel> list = new List<Models.PersonModel>();
           
            var reads= SqlHelper.ExecuteReader(sql);
            while (reads.Read())
            {
                PersonModel model = new PersonModel();
                model.Id =Convert.ToInt32(reads[0]);
                model.Name = reads[1].ToString();
                model.Age =Convert.ToInt32(reads[2]);
                model.Height = Convert.ToInt32(reads[3]);
                model.Idkey = reads[4].ToString();
                list.Add(model);                       
            }
            ViewBag.data = list;//第一种
            return View(list);//第二种
        }

    }
        
}