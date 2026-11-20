namespace LUMEN.Controllers;

using LUMEN.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class VirtualLabController : Controller
{
    // GET: /VirtualLab
    [HttpGet]
    public IActionResult Index()
    {
        return View("CodingLab");
    }

    // GET: /VirtualLab/CodingLab
    [HttpGet]
    public IActionResult CodingLab()
    {
        return View();
    }

    // POST: /VirtualLab/RunCode
    [HttpPost]
    public async Task<IActionResult> RunCode(
        [FromBody] CodeExecutionRequest request)
    {
        if (request == null)
        {
            return BadRequest(new
            {
                success = false,
                output = "Invalid request."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return BadRequest(new
            {
                success = false,
                output = "No code was provided."
            });
        }

        string language =
            string.IsNullOrWhiteSpace(request.Language)
                ? "python"
                : request.Language.Trim().ToLowerInvariant();

        if (language != "python")
        {
            return BadRequest(new
            {
                success = false,
                output = "Only Python is currently supported."
            });
        }

        string tempFile = Path.Combine(
            Path.GetTempPath(),
            $"lumen_{Guid.NewGuid():N}.py"
        );

        try
        {
            // ---------------------------------------------------------
            // Save Python source code
            // ---------------------------------------------------------

            await System.IO.File.WriteAllTextAsync(
                tempFile,
                request.Code
            );

            // ---------------------------------------------------------
            // Docker configuration
            // ---------------------------------------------------------

            var startInfo = new ProcessStartInfo
            {
                FileName = "docker",

                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,

                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Docker run
            startInfo.ArgumentList.Add("run");
            startInfo.ArgumentList.Add("--rm");

            // No network
            startInfo.ArgumentList.Add("--network");
            startInfo.ArgumentList.Add("none");

            // Resource limits
            startInfo.ArgumentList.Add("--memory");
            startInfo.ArgumentList.Add("128m");

            startInfo.ArgumentList.Add("--cpus");
            startInfo.ArgumentList.Add("0.5");

            startInfo.ArgumentList.Add("--pids-limit");
            startInfo.ArgumentList.Add("64");

            // Read-only filesystem
            startInfo.ArgumentList.Add("--read-only");

            // Temporary writable directory
            startInfo.ArgumentList.Add("--tmpfs");
            startInfo.ArgumentList.Add(
                "/tmp:rw,noexec,nosuid,size=16m"
            );

            // Mount Python source
            startInfo.ArgumentList.Add("-v");
            startInfo.ArgumentList.Add(
                $"{tempFile}:/app/main.py:ro"
            );

            // Python image
            startInfo.ArgumentList.Add("python:3.12-slim");

            // Python command
            startInfo.ArgumentList.Add("python");
            startInfo.ArgumentList.Add("/app/main.py");

            using var process = new Process
            {
                StartInfo = startInfo
            };

            // ---------------------------------------------------------
            // Start process
            // ---------------------------------------------------------

            process.Start();

            // ---------------------------------------------------------
            // SEND USER INPUT TO PYTHON STDIN
            // ---------------------------------------------------------

            if (!string.IsNullOrEmpty(request.Input))
            {
                await process.StandardInput.WriteAsync(
                    request.Input
                );

                // Python input() reads line-by-line.
                // Make sure the final value has a newline.
                if (!request.Input.EndsWith("\n"))
                {
                    await process.StandardInput.WriteAsync("\n");
                }
            }

            // IMPORTANT:
            // Closing stdin tells Python there is no more input.
            await process.StandardInput.FlushAsync();
            process.StandardInput.Close();

            // ---------------------------------------------------------
            // Read stdout/stderr concurrently
            // ---------------------------------------------------------

            Task<string> outputTask =
                process.StandardOutput.ReadToEndAsync();

            Task<string> errorTask =
                process.StandardError.ReadToEndAsync();

            // ---------------------------------------------------------
            // Timeout
            // ---------------------------------------------------------

            using var timeout =
                new CancellationTokenSource(
                    TimeSpan.FromSeconds(10)
                );

            try
            {
                await process.WaitForExitAsync(
                    timeout.Token
                );
            }
            catch (OperationCanceledException)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill(true);
                    }
                }
                catch
                {
                    // Ignore process kill errors.
                }

                return Ok(new
                {
                    success = false,
                    output =
                        "Execution timed out after 10 seconds."
                });
            }

            // ---------------------------------------------------------
            // Get output
            // ---------------------------------------------------------

            string output = await outputTask;
            string error = await errorTask;

            if (!string.IsNullOrWhiteSpace(error))
            {
                if (!string.IsNullOrWhiteSpace(output))
                {
                    output += Environment.NewLine;
                }

                output += error;
            }

            // ---------------------------------------------------------
            // Return result
            // ---------------------------------------------------------

            return Ok(new
            {
                success = process.ExitCode == 0,
                output = output
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                success = false,
                output =
                    "Execution failed: " + ex.Message
            });
        }
        finally
        {
            // ---------------------------------------------------------
            // Delete temporary Python file
            // ---------------------------------------------------------

            try
            {
                if (System.IO.File.Exists(tempFile))
                {
                    System.IO.File.Delete(tempFile);
                }
            }
            catch
            {
                // Ignore cleanup errors.
            }
        }
    }
}