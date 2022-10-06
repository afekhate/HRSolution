using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Domain
{
    public class SystemSetting : BaseObject
    {
        public string LookUpCode { get; set; }
        public string ItemName { get; set; }
        public string ItemValue { get; set; }

    }
}
