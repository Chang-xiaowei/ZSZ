using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.DTO;
using ZSZ.Service.Entities;
using ZSZIService;
using System.Data.Entity;

namespace ZSZ.Service
{
    class AdminUserService : IAdminUserService
    {
        public long AddAdminUser(string name, string phoneNum, string password, string email, long? cityId)
        {
            AdminUserEntity adminAdd = new AdminUserEntity();
            adminAdd.Name = name;
            adminAdd.PhoneNum = phoneNum;
            adminAdd.Email = email;
            adminAdd.CityId = cityId;
            //Passwordhash=md5(salt+用户输入密码）           
            string salt = CommonHelper.GenerateCaptchaCode(4);//盐值
            adminAdd.PasswordSalt = salt;
            adminAdd.PasswordHash = CommonHelper.CalMD5(salt + password);
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                bool exsits = bs.GetAll().Any(u => u.PhoneNum == phoneNum);
                if (exsits)
                {
                    throw new ArgumentException("手机号已存在！");
                }
                ctx.AdminUsers.Add(adminAdd);
                ctx.SaveChanges();
                return adminAdd.Id;
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var user = bs.GetAll().SingleOrDefault(u => u.PhoneNum==phoneNum);
                if (user==null)
                {
                    return false;
                }
                string dbHash = user.PasswordHash;
                string hash =CommonHelper.CalMD5(user.PasswordSalt + password);
                return hash ==dbHash;              
            }
        }
        public AdminUserDTO[] GetAll()
        {
            /* using (ZSZDbContext ctx=new ZSZDbContext())
             {
                 BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                 List<AdminUserDTO> list = new List<AdminUserDTO>();
                 AdminUserEntity adminUser = new AdminUserEntity();
                 foreach (var admin in bs.GetAll())
                 {
                     AdminUserDTO dto = new AdminUserDTO();
                     dto.Email = admin.Email;
                     dto.Name = admin.Name;
                     dto.PhoneNum = admin.PhoneNum;
                     dto.LastLoginErrorDateTime = admin.LastLoginErrorDateTime;
                     dto.LoginErrorTimes = admin.LoginErrorTimes;
                     if (admin.City.Name!=null)
                     {
                         dto.CityName = admin.City.Name;
                     }
                     else
                     {
                         dto.CityName = "总部";
                     }
                     dto.Id = admin.Id;
                     dto.CreateDateTime = admin.CreateDateTime;
                     list.Add(dto);
                 }
                 return list.ToArray();
             }
             */
            using (ZSZDbContext ctx=new Service.ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                return bs.GetAll().Include(c => c.City).AsNoTracking().ToList().Select(a => ToDTO(a)).ToArray();
            }
        }

        private AdminUserDTO ToDTO(AdminUserEntity user)
        {
            AdminUserDTO dto = new AdminUserDTO();
            dto.CityId = user.CityId;
            if(user.CityId!=null)
            {
                dto.CityName = user.City.Name;
            }
            else
            {
                dto.CityName = "总部";
            }
            dto.CreateDateTime = user.CreateDateTime;
            dto.Email = user.Email;
            dto.Id = user.Id;
            dto.LastLoginErrorDateTime = user.LastLoginErrorDateTime;
            dto.LoginErrorTimes = user.LoginErrorTimes;
            dto.Name = user.Name;
            dto.PhoneNum = user.PhoneNum;
            return dto;
        }
        public AdminUserDTO[] GetAll(long? cityId)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var all = bs.GetAll().Include(c => c.City).AsNoTracking().Where(c => c.CityId == cityId);
                return all.ToList().Select(a => ToDTO(a)).ToArray();
            }
        }
        public AdminUserDTO GetById(long id)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                //这里不能用bs.GetById(id);因为无法Include、AsNoTracking()等
                var user= bs.GetAll().Include(u=>u.City).AsNoTracking().SingleOrDefault(a=>a.Id==id);
                if (user==null)
                {
                    return null;
                }
                return ToDTO(user);
            }
        }

        public AdminUserDTO GetByPhoneNum(string phoneNum)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var user = bs.GetAll().Include(u => u.City).AsNoTracking().Where(a => a.PhoneNum== phoneNum);
                int count = user.Count();
                if (count<=0)
                {
                    return null;
                }
                else if (count==1)
                {
                    return ToDTO(user.Single());
                }
                else
                {
                    throw new ArgumentException("找到多个手机号为" + phoneNum + "的管理员");
                }                
            }
        }

        public bool HasPermission(long adminUserId, string permissionName)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var user = bs.GetAll().Include(a => a.Roles).AsNoTracking().SingleOrDefault(a => a.Id == adminUserId);
                if (user==null)
                {
                    throw new ArgumentException("找不到id=" + adminUserId + "的用户");
                }
                return user.Roles.SelectMany(r => r.Permissions).Any(p => p.Name == permissionName);
            }
        }
        public void MarkDeleted(long adminUserId)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<AdminUserEntity>bs = new BaseService<AdminUserEntity>(ctx);
                bs.MarkDeleted(adminUserId);
            }
        }
        public void RecordLoginError(long id)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var user = bs.GetById(id);
                if (user==null)
                {
                    throw new ArgumentException("用户不存在"+id);
                }
                user.LoginErrorTimes++;
                user.LastLoginErrorDateTime = DateTime.Now;
                ctx.SaveChanges();
            }
        }

        public void ResetLoginError(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var user = GetById(id);
                if (user == null)
                {
                    throw new ArgumentException("用户不存在" + id);
                }
                user.LoginErrorTimes=0;
                user.LastLoginErrorDateTime = null;
                ctx.SaveChanges();
            }
        }
        public void UpdateAdminUser(long id, string name, string phoneNum,
            string password, string email,long?cityId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                var user = bs.GetById(id);
                if (user==null)
                {
                    throw new ArgumentException("找不到Id为" + id + "的管理员");
                }
                user.Name = name;
                user.PhoneNum = phoneNum;
                user.Email = email;
                if (!string.IsNullOrEmpty(password))
                {
                    string salt = user.PasswordSalt;
                    //注意：MD5加密前后字符串必须一致（ salt + password）顺序不能颠倒
                    user.PasswordHash = CommonHelper.CalMD5( salt+ password );
                }
                user.CityId = cityId;
                ctx.SaveChanges();
            }
        }
    }
}
