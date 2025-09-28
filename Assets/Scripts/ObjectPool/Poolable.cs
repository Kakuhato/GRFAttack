using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public event Action<GameObject> OnDispose = delegate { };

    protected bool isDisposed = true;

    public void Dispose()
    {
        if (isDisposed) return;
        isDisposed = true;
        OnDispose.Invoke(this.gameObject);
    }
}