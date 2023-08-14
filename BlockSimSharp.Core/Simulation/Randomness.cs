using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;

namespace BlockSimSharp.Core.Simulation;

public sealed class Randomness
{
    private readonly Random _randomNumberGenerator;

    public Randomness(Settings settings)
    {
        _randomNumberGenerator = settings.Get<RandomNumberGenerationSettings>().UseStaticSeed
            ? new Random(settings.Get<RandomNumberGenerationSettings>().StaticSeed)
            : new Random();
    }

    public int Next() => _randomNumberGenerator.Next();
    public int Next(int maxValue) => _randomNumberGenerator.Next(maxValue);
    public int Next(int minValue, int maxValue) => _randomNumberGenerator.Next(minValue, maxValue);

    public float Expovariate(float lambda)
    {
        return -MathF.Log(1.0f - _randomNumberGenerator.NextSingle()) / lambda;
    }

    public float NextGaussian()
    {
        var radiusVariable = Uniform0To1Inclusive();
        var angleVariable = Uniform0To1Inclusive();
        
        return MathF.Sqrt(-2.0f * MathF.Log(radiusVariable)) * MathF.Sin(2.0f * MathF.PI * angleVariable);

        float Uniform0To1Inclusive()
        {
            return 1.0f - _randomNumberGenerator.NextSingle();
        }
    }
}