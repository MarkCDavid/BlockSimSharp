using System.Security.Cryptography;

namespace TimeDependenceOnAmountOfZeroesExperiment;

public class Hash
{
    public static void GenerateSha256Hash(ref byte[] source, ref byte[] destination)
    {
        SHA256.HashData(source, destination);
    }
    
    // We are checking whether the hex representation of the hash contains `leadingZeroes`
    // amount of leading zeroes. As two hex symbols are used to represent a byte, this means
    // that one hex zero is represented by 4 bits (a nibble of a byte). As such, we simply
    // check half of the `leadingZeroes` bytes and if `leadingZeroes` is an odd number,
    // we check the last nibble to be a zero as well.
    public static bool IsHashWithLeadingZeroes(ref byte[] hash, int leadingZeroes)
    {
        var fullBytes = leadingZeroes / 2;
        for (var index = 0; index < fullBytes; index++)
        {
            if (hash[index] > 0)
            {
                return false;
            }
        }

        var partialNibble = leadingZeroes % 2;
        if (partialNibble > 0)
        {
            return (hash[fullBytes] & 0xF0) == 0;
        }

        return true;
    }
}