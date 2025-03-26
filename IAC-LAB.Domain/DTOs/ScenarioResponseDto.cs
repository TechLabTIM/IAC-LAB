using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAC_LAB.Domain.Enums;

namespace IAC_LAB.Domain.DTOs
{
    public class ScenarioResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ScenarioStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
