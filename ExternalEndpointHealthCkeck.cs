using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace microservices
{
  public class ExternalEndpointHealthCkeck : IHealthCheck
  {
    private readonly ServiceSettings settings;

    public ExternalEndpointHealthCkeck(IOptions<ServiceSettings> options)
    {
      this.settings = options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
      Ping ping = new();
      var reply = await ping.SendPingAsync(settings.OpenWeatherHost);

      if (reply.Status != IPStatus.Success)
        return HealthCheckResult.Unhealthy();
      
      return HealthCheckResult.Healthy();
    }
  }
}