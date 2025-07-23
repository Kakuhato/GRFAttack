using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : RegulatorSingleton<GameManager>
{
    public GameObject player;

    public int initialRedHealth;
    public int initialSoulHealth;

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        player = GameObject.Find("Doll");
        initialRedHealth = player.GetComponent<Entity>().Health.currentRed;
        initialSoulHealth = player.GetComponent<Entity>().Health.currentSoul;
    }


    public void InitGame()
    {
        UIManager.Instance.ShowPanel<BeginPanel>();
        AudioManager.Instance.PlayBackGroundMusic("Audio/Cyborg");
    }

    public void InitBattle()
    {
        UIManager.Instance.ShowPanel<GamePanel>();
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
}