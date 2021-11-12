namespace nflow.core;

public interface IStream
{
    bool IsPublic => false;
    int Order => int.MaxValue;
}
