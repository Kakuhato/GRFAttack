using System;

public interface IEventBinding<T>
{
    public Action<T> OnEvent { get; set; }
    public Action OnEventNoParam { get; set; }
}


public class EventBinding<T> : IEventBinding<T> where T : IEvent
{
    private Action<T> onEvent = delegate(T _) { };
    private Action onEventNoParam = delegate { };

    // 只能通过接口引用访问，不能直接通过 EventBinding<T> 类实例访问
    Action<T> IEventBinding<T>.OnEvent
    {
        get => onEvent;
        set => onEvent += value;
    }

    Action IEventBinding<T>.OnEventNoParam
    {
        get => onEventNoParam;
        set => onEventNoParam += value;
    }

    public EventBinding(Action<T> onEvent)
    {
        this.onEvent = onEvent;
    }

    public EventBinding(Action onEventNoParam)
    {
        this.onEventNoParam = onEventNoParam;
    }

    public void Add(Action onEventNoParam)
    {
        this.onEventNoParam += onEventNoParam;
    }

    public void Remove(Action onEventNoParam)
    {
        this.onEventNoParam -= onEventNoParam;
    }

    public void Add(Action<T> onEvent)
    {
        this.onEvent += onEvent;
    }

    public void Remove(Action<T> onEvent)
    {
        this.onEvent -= onEvent;
    }
}