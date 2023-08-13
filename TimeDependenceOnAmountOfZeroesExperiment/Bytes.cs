using System.Runtime.CompilerServices;

namespace TimeDependenceOnAmountOfZeroesExperiment;

public static class Bytes
{
    public static byte[] NewIntegerBytes()
    {
        return new byte[sizeof(int)];
    }

    public static byte[] NewHashResultBytes()
    {
        return new byte[32];
    }

    public static void ToBytes(this int value, ref byte[] storage)
    {
        Unsafe.As<byte, int>(ref storage[0]) = value;
    }
}