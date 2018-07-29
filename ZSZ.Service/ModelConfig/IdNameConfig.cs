using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class IdNameConfig: EntityTypeConfiguration<IdNameEntity>
    {
        public IdNameConfig()
        {
            this.ToTable("T_IdNames");
            Property(p => p.Name).IsRequired().HasMaxLength(1024);
            Property(p => p.TypeName).IsRequired().HasMaxLength(1024);
        }
    }
}
