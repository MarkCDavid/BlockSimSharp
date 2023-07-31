namespace BlockSimSharp.Core.Interface;

public interface ICloneable<out T>
{
    T Clone();
}