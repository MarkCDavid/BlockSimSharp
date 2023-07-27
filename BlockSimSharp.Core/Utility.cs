namespace BlockSimSharp.Core;

public static class Utility
{
    private static readonly Random Random = new();
    
    public static float Expovariate(float lambda)
    {
        return -MathF.Log(1.0f - Random.NextSingle()) / lambda;
    }
}