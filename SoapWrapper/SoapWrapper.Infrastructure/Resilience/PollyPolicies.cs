using Polly;
using Polly.Retry;
using SoapWrapper.Infrastructure.Resilience.Config;
using System.ServiceModel;

namespace SoapWrapper.Infrastructure.Resilience;

public static class PollyPolicies
{
    public static AsyncRetryPolicy GetRetryPolicy(PollyOptions options)
    {
        return Policy
            .Handle<TimeoutException>()
            .Or<CommunicationException>()
            .Or<FaultException>()
            .WaitAndRetryAsync(
                retryCount: options.RetryCount,
                sleepDurationProvider: attempt => 
                    TimeSpan.FromSeconds(Math.Pow(options.SleepDurationBase,attempt)),
                onRetry: (exception, time, retryCount, context) =>
                {
                    Console.WriteLine(
                        $"Retry {retryCount} after {time.TotalSeconds}s due to {exception.Message}");
                }
            );
    }
}
