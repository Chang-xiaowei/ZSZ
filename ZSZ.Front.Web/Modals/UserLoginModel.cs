using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.Front.Web.Modals
{
    public class UserLoginModel
    {
        [Required]
        [Phone]
        public string  PhoneNum { get; set; }
        [Required]        
        public string  Password { get; set; }
    }
}