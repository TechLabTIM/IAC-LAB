using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAC_LAB.Domain.DTOs;

namespace IAC_LAB.Services.OpenstackServices
{
    public interface IOpenstackServiceTerraform
    {
        Task<TerraformResultDto> CreateProjectAsync(CreateProjectDto request);
        Task<List<ProjectDto>> ListProjectsAsync();

    }
}
