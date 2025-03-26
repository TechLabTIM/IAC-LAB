using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAC_LAB.Domain.Enums;

namespace IAC_LAB.Domain.Entities
{
    public class Scenario
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string TemplatePath { get; set; } // e.g., path to the .tf folder
        public Dictionary<string, string> Variables { get; set; }
        public DateTime CreatedAt { get; set; }
        public ScenarioStatus Status { get; set; }
    }

}
