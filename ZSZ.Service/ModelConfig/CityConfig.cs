using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class CityConfig:EntityTypeConfiguration<CityEntity>
    {
        public CityConfig()
        {
            this.ToTable("T_Cities");
            Property(c => c.Name).IsRequired().HasMaxLength(20);
        }
    }
}
