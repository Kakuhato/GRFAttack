using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Button exitButton;
    public TextMeshProUGUI scoreText;


    protected override void Init()
    {
        base.Init();
        exitButton.onClick.AddListener(() =>
            {
                GameDataManager.Instance.SaveMusicData();
                UIManager.Instance.CloseAllPanels();
                GameManager.Instance.Move2Begin();
                // BeginScene有Main入口，会自动打开一次主UI
                // 之后改成gameManager之后这里需要统一逻辑
                // UIManager.Instance.ShowPanel<BeginPanel>();
            }
        );
        scoreText.text = $"Score: {GameManager.Instance.Score}";
    }
}