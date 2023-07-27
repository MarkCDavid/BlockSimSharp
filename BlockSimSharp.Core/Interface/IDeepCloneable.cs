namespace BlockSimSharp.Core.Interface;

public interface IDeepCloneable<out T>
{
    T DeepClone();
}