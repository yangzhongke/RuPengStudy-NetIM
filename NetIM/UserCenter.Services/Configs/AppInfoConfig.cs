using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Services.Models;

namespace UserCenter.Services.Configs
{
    public class AppInfoConfig : EntityTypeConfiguration<AppInfo>
    {
        public AppInfoConfig()
        {
            ToTable("T_AppInfos");
            Property(e => e.Name).HasMaxLength(100).IsRequired();
            Property(e => e.AppKey).HasMaxLength(100).IsRequired();
            Property(e => e.AppSecret).HasMaxLength(100).IsRequired();
        }
    }
}
