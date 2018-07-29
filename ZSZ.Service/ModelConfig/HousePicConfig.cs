using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class HousePicConfig:EntityTypeConfiguration<HousePicEntity>
    {
        public HousePicConfig()
        {
            this.ToTable("T_HousePics");
            HasRequired(h => h.House).WithMany().HasForeignKey(h => h.HouseId).WillCascadeOnDelete(false);
            Property(p => p.Url).IsRequired().HasMaxLength(1024);
            Property(p => p.ThumbUrl).IsRequired().HasMaxLength(1024);
        }
    }
}
