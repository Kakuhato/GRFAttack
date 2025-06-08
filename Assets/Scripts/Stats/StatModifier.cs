using System;

public abstract class StatModifier : IDisposable
{
    public bool MarkedForRemoval { get; private set; }
    
    public event Action<StatModifier> OnDispose = delegate { }; 
    
    private readonly CountdownTimer countdownTimer;
    
    public abstract void Handle(object sender, Query query);

    public void Dispose()
    {
        MarkedForRemoval = true;
        OnDispose.Invoke(this);
    }
}