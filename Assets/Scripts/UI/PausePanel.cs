using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// TODO: 可以和OptionPanel合并，通过判断当前场景决定是否显示exitButton，但需要修改关闭的逻辑
public class PausePanel : BasePanel
{
    public Button closeButton;
    public Button exitButton;


    public Toggle musicToggle;
    public Slider musicSlider;

    public Toggle effectToggle;
    public Slider effectSlider;

    protected override void Init()
    {
        base.Init();
        LoadSettins();

        closeButton.onClick.AddListener(() =>
            {
                GameDataManager.Instance.SaveMusicData();
                UIManager.Instance.HidePanel<PausePanel>();
                // UIManager.Instance.ShowPanel<BeginPanel>();
            }
        );

        exitButton.onClick.AddListener(() =>
            {
                UIManager.Instance.CloseAllPanels();
                GameManager.Instance.Move2Begin();


                // BeginScene有Main入口，会自动打开一次主UI
                // 之后改成gameManager之后这里需要统一逻辑
                // UIManager.Instance.ShowPanel<BeginPanel>();
            }
        );

        musicToggle.onValueChanged.AddListener((isOn) =>
            {
                musicSlider.interactable = isOn;
                AudioManager.Instance.SetBgmMute(!isOn);
            }
        );

        effectToggle.onValueChanged.AddListener((isOn) =>
            {
                effectSlider.interactable = isOn;
                AudioManager.Instance.SetEffectMute(!isOn);
            }
        );

        musicSlider.onValueChanged.AddListener((value) => { AudioManager.Instance.SetBgmVolume(value); }
        );

        effectSlider.onValueChanged.AddListener((value) => { AudioManager.Instance.SetEffectVolume(value); });
    }

    private void LoadSettins()
    {
        musicToggle.isOn = !AudioManager.Instance.IsBgmMute();
        musicSlider.value = AudioManager.Instance.GetBgmVolume();

        effectToggle.isOn = !AudioManager.Instance.IsEffectMute();
        effectSlider.value = AudioManager.Instance.GetEffectVolume();
    }
}