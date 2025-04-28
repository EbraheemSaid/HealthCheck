using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace HealthCheck.Server
{
    public class ICMPHealthCheck : IHealthCheck
    {
        private readonly string Host = $"10.0.0.0";
        private readonly int HealthyRoundTrip = 300;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(Host);

                switch (reply.Status)
                {
                    case IPStatus.Success:
                        return (reply.RoundtripTime > HealthyRoundTrip)
                            ? HealthCheckResult.Degraded()
                            : HealthCheckResult.Healthy();

                    default:
                        return HealthCheckResult.Unhealthy();


                }

            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
