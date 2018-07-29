using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZSZ.Front.Web.Modals
{
    public class UserFogetPwdModel
    {
        public string PhoneNum { get; set; }
        public string  VerifyCode { get; set; }
        public string  NewPassword { get; set; }
    }
}