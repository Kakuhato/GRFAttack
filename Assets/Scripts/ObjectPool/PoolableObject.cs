using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnDispose = delegate { };
    private bool IsDisposed = false;

    protected virtual void OnEnable()
    {
        IsDisposed = false;
    }

    public void Dispose()
    {
        if (IsDisposed) return;
        IsDisposed = true;
        OnDispose.Invoke(this);
    }
}