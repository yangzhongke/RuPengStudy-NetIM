using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.Services.Models;

namespace UserCenter.Services.Configs
{
    public class UserConfig:EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            ToTable("T_Users");
            this.HasMany(e => e.Groups).WithMany(e=>e.Users)
                .Map(e=>e.ToTable("T_GroupUsers").MapLeftKey("UserId").MapRightKey("GrouId"));
        }
    }
}
