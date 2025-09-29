using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : RegulatorSingleton<GameManager>
{
    [SerializeField] private GameObject player;

    public int initialRedHealth;
    public int initialSoulHealth;

    public Transform PlayerTransform => player.transform;

    public int Score { private set; get; }

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
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
        AddScore(0);
        player = GameObject.Find("Doll");
        initialRedHealth = player.GetComponent<Entity>().Health.GetCurrentRed();
        initialSoulHealth = player.GetComponent<Entity>().Health.GetCurrentSoul();
        EnemySpawner.Instance.StartSpawning();
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

    public void AddScore(int delta)
    {
        Score += delta;
        UIManager.Instance.GetPanel<GamePanel>()?.updateScore(Score);
    }
}