using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseDollPanel : BasePanel
{
    public DollsList dollsList;
    public TextMeshProUGUI dollNameText;

    public Button backButton;
    public Button actionButton;

    protected override void Init()
    {
        base.Init();
        // TODO: 动态加载角色
        if (dollNameText == null)
        {
            dollNameText = transform.Find("DollNameText").GetComponent<TextMeshProUGUI>();
        }

        dollsList.AddOnChooseListener((index) =>
            {
                GameDataManager.Instance.dollData.dollId = index;
                dollNameText.text = index.ToString();
            }
        );

        dollNameText.text = GameDataManager.Instance.dollData.dollId.ToString();

        backButton.onClick.AddListener(() =>
            {
                UIManager.Instance.HidePanel<ChooseDollPanel>();
                UIManager.Instance.ShowPanel<BeginPanel>();
            }
        );

        actionButton.onClick.AddListener(() =>
            {
                print("action");
                UIManager.Instance.CloseAllPanels();
                GameManager.Instance.Move2Battle();
            }
        );
    }
}