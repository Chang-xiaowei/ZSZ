using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class AdminUserConfig:EntityTypeConfiguration<AdminUserEntity>
    {
        public AdminUserConfig()
        {
            this.ToTable("T_AdminUsers");
            HasOptional(u => u.City).WithMany().HasForeignKey(u => u.CityId).WillCascadeOnDelete(false);
            HasMany(u => u.Roles).WithMany(r => r.AdminUsers).Map(m => m.ToTable("T_AdminUserRoles").MapLeftKey("AdminUserId").MapRightKey("RoleId"));
            Property(u => u.Name).IsRequired().HasMaxLength(50);
            Property(u => u.Email).IsRequired().HasMaxLength(30).IsUnicode(false);
            Property(u => u.PhoneNum).IsRequired().HasMaxLength(20).IsUnicode(false);
            Property(u => u.PasswordHash).IsRequired().HasMaxLength(100).IsUnicode(false);
            Property(u => u.PasswordSalt).IsRequired().HasMaxLength(50).IsUnicode(false);
        }
    }
}
