using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.DTO
{
    public class AppInfoDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
    }
}
