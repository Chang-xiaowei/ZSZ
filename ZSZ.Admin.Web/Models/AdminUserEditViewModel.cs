using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.Admin.Web.Models
{
    public class AdminUserEditViewModel
    {
        public AdminUserDTO AdminUser { get; set; }
        public CityDTO[] Cities { get; set; }
        public RoleDTO[] AdminRoles { get; set; }
        public long? CityId { get; set; }
        public RoleDTO[] AllRoles { get; set; }
        /// <summary>
        /// 当前用户拥有的角色id
        /// </summary>
        public long[] UserRoleIds { get; set; }
    }
}