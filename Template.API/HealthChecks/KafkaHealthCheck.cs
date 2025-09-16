using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Template.API.HealthChecks;

public class KafkaHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}