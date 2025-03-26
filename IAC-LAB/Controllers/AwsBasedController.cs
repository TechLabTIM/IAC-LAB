using IAC_LAB.Domain.DTOs;
using IAC_LAB.Services.AwsServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IAC_LAB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwsBasedController : ControllerBase
    {
        private readonly IAwsServiceTerraform _terraformService;

        public AwsBasedController(IAwsServiceTerraform terraformService)
        {
            _terraformService = terraformService;
        }

        [HttpPost("plan")]
        public async Task<IActionResult> PlanScenario([FromBody] ScenarioRequestDto request)
        {
            var result = await _terraformService.PlanScenarioAsync(request);
            return Ok(result);
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyScenario([FromBody] ScenarioRequestDto request)
        {
            var result = await _terraformService.ApplyScenarioAsync(request);
            return Ok(result);
        }

        [HttpPost("destroy")]
        public async Task<IActionResult> DestroyScenario([FromBody] ScenarioRequestDto request)
        {
            var result = await _terraformService.DestroyScenarioAsync(request);
            return Ok(result);
        }

        [HttpGet("templates")]
        public IActionResult GetTemplates()
        {
            var templates = _terraformService.ListAvailableTemplates();
            return Ok(templates);
        }


    }
}
