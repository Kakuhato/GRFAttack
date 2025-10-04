using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventChannel<T> : ScriptableObject
{
    private HashSet<EventListener<T>> observers = new HashSet<EventListener<T>>();

    public void Invoke(T value)
    {
        foreach (var observer in observers)
        {
            observer.Raise(value);
        }
    }

    public void Register(EventListener<T> observer) => observers.Add(observer);
    public void Unregister(EventListener<T> observer) => observers.Remove(observer);
}