public interface IEvent
{
}


public struct TestEvnet : IEvent
{
}

public struct PlayerEvent : IEvent
{
    public int health;
    public int score;
}

public struct HealthEvent : IEvent
{
    public int hurts;
}