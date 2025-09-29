using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Button pauseButton;

    public HealthBar healthBar;

    public TextMeshProUGUI scoreText;

    protected override void Init()
    {
        base.Init();
        pauseButton.onClick.AddListener(() => { UIManager.Instance.ShowPanel<PausePanel>(); }
        );
    }

    public void updateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
}