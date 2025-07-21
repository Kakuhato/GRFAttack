using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : BasePanel
{
    public Button closeButton;
    
    public Toggle musicToggle;
    public Slider musicSlider;
    
    public Toggle effectToggle;
    public Slider effectSlider;

    protected override void Init()
    {
        base.Init();
        LoadSettins();
        
        closeButton.onClick.AddListener(
            () =>
            {
                GameDataManager.Instance.SaveMusicData();
                UIManager.Instance.HidePanel<OptionPanel>();
                UIManager.Instance.ShowPanel<BeginPanel>();
            }
        );
        
        musicToggle.onValueChanged.AddListener(
            (isOn) =>
            {
                musicSlider.interactable = isOn;
                AudioManager.Instance.SetBgmMute(!isOn);
            }
            );
        
        effectToggle.onValueChanged.AddListener(
            (isOn) =>
            {
                effectSlider.interactable = isOn;
                AudioManager.Instance.SetEffectMute(!isOn);
            }
            );
        
        musicSlider.onValueChanged.AddListener(
            (value) =>
            {
                AudioManager.Instance.SetBgmVolume(value);
            }
            );

        effectSlider.onValueChanged.AddListener(
            (value) =>
            {
                AudioManager.Instance.SetEffectVolume(value);
            });
    }

    private void LoadSettins()
    {
        musicToggle.isOn = !AudioManager.Instance.IsBgmMute();
        musicSlider.value = AudioManager.Instance.GetBgmVolume();
        
        effectToggle.isOn = !AudioManager.Instance.IsEffectMute();
        effectSlider.value = AudioManager.Instance.GetEffectVolume();
    }
    
}
