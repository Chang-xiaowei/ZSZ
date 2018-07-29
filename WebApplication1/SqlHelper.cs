using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    public class SqlHelper
    {
        //1.定义一个连接字符串
        private static readonly string constr = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        //2.执行增，删，改的方法
        public static int ExecuteNonQuery(string sql, params SqlParameter[] oms)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql,conn))
                {
                    if (oms != null)
                    {
                        cmd.Parameters.AddRange(oms);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        //3.执行查询返回单个值的方法
        public static object ExecuteScalar(string sql, params OleDbParameter[] oms)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (oms != null)
                    {
                        cmd.Parameters.AddRange(oms);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        //4.执行查询的方法
        public static SqlDataReader ExecuteReader(string sql,params SqlParameter[] pms)
        {
            SqlConnection conn = new SqlConnection(constr);
            using (SqlCommand cmd = new SqlCommand(sql,conn))
            {
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                try
                {
                    conn.Open();                   
                    return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
                catch
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
        }
        //5.查询数据返回DataTable
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[]oms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, constr))
            {
                adapter.SelectCommand.Parameters.AddRange(oms);
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}

  

