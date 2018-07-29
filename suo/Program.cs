using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace suo
{
    class Program
    {        
        static void Main(string[] args)
        {
            Console.WriteLine("请输入你的名字");
            string bf = Console.ReadLine();
            using (MyDbContext ctx=new MyDbContext())
            {
                ctx.Database.Log = (sql) => { Console.WriteLine(sql); };
                var g= ctx.Girls.First();
                if (!string.IsNullOrEmpty(g.BF))
                {
                    if (g.BF==bf)
                    {
                        Console.WriteLine("早已经是你的人了，还抢啥");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("来晚了"+bf+"被人抢走了");
                        return;
                    }
                }
                Console.WriteLine("点任意键开抢");
                Console.ReadKey();
                g.BF = bf;
                try
                {
                    ctx.SaveChanges();
                    Console.WriteLine("抢媳妇成功");
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("抢媳妇失败");
                }
            }
            Console.ReadKey();
        }
        static void Main4(string[] args)
        {
            Console.WriteLine("请输入你的名字");
            string myname = Console.ReadLine();
            string connstr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {                      
                        using (var selectCmd = conn.CreateCommand())
                        {
                            Console.WriteLine("开始查询");
                            selectCmd.Transaction = tx;
                            selectCmd.CommandText = "select * from T_Girls with(xlock,ROWLOCK) where id=1";
                            using (var reader = selectCmd.ExecuteReader())
                            {
                                //数据库中没有数据
                                if (!reader.Read())
                                {
                                    Console.WriteLine("没有id为1的女孩");
                                    return;
                                }
                                string bf = null;
                                if (!reader.IsDBNull(reader.GetOrdinal("BF")))
                                {
                                    bf = reader.GetString(reader.GetOrdinal("BF"));
                                }
                                if (!string.IsNullOrEmpty(bf))//已经有男票了
                                {
                                    if (bf== myname)
                                    {
                                        Console.WriteLine("早已经是我的人了");
                                    }
                                    else
                                    {
                                        Console.WriteLine("早已经被" + bf + "抢走了");
                                    }
                                    Console.ReadKey();
                                    return;
                                } 
                                //如果bf=null,则继续向下抢       
                            }
                            Console.WriteLine("查询完成,开始update");
                            using (var updateCmd = conn.CreateCommand())
                            {
                                updateCmd.Transaction = tx;
                                updateCmd.CommandText = "Update T_Girls set BF=@bf where id=1";
                                updateCmd.Parameters.Add(new SqlParameter("@bf",myname));
                                updateCmd.ExecuteNonQuery();
                            }
                            Console.WriteLine("结束Update");
                            Console.WriteLine("按任意键结束事务");
                            Console.ReadKey();
                        }
                        tx.Commit();                       
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        tx.Rollback();
                    }
                }
            }
        }
    }
}

    

