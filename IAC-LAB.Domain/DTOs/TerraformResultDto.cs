using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAC_LAB.Domain.DTOs
{
    public class TerraformResultDto
    {
        public bool Success { get; set; }
        public string Output { get; set; }
        public string Error { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
