using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.Service.Entities;
using ZSZIService;
using System.Data.Entity;
namespace ZSZ.Service
{
    public class AttachmentService : IAttachmentService
    {
        public AttachmentDTO[] GetAll()
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<AttachmentEntity> bs = new Service.BaseService<Entities.AttachmentEntity>(ctx);
               return  bs.GetAll().ToList().Select(a => ToDTO(a)).ToArray();
            }
        }

        private AttachmentDTO ToDTO(AttachmentEntity a)
        {
            AttachmentDTO dto = new AttachmentDTO();
            dto.CreateDateTime = a.CreateDateTime;
            dto.Id = a.Id;
            dto.Name = a.Name;
            dto.IconName = a.IconName;
            return dto;
        }

        public AttachmentDTO[] GetAttachments(long houseId)
        {
            using (ZSZDbContext ctx = new Service.ZSZDbContext())
            {
                BaseService<HouseEntity> bs = new BaseService<HouseEntity>(ctx);
                var house = bs.GetAll().Include(h => h.Attachments).AsNoTracking().SingleOrDefault(h => h.Id == houseId);
                if (house==null)
                {
                    throw new ArgumentException("houseId" + houseId + "不存在");
                }
                return house.Attachments.ToList().Select(a => ToDTO(a)).ToArray();
            }
        }
    }
}
