using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAC_LAB.Domain.DTOs
{
    public class ScenarioRequestDto
    {
        public string Name { get; set; }
        public string TemplateName { get; set; }
        public Dictionary<string, string> Variables { get; set; }
    }

}
