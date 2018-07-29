using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
   public class AdminUserEntity:BaseEntity
    {
        public string Name { get; set; }
        public string PhoneNum { get; set; }
        /// <summary>
        /// 密码哈希值
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// 密码验值
        /// </summary>
        public string PasswordSalt { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public long? CityId { get; set; }
        public virtual CityEntity City { get; set; }
        public int LoginErrorTimes { get; set; }
        public DateTime? LastLoginErrorDateTime { get; set; }
        public virtual ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
    }
}
