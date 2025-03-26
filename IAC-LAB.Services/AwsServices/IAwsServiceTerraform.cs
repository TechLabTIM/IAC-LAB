using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAC_LAB.Domain.DTOs;

namespace IAC_LAB.Services.AwsServices
{
    public interface IAwsServiceTerraform
    {
        Task<TerraformResultDto> ApplyScenarioAsync(ScenarioRequestDto request);

        Task<TerraformResultDto> PlanScenarioAsync(ScenarioRequestDto request);
        Task<TerraformResultDto> DestroyScenarioAsync(ScenarioRequestDto request);

        IEnumerable<string> ListAvailableTemplates();

        }

}

