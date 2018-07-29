using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class UserConfig: EntityTypeConfiguration<UserEntity>
    {
        public UserConfig()
        {
            this.ToTable("T_Users");
            Property(p => p.PasswordHash).IsRequired().HasMaxLength(100);
            Property(p => p.PasswordSalt).IsRequired().HasMaxLength(20);
            Property(p => p.PhoneNum).IsRequired().HasMaxLength(20).IsUnicode(false);
        }
    }
}
