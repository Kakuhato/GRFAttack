using System;

public interface IPoolable
{
    public event Action<IPoolable> OnDispose;
    public void Dispose();
}