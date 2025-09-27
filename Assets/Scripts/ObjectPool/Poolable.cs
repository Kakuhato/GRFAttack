using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
    public event Action<GameObject> OnDispose = delegate { };

    public void Dispose()
    {
        OnDispose.Invoke(this.gameObject);
    }
}