using System.Diagnostics;

namespace TimeDependenceOnAmountOfZeroesExperiment;

public static class Average
{
    public static double GetAverageRuntimeForHashingWithLeadingZeroes(int runCount, int leadingZeroes)
    {
        var nonce = Bytes.NewIntegerBytes();
        var hash = Bytes.NewHashResultBytes();
        var prng = new Random();
        var stopwatch = new Stopwatch();
    
        var totalDuration = 0.0D;

        for (var run = 0; run < runCount; run++)
        {
            stopwatch.Restart();
            do
            {
                prng.Next().ToBytes(ref nonce);
                Hash.GenerateSha256Hash(ref nonce, ref hash);
            }
            while (!Hash.IsHashWithLeadingZeroes(ref hash, leadingZeroes));

            stopwatch.Stop();
            totalDuration += stopwatch.Elapsed.TotalMilliseconds;
        }

        return totalDuration / runCount;
    }
}