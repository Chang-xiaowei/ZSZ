using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Service.Entities;

namespace ZSZ.Service.ModelConfig
{
    class CommunityConfig: EntityTypeConfiguration<CommunityEntity>
    {
        public CommunityConfig()
        {
            this.ToTable("T_Communities");
            HasRequired(c => c.Region).WithMany().HasForeignKey(c => c.RegionId).WillCascadeOnDelete(false);
            Property(e => e.Location).HasMaxLength(1024).IsRequired();
            Property(e => e.Name).HasMaxLength(20).IsRequired();
        }
    }
}
