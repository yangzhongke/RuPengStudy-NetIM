using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCenter.DTO;

namespace UserCenter.IServices
{
    public interface IAppInfoService:IServiceTag
    {
        Task<AppInfoDTO> GetByAppKeyAsync(string appKey);
    }
}
