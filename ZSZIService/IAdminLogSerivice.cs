using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZIService
{
   public  interface IAdminLogSerivice:IServiceSupport
    {
        /// <summary>
        /// 插入一条日志：adminUserId为操作用户Id,message为消息
        /// </summary>
        /// <param name="adminUserId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        long AddNew(long adminUserId, string message);
        //ToDo，以后做日志搜索等的话就要增加新的方法
        AdminLogDTO GetById(long id);
    }
}
