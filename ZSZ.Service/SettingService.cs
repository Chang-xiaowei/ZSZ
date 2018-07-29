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
    class SettingService:ISettingService
    {
        public SettingDTO[] GetAll()
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
               BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
               return bs.GetAll().Select(s => new SettingDTO() {
                    Id = s.Id,
                    Name=s.Name,
                    CreateDateTime=s.CreateDateTime,
                    Value=s.Value
                }).ToArray();
                /*
                List<SettingDTO> list = new List<SettingDTO>();
                foreach (var setting in bs.GetAll())
                {
                    SettingDTO dto = new SettingDTO();
                    dto.CreateDateTime = setting.CreateDateTime;
                    dto.Id = setting.Id;
                    dto.Name = setting.Name;
                    dto.Value = setting.Name;
                    list.Add(dto);
                }
                return list.ToArray();*/
            }            
        }
        public bool? GetBoolValue(string name)
        {
            return Convert.ToBoolean(GetValue(name));
        }
        public int? GetIntValue(string name)
        {
            return Convert.ToInt32(GetValue(name));
        }
        public string GetValue(string name)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                var setting = bs.GetAll().Where(s => s.Name == name).SingleOrDefault();
                if (setting == null)
                {
                    return null;
                    //throw new ArgumentException("配置项Name不存在");
                }
                else
                {
                    return setting.Value;
                }
                
            }
        }
        public void SetBoolValue(string name, bool value)
        {
            SetValue(name, value.ToString());
        }
        public void SetIntValue(string name, int value)
        {
            SetValue(name, value.ToString());
        }
        public void SetValue(string name, string value)
        {
            using (ZSZDbContext ctx = new Service.ZSZDbContext())
            {
                BaseService<SettingEntity> bs= new Service.BaseService<SettingEntity>(ctx);
                /* 
               bool exsits = GetAll().Any(s => s.Name == name);    
               SettingEntity setting = new Entities.SettingEntity();
                 if (exsits)
                 {
                     setting.Value = value;
                 }
                 else
                 {
                     setting.Name = name;
                     setting.Value = value;
                 }
                 ctx.SaveChanges();
                 */
                var setting = bs.GetAll().SingleOrDefault(s => s.Name == name);
                if (setting != null)
                {
                    ctx.Settings.Add(new SettingEntity() { Value = value });
                }
                else
                {
                    ctx.Settings.Add(new SettingEntity {Value=value,Name=name });
                }
                ctx.SaveChanges();
            }
        }
    }
}
