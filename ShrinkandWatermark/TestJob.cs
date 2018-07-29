using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShrinkandWatermark
{
    class TestJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("该吃饭了");
            Console.ReadKey();
        }
    }
}
