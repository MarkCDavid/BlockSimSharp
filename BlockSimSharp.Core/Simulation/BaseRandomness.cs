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

    public float Expovariate(float lambda) => -MathF.Log(1.0f - _randomNumberGenerator.NextSingle()) / lambda;
}