using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.Service.Entities;
using ZSZIService;

namespace ZSZ.Service
{
    public class AdminLogSerivice : IAdminLogSerivice
    {
        public long AddNew(long adminUserId, string message)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                //BaseService<AdminLogEntity> bs = new BaseService<AdminLogEntity>(ctx);
                /* ctx.AdminUserLogs.Add(new Entities.AdminLogEntity() {
                     Id = adminUserId,
                     Message = message
                 });
                 ctx.SaveChanges();
                 */
                AdminLogEntity log = new AdminLogEntity()
                {
                    AdminUserId = adminUserId,
                    Message = message
                };
                ctx.AdminUserLogs.Add(log);
                ctx.SaveChanges();
                return log.Id;
            }
        }
        public AdminLogDTO GetById(long id)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<AdminLogEntity> bs = new BaseService<AdminLogEntity>(ctx);
                var adminlog= bs.GetById(id);
                if (adminlog==null)
                {
                    return null;
                }
                AdminLogDTO dto = new AdminLogDTO();
                dto.AdminUserId = adminlog.AdminUserId;
                dto.AdminUserName = adminlog.AdminUser.Name;
                dto.AdminUserName = adminlog.AdminUser.PhoneNum;
                dto.CreateDateTime = adminlog.CreateDateTime;
                dto.Id = adminlog.Id;
                dto.Message = adminlog.Message;
                return dto;
            }
        }
    }
}
