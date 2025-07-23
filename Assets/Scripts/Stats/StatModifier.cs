using System;

public class BasicStatModifier : StatModifier
{
    private readonly StatsType statsType;
    private readonly Func<int, int> operation;

    public BasicStatModifier(StatsType type, float duration, Func<int, int> operation) : base(duration)
    {
        this.statsType = type;
        this.operation = operation;
    }

    public override void Handle(object sender, Query query)
    {
        if (query.StatsType == statsType)
        {
            query.Value = operation(query.Value);
        }
    }
}

public abstract class StatModifier : IDisposable
{
    public bool MarkedForRemoval { get; private set; }

    public event Action<StatModifier> OnDispose = delegate { };

    private readonly CountdownTimer timer;

    protected StatModifier(float duration)
    {
        if (duration <= 0) return;
        // 当duration小于等于0时，不会有计时器，意味着永久装备
        timer = new CountdownTimer(duration);
        timer.OnTimerStop += () => MarkedForRemoval = true;
        timer.Start();
    }

    public void Update(float deltaTime) => timer?.Tick(deltaTime);

    public abstract void Handle(object sender, Query query);

    public void Dispose()
    {
        OnDispose.Invoke(this);
    }

    public void Remove()
    {
        MarkedForRemoval = true;
    }
}