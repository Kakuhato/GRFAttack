using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : RegulatorSingleton<GameManager>
{
    public GameObject player;

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        
        
    }


    public void InitGame()
    {
        UIManager.Instance.ShowPanel<BeginPanel>();
        AudioManager.Instance.PlayBackGroundMusic("Audio/Cyborg");
    }

    public void Move2Battle()
    {
        // TODO: 转换为延迟加载场景
        SceneManager.LoadScene("Scenes/BattleScene");
        UIManager.Instance.ShowPanel<GamePanel>();
    }

    public void Move2Begin()
    {
        // BeginScene有BeginMain入口，会调用一次InitGame()
        SceneManager.LoadScene("Scenes/BeginScene");
    }
    
}
