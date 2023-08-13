namespace TimeDependenceOnAmountOfZeroesExperiment;

public static class IntegerMath
{
    public static int Power(int @base, int power)
    {
        var result = 1;

        for (var _ = 0; _ < power; _++)
        {
            result *= @base;
        }
        
        return result;
    }
}