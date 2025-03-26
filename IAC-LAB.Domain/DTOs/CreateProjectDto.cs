using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAC_LAB.Domain.DTOs
{
    public class CreateProjectDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public string DomainId { get; set; } = "default";
        public string Description { get; set; } = string.Empty;
    }


}
