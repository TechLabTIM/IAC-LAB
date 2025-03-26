using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using IAC_LAB.Domain.DTOs;
using IAC_LAB.Domain;
using System.Reflection;


namespace IAC_LAB.Services.AwsServices
{
    public class AwsServiceTerraform : IAwsServiceTerraform
    {
        private readonly string _templateDirectory;
        public AwsServiceTerraform()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            _templateDirectory = Path.Combine(assemblyDirectory, "templates");
        }

        public async Task<TerraformResultDto> ApplyScenarioAsync(ScenarioRequestDto request)
        {
            try
            {
                var scenarioId = Guid.NewGuid().ToString();
                var scenarioDir = Path.Combine(Path.GetTempPath(), $"scenario_{scenarioId}");

                Directory.CreateDirectory(scenarioDir);

                // Load template
                var templatePath = Path.Combine(_templateDirectory, $"{request.TemplateName}.tf");
                var templateContent = await File.ReadAllTextAsync(templatePath);

                foreach (var variable in request.Variables)
                {
                    templateContent = templateContent.Replace($"${{{variable.Key}}}", variable.Value);
                }

                var mainTfPath = Path.Combine(scenarioDir, "main.tf");
                await File.WriteAllTextAsync(mainTfPath, templateContent);

                // Run terraform init and apply
                var result = await ExecuteTerraformCommandsAsync(scenarioDir);

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

        public async Task<TerraformResultDto> PlanScenarioAsync(ScenarioRequestDto request)
        {
            try
            {
                var scenarioDir = await PrepareScenarioDirectoryAsync(request);
                var init = await RunCommand("terraform", "init", scenarioDir);
                if (!init.Success) return ToDto(init);

                var plan = await RunCommand("terraform", "plan", scenarioDir);
                return ToDto(plan);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public async Task<TerraformResultDto> DestroyScenarioAsync(ScenarioRequestDto request)
        {
            try
            {
                var scenarioDir = await PrepareScenarioDirectoryAsync(request);
                var init = await RunCommand("terraform", "init", scenarioDir);
                if (!init.Success) return ToDto(init);

                var destroy = await RunCommand("terraform", "destroy -auto-approve", scenarioDir);
                return ToDto(destroy);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public IEnumerable<string> ListAvailableTemplates()
        {
            if (!Directory.Exists(_templateDirectory))
                return Enumerable.Empty<string>();

            return Directory
                .EnumerateFiles(_templateDirectory, "*.tf")
                .Select(Path.GetFileNameWithoutExtension);
        }



        private TerraformResultDto ToDto(TerraformResult result)
        {
            return new TerraformResultDto
            {
                Success = result.Success,
                Output = result.StdOut,
                Error = result.StdErr,
                Timestamp = result.ExecutedAt
            };
        }


        private async Task<string> PrepareScenarioDirectoryAsync(ScenarioRequestDto request)
        {
            var scenarioId = Guid.NewGuid().ToString();
            var scenarioDir = Path.Combine(Path.GetTempPath(), $"scenario_{scenarioId}");

            Directory.CreateDirectory(scenarioDir);

            var templatePath = Path.Combine(_templateDirectory, $"{request.TemplateName}.tf");
            var templateContent = await File.ReadAllTextAsync(templatePath);

            foreach (var variable in request.Variables)
            {
                templateContent = templateContent.Replace($"${{{variable.Key}}}", variable.Value);
            }

            var mainTfPath = Path.Combine(scenarioDir, "main.tf");
            await File.WriteAllTextAsync(mainTfPath, templateContent);

            return scenarioDir;
        }

        private async Task<TerraformResult> ExecuteTerraformCommandsAsync(string workingDir)
        {
            var init = await RunCommand("terraform", "init", workingDir);
            if (!init.Success) return init;

            return await RunCommand("terraform", "apply -auto-approve", workingDir);
        }

        private async Task<TerraformResult> RunCommand(string fileName, string args, string workingDir)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                WorkingDirectory = workingDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = startInfo };

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
