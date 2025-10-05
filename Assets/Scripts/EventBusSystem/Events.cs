using UnityEngine;

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
    public HealthType healthType;
    public Transform idx;
}

public struct FreshHealthEvent : IEvent
{
    public int red;
    public int soul;
}

public struct GameOverEvent : IEvent
{
}

public struct PauseEvent : IEvent
{
    public bool isPaused;
}

public struct EnemyDieEvent : IEvent
{
    public int ScoreGained;
    public Vector3 Position;
}