using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.Admin.Web.Models
{
    public class AdminUserAddViewModel
    {
        public RoleDTO[] Roles { get; set; }
        public CityDTO[] Cities { get; set; }
    }
}