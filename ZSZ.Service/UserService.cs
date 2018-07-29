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
    public class UserService : IUserService
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public long AddNew(string phoneNum, string password)
        {
            using (ZSZDbContext ctx = new Service.ZSZDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                bool exsits = bs.GetAll().Any(u => u.PhoneNum == phoneNum);
                if (exsits)
                {
                    throw new ArgumentException("手机号已存在");
                }
                UserEntity userEntity = new UserEntity();
                userEntity.PhoneNum = phoneNum;
                string salt = Common.CommonHelper.GenerateCaptchaCode(5);
                userEntity.PasswordSalt = salt;
                userEntity.PasswordHash = Common.CommonHelper.CalMD5(salt+password);
                userEntity.LoginErrorTimes = 0;                            
                ctx.Users.Add(userEntity);
                ctx.SaveChanges();
                return userEntity.Id;                 
            }
           
        }
        /// <summary>
        /// 价差登录状态
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckLogin(string phoneNum, string password)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetAll().SingleOrDefault(u => u.PhoneNum == phoneNum);
                if (user==null)
                {
                    return false;
                }
                else
                {
                    string dbPwdHash = user.PasswordHash;
                    string salt = user.PasswordSalt;
                    string userPwdHash = Common.CommonHelper.CalMD5(salt + password);
                    return dbPwdHash == userPwdHash;
                }
            }
        }

        public UserDTO GetById(long id)
        {
            using (ZSZDbContext ctx=new ZSZDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(id);
                return user == null ? null : ToDTO(user);
            }
        }

        private UserDTO ToDTO(UserEntity user)
        {
            UserDTO dto = new UserDTO();
            dto.PhoneSum = user.PhoneNum;
            dto.CityId = user.CityId;
            dto.LoginErrorTimes = user.LoginErrorTimes;
            dto.LastLoginErrorDateTime = user.LastLoginErrorDateTime;
            dto.Id = user.Id;
            dto.CreateDateTime = user.CreateDateTime;
            return dto;
        }

        public UserDTO GetByPhoneNum(string phoneNum)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetAll().SingleOrDefault(u=>u.PhoneNum==phoneNum);
                return user == null ? null : ToDTO(user);
            }
        }
        /// <summary>
        /// 记录用户登录次数
        /// </summary>
        /// <param name="id"></param>
        public long?  IncrLoginError(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(id);
                if (user==null)
                {
                  throw new ArgumentException("用户不存在" + id);
                }
                user.LoginErrorTimes++;
                user.LastLoginErrorDateTime = DateTime.Now;
                ctx.SaveChanges();
                return user.LoginErrorTimes;              
            }
        }
        public bool IsLocked(long id)
        {
            var user = GetById(id);
            // 错误登录次数 >= 5，最后一次登录错误时间在30分钟之内
            if (user==null)
            {
                throw new ArgumentException("用户不存在" + id);
            }
            return (user.LoginErrorTimes >= 5 && user.LastLoginErrorDateTime > DateTime.Now.AddMinutes(-30));
        }

        public void ResetLoginError(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(id);
                if (user == null)
                {
                    throw new ArgumentException("用户不存在" + id);
                }
                user.LoginErrorTimes=0;
                user.LastLoginErrorDateTime = null;
                ctx.SaveChanges();

            }
        }

        public void SetUserCityId(long userId, long cityId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("用户id不存在" + userId);
                }
                user.CityId = cityId;
                ctx.SaveChanges();
            }
        }
        /// <summary>
        /// 更新用户的密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        public void UpdatePwd(long userId, string newPassword)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<UserEntity> bs = new BaseService<UserEntity>(ctx);
                var user = bs.GetById(userId);
                if (user == null)
                {
                    throw new ArgumentException("用户id不存在" + userId);
                }
                string salt = user.PasswordSalt;
                string hash = Common.CommonHelper.CalMD5(salt + newPassword);
                user.PasswordHash = hash;
                ctx.SaveChanges();
            }
        }
    }
}
