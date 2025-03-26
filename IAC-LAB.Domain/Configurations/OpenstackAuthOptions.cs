using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAC_LAB.Domain.Configurations
{
    public class OpenstackAuthOptions
    {
        public string OS_AUTH_URL { get; set; } = "";
        public string OS_PROJECT_NAME { get; set; } = "";
        public string OS_USERNAME { get; set; } = "";
        public string OS_PASSWORD { get; set; } = "";
        public string OS_USER_DOMAIN_NAME { get; set; } = "";
        public string OS_PROJECT_DOMAIN_NAME { get; set; } = "";
    }

}
