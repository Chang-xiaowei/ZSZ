using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service;

namespace TestDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (ZSZDbContext ctx=new ZSZDbContext())
            //{
            //    //ctx.Database.Delete();
            //    //ctx.Database.Create();
            //    //Console.WriteLine("ok");               
            //}
            string value = "ssf-fasf";
            string[] values = value.Split('-');
            Console.WriteLine(values[0]);
            foreach (var item in values)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
