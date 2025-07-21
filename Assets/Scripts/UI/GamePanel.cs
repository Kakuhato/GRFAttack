using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Button pauseButton;

    protected override void Init()
    {
        base.Init();
        pauseButton.onClick.AddListener(
            () =>
            {
                UIManager.Instance.ShowPanel<PausePanel>();
            }
        );
    }
}
