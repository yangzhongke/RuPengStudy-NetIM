using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Services.Models;

namespace UserCenter.Services.Configs
{
    public class UserGroupConfig:EntityTypeConfiguration<UserGroup>
    {
        public UserGroupConfig()
        {
            ToTable("T_UserGroups");
            this.Property(e => e.Name).HasMaxLength(50).IsRequired();
        }
    }
}
