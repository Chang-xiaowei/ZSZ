using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.Admin.Web.Models
{
    public class TestLoginModel
    {
        [Required]
        [StringLength(11,MinimumLength =6)]
        public string  PhoneNum { get; set; }
        [Required]      
        public string  Password { get; set; }
        [Required]
        [StringLength(4,MinimumLength =4)]
        public string VertifyCode { get; set; }
    }
}