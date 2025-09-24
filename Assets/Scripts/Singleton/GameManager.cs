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
        UIManager.Instance.ShowPanel<GamePanel>();
        player = GameObject.Find("Doll");
        initialRedHealth = player.GetComponent<Entity>().Health.GetCurrentRed();
        initialSoulHealth = player.GetComponent<Entity>().Health.GetCurrentSoul();
    }

    public void Move2Battle()
    {
        // TODO: 转换为延迟加载场景
        SceneManager.LoadScene("Scenes/BattleScene");
    }

    public void Move2Begin()
    {
        // BeginScene有BeginMain入口，会调用一次InitGame()
        SceneManager.LoadScene("Scenes/BeginScene");
    }

    public void GameOver()
    {
    }
}