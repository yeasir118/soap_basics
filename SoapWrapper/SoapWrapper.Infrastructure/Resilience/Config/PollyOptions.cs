namespace SoapWrapper.Infrastructure.Resilience.Config;

public class PollyOptions
{
    public int RetryCount { get; set; }
    public int SleepDurationBase { get; set; }
}
