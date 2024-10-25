using System.Diagnostics.Metrics;

namespace TravelInspiration.API.Shared.Metrics;

/// <remarks>
/// This is automatically injected on Mediatr so we don't need to do anything to register it.
/// </remarks>
public sealed class HandlerPerformanceMetric
{
    private readonly Counter<long> _milliSecondsElapsed;

    public HandlerPerformanceMetric(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create("TravelInspiration.API");
        _milliSecondsElapsed = meter.CreateCounter<long>("travelinspiration.api.requesthandler.millisecondselapsed");
    }

    public void MilliSecondsElapsed(long milliSecondsElapsed)
        => _milliSecondsElapsed.Add(milliSecondsElapsed);
}