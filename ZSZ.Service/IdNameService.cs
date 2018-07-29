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
    public class IdNameService : IIdNameService
    {
        public long AddNew(string typeName, string name)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<IdNameEntity> bs = new BaseService<IdNameEntity>(ctx);
                var idnames = bs.GetAll().SingleOrDefault(i => i.TypeName == typeName);
                IdNameEntity idname = new IdNameEntity();
                if (idnames==null)
                {
                    idname.Name = name;
                    idname.TypeName = typeName;
                }               
               
                ctx.IdNames.Add(idname);
                ctx.SaveChanges();
                return idname.Id;
            }
        }

        public IdNameDTO[] GetAll(string typeName)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<IdNameEntity> bs = new BaseService<IdNameEntity>(ctx);
                return bs.GetAll().Where(i => i.TypeName == typeName).ToList().Select(e => ToDTO(e)).ToArray();
            }
        }

        private IdNameDTO ToDTO(IdNameEntity idname)
        {
            IdNameDTO dto = new IdNameDTO();
            dto.Name = idname.Name;
            dto.TypeName = idname.TypeName;
            dto.Id = idname.Id;
            dto.CreateDateTime = idname.CreateDateTime;
            return dto;
        }

        public IdNameDTO GetById(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<IdNameEntity> inameBs = new BaseService<IdNameEntity>(ctx);
                return ToDTO(inameBs.GetById(id));
            }
        }
    }
}
