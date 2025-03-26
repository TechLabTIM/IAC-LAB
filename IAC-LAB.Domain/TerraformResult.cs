using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAC_LAB.Domain
{
    public class TerraformResult
    {
        public bool Success { get; set; }
        public string StdOut { get; set; }
        public string StdErr { get; set; }
        public DateTime ExecutedAt { get; set; }
    }

}
