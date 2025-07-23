using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button startButton;
    public Button optionButton;
    public Button exitButton;

    protected override void Init()
    {
        base.Init();
        startButton.onClick.AddListener(() =>
            {
                print("start");
                UIManager.Instance.HidePanel<BeginPanel>();
                UIManager.Instance.ShowPanel<ChooseDollPanel>();
            }
        );
        optionButton.onClick.AddListener(() =>
            {
                UIManager.Instance.HidePanel<BeginPanel>();
                UIManager.Instance.ShowPanel<OptionPanel>();
            }
        );
        exitButton.onClick.AddListener(() => { Application.Quit(); }
        );
    }
}