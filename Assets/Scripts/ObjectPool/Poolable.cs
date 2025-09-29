using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public event Action<Poolable> OnDispose = delegate { };

    protected bool IsDisposed = false;

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