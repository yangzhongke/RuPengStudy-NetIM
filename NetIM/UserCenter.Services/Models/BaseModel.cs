using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCenter.Services.Models
{
    public abstract class BaseModel
    {
        public long Id { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
