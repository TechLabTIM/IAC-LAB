using System.Diagnostics;
using System.Text;
using IAC_LAB.Domain.DTOs;
using IAC_LAB.Domain;
using System.Text.Json;
using IAC_LAB.Domain.Configurations;
using Microsoft.Extensions.Options;

namespace IAC_LAB.Services.OpenstackServices
{
    public class OpenstackServiceTerraform : IOpenstackServiceTerraform
    {

        private readonly OpenstackAuthOptions _auth;

        public OpenstackServiceTerraform(IOptions<OpenstackAuthOptions> authOptions)
        {
            _auth = authOptions.Value;
        }

        public async Task<TerraformResultDto> CreateProjectAsync(CreateProjectDto request)
        {
            try
            {
                var envVars = GetOpenstackEnvVars();
                var args = $"project create {request.ProjectName} --domain {request.DomainId} --description \"{request.Description}\"";

                var result = await RunOpenstackCommand("openstack", args, envVars);
                return new TerraformResultDto
                {
                    Success = result.Success,
                    Output = result.StdOut,
                    Error = result.StdErr,
                    Timestamp = result.ExecutedAt
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<List<ProjectDto>> ListProjectsAsync()
        {
            try
            {
                var env = GetOpenstackEnvVars();
                var args = "project list --format json";

                var result = await RunOpenstackCommand("openstack", args, env);
                if (!result.Success) throw new Exception($"Failed to list projects: {result.StdErr}");

                return JsonSerializer.Deserialize<List<ProjectDto>>(result.StdOut,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        private Dictionary<string, string> GetOpenstackEnvVars()
        {
            return new Dictionary<string, string>
            {
                ["OS_AUTH_URL"] = _auth.OS_AUTH_URL,
                ["OS_PROJECT_NAME"] = _auth.OS_PROJECT_NAME,
                ["OS_USERNAME"] = _auth.OS_USERNAME,
                ["OS_PASSWORD"] = _auth.OS_PASSWORD,
                ["OS_USER_DOMAIN_NAME"] = _auth.OS_USER_DOMAIN_NAME,
                ["OS_PROJECT_DOMAIN_NAME"] = _auth.OS_PROJECT_DOMAIN_NAME
            };
        }

        private async Task<TerraformResult> RunOpenstackCommand(string command, string args, Dictionary<string, string> envVars)
        {
            var psi = new ProcessStartInfo
            {
                FileName = command,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            foreach (var kv in envVars)
            {
                psi.Environment[kv.Key] = kv.Value;
            }

            var process = new Process { StartInfo = psi };
            var output = new StringBuilder();
            var error = new StringBuilder();

            process.OutputDataReceived += (_, e) => output.AppendLine(e.Data);
            process.ErrorDataReceived += (_, e) => error.AppendLine(e.Data);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync();

            return new TerraformResult
            {
                Success = process.ExitCode == 0,
                StdOut = output.ToString(),
                StdErr = error.ToString(),
                ExecutedAt = DateTime.UtcNow
            };
        }
    }
}
