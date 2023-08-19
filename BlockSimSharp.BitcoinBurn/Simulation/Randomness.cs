using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Randomness
{
    private readonly Random _randomNumberGenerator;

    public Randomness(Configuration configuration)
    {
        _randomNumberGenerator = configuration.RandomNumberGenerator.UseStaticSeed
            ? new Random(configuration.RandomNumberGenerator.StaticSeed)
            : new Random();
    }

    public int Next() => _randomNumberGenerator.Next();
    public int Next(int maxValue) => _randomNumberGenerator.Next(maxValue);
    public int Next(int minValue, int maxValue) => _randomNumberGenerator.Next(minValue, maxValue);

    public double Expovariate(double lambda)
    {
        return -Math.Log(1.0 - _randomNumberGenerator.NextDouble()) / lambda;
    }

    public double NextGaussian()
    {
        var radiusVariable = Uniform0To1Inclusive();
        var angleVariable = Uniform0To1Inclusive();
        
        // Box-Muller transform
        return Math.Sqrt(-2.0 * Math.Log(radiusVariable)) * Math.Sin(2.0 * MathF.PI * angleVariable);

        double Uniform0To1Inclusive()
        {
            return 1.0 - _randomNumberGenerator.NextDouble();
        }
    }
}