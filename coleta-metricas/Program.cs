using OpenTelemetry;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;

static class Program
{
    private static readonly Meter SMeter = new("HatCo.HatStore", "1.0.0");

    public static readonly Counter<int> SHatsSold = SMeter.CreateCounter<int>(
        name: "hats-sold",
        unit: "Hats",
        description: "The number of hats sold in our store");

    private static void Main(string[] args)
    {
        using var meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter("HatCo.HatStore")
            .AddPrometheusHttpListener(
                options => options.UriPrefixes = new[] { "http://localhost:9184/" })
            .Build();

        var rand = Random.Shared;
        Console.WriteLine("Press any key to exit");
        while (!Console.KeyAvailable)
        {
            //// Simulate hat selling transactions.
            Thread.Sleep(rand.Next(100, 2500));
            SHatsSold.Add(rand.Next(0, 1000));
        }
    }
}