using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : RegulatorSingleton<GameManager>
{
    [SerializeField] private PlayerPartyManager party;
    [SerializeField] StringEventChannel gameScoreEventChannel;

    private EventBinding<GameOverEvent> gameOverEventBinding;
    private EventBinding<EnemyDieEvent> scoreEventBinding;


    public Transform PlayerTransform => party.CameraFocusPoint;

    public int Score { private set; get; }

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        gameOverEventBinding = new EventBinding<GameOverEvent>(GameOver);
        EventBus<GameOverEvent>.Register(gameOverEventBinding);

        scoreEventBinding = new EventBinding<EnemyDieEvent>(AddScore);
        EventBus<EnemyDieEvent>.Register(scoreEventBinding);

        gameScoreEventChannel = Resources.Load<StringEventChannel>("Datas/GameScore");
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

        party = GameObject.Find("PlayerParty")?.GetComponent<PlayerPartyManager>();
        EventBus<FreshHealthEvent>.Raise(new FreshHealthEvent { red = 5, soul = 5 });

        EnemySpawner.Instance.StartSpawning();
    }

    public void Move2Battle()
    {
        // TODO: 转换为延迟加载场景
        SceneManager.LoadScene("Scenes/BattleScene");
        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/BattleScene", LoadSceneMode.Single);
        // asyncLoad.completed += (AsyncOperation op) => { InitBattle(); };
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
        PublishGameScore();
    }

    private void PublishGameScore()
    {
        if (gameScoreEventChannel != null)
            gameScoreEventChannel.Invoke("Score: " + Score.ToString());
    }

    private void OnDestroy()
    {
        EventBus<GameOverEvent>.Unregister(gameOverEventBinding);
        EventBus<EnemyDieEvent>.Unregister(scoreEventBinding);
    }
}