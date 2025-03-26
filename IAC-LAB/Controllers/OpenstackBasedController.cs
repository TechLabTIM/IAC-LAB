using IAC_LAB.Domain.DTOs;
using IAC_LAB.Services.OpenstackServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IAC_LAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenstackBasedController : ControllerBase
    {
        private readonly IOpenstackServiceTerraform _openstackService;

        public OpenstackBasedController(IOpenstackServiceTerraform openstackService)
        {
            _openstackService = openstackService;
        }

        [HttpPost("create-project")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
        {
            var result = await _openstackService.CreateProjectAsync(dto);
            return Ok(result);
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _openstackService.ListProjectsAsync();
            return Ok(projects);
        }

    }
}
