using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace suo
{
   public  class GirlConfig:EntityTypeConfiguration<Girl>
    {
        public GirlConfig()
        {
            this.ToTable("T_Girls");
            Property(g => g.rowVer).IsRowVersion();
        }
    }
}
