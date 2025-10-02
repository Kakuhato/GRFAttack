using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : RegulatorSingleton<GameManager>
{
    [SerializeField] private GameObject player;

    public List<int> playerHealthInfo;

    private EventBinding<GameOverEvent> gameOverEventBinding;
    private EventBinding<EnemyDieEvent> scoreEventBinding;

    public Transform PlayerTransform => player.transform;

    public int Score { private set; get; }

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        gameOverEventBinding = new EventBinding<GameOverEvent>(GameOver);
        EventBus<GameOverEvent>.Register(gameOverEventBinding);

        scoreEventBinding = new EventBinding<EnemyDieEvent>(AddScore);
        EventBus<EnemyDieEvent>.Register(scoreEventBinding);
    }


    public void InitGame()
    {
        UIManager.Instance.ShowPanel<BeginPanel>();
        AudioManager.Instance.PlayBackGroundMusic("Audio/Cyborg");
    }

    public void InitBattle()
    {
        AudioManager.Instance.PlayBackGroundMusic("Audio/SinOfFire");
        Score = 0;
        UIManager.Instance.ShowPanel<GamePanel>();

        player = GameObject.Find("Doll");
        playerHealthInfo = player.GetComponent<PlayerStats>().GetHealthInfo();

        // EnemySpawner.Instance.StartSpawning();
    }

    public void Move2Battle()
    {
        // TODO: 转换为延迟加载场景
        SceneManager.LoadScene("Scenes/BattleScene");
    }

    public void Move2Begin()
    {
        // BeginScene有BeginMain入口，会调用一次InitGame()
        Time.timeScale = 1; // TODO: 改暂停机制
        EnemySpawner.Instance.StopSpawning();
        SceneManager.LoadScene("Scenes/BeginScene");
    }

    public void GameOver()
    {
        Time.timeScale = 0; // TODO: 改暂停机制
        EnemySpawner.Instance.StopSpawning();
        UIManager.Instance.CloseAllPanels();
        UIManager.Instance.ShowPanel<GameOverPanel>();
    }

    public void AddScore(EnemyDieEvent delta)
    {
        Score += delta.ScoreGained;
        UIManager.Instance.UpdatePanel(Score);
    }

    private void OnDestroy()
    {
        EventBus<GameOverEvent>.Unregister(gameOverEventBinding);
        EventBus<EnemyDieEvent>.Unregister(scoreEventBinding);
    }
}