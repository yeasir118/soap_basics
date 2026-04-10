namespace SoapWrapper.Infrastructure.Resilience.Config;

public class PollyOptions
{
    public int RetryCount { get; set; }
    public int SleepDurationBase { get; set; }

    public int ExceptionsAllowedBeforeBreaking { get; set; }
    public int DurationOfBreakSeconds { get; set; }
}
