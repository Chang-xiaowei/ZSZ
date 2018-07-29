using IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Test : ITest
    {
        public void Test1()
        {
            Console.WriteLine("你好23");
        }
    }
}
