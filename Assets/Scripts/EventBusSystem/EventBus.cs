using System;
using System.Collections.Generic;
using UnityEngine;


public static class EventBus<T> where T : IEvent
{
    private static readonly HashSet<IEventBinding<T>> Bindings = new HashSet<IEventBinding<T>>();

    public static void Register(IEventBinding<T> binding)
    {
        Bindings.Add(binding);
    }

    public static void Unregister(IEventBinding<T> binding)
    {
        Bindings.Remove(binding);
    }

    // @ 的作用是告诉编译，虽然这是个关键字，但我要把它当作普通标识符用。
    public static void Raise(T @event)
    {
        foreach (var binding in Bindings)
        {
            binding.OnEvent.Invoke(@event);
            binding.OnEventNoParam.Invoke();
        }
    }

    static void Clear()
    {
        Debug.Log($"Clearing all bindings for event type {typeof(T).Name}");
        Bindings.Clear();
    }
}