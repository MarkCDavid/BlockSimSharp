using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Randomness
{
    private readonly Random _randomNumberGenerator;
    private readonly Random _hashPowerRandomNumberGenerator;

    public Randomness(Configuration configuration)
    {
        _randomNumberGenerator = configuration.RandomNumberGenerator.UseStaticSeed
            ? new Random(configuration.RandomNumberGenerator.StaticSeed)
            : new Random();
        
        _hashPowerRandomNumberGenerator = configuration.RandomNumberGenerator.HashPowerUseStaticSeed
            ? new Random(configuration.RandomNumberGenerator.HashPowerStaticSeed)
            : new Random();
    }

    public int Next() => _randomNumberGenerator.Next();
    public double NextDouble(double lowBound, double highBound)
    {
        return lowBound + _randomNumberGenerator.NextDouble() * (highBound - lowBound);
    }
    
    public bool Binary(double probability) => _randomNumberGenerator.NextDouble() < probability;

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
            return 1.0 - _hashPowerRandomNumberGenerator.NextDouble();
        }
    }
}