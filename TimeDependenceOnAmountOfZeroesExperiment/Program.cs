using TimeDependenceOnAmountOfZeroesExperiment;

const int hexZeroRatio = 16;
const int maximumLeadingZeroes = 5;

var averageRuntimeWithLeadingZeroes = new Dictionary<int, double>();

for (var leadingZeroes = maximumLeadingZeroes; leadingZeroes > 0; leadingZeroes--)
{
    var runCount = IntegerMath.Power(hexZeroRatio, maximumLeadingZeroes - leadingZeroes + 1);
    var averageRuntime = Average.GetAverageRuntimeForHashingWithLeadingZeroes(runCount, leadingZeroes);
    averageRuntimeWithLeadingZeroes.Add(leadingZeroes, averageRuntime);
}

double? previousAverageRuntime = null;
foreach (var (leadingZeroes, averageRuntime) in averageRuntimeWithLeadingZeroes)
{
    Console.WriteLine($"Average time for {leadingZeroes} zero: {averageRuntime} ms");
    if (previousAverageRuntime.HasValue)
    {
        Console.WriteLine($"Ratio from {previousAverageRuntime} to {averageRuntime} is: {previousAverageRuntime / averageRuntime}");
    }

    previousAverageRuntime = averageRuntime;
}
