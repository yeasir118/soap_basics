using Polly;
using Polly.CircuitBreaker;
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
            .Or<CommunicationException>(ex => ex is not FaultException)
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

    public static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(PollyOptions options)
    {
        return Policy
            .Handle<TimeoutException>()
            .Or<CommunicationException>(ex => ex is not FaultException)
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: options.ExceptionsAllowedBeforeBreaking,
                durationOfBreak: TimeSpan.FromSeconds(options.DurationOfBreakSeconds),

                onBreak: (exception, duration) =>
                {
                    Console.WriteLine($"[CircuitBreaker] OPEN for {duration.TotalSeconds}s due to {exception.Message}");
                },

                onReset: () =>
                {
                    Console.WriteLine("[CircuitBreaker] CLOSED");
                },

                onHalfOpen: () =>
                {
                    Console.WriteLine("[CircuitBreaker] HALF-OPEN -> testing...");
                }
            );
    }

    public static IAsyncPolicy GetCombinedPolicy(PollyOptions options)
    {
        var retry = GetRetryPolicy(options);
        var circuit = GetCircuitBreakerPolicy(options);

        return circuit.WrapAsync(retry);
    }
}
