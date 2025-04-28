using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace HealthCheck.Server
{
    public class CustomHealthCheckOptions : HealthCheckOptions
    {
        // step 1: override the constructor:
        // step 2: init new json serializer option
        //    - override the ResponseWriter delegate 
        //      --> create results using the new json serializer option
        //      --> inside results make a new Json.
        public CustomHealthCheckOptions() : base()
        {
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true

            };
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status200OK;

                var result = JsonSerializer.Serialize(new
                {
                    checks = report.Entries.Select(e => new
                    {
                        name = e.Key,
                        responseTime = e.Value.Duration.TotalMilliseconds,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description

                    }),
                    totalStatus = report.Status,
                    totalResponseTime = report.TotalDuration.TotalMilliseconds,
                }, jsonSerializerOptions);

                await context.Response.WriteAsync(result);
            };


        }

    }
}
