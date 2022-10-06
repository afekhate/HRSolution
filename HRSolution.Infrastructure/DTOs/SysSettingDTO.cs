using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.DTOs
{
    public class SysSettingDTO
    {
        public int SysSettingId { get; set; }
        public string LookUpCode { get; set; }
        public string ItemName { get; set; }
        public string ItemValue { get; set; }
    }
}
