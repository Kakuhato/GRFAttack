using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasePanel : MonoBehaviour
{
    protected bool isOpen;
    protected string PanelName;

    protected CanvasGroup canvasGroup;

    public event Action<string> OnOpenPanel = delegate { };
    public event Action<string> OnClosePanel = delegate { };

    void Awake()
    {
        this.canvasGroup = GetComponent<CanvasGroup>();
        if (this.canvasGroup == null)
        {
            this.canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        this.isOpen = false;

        Init();
    }

    protected virtual void Init()
    {
    }

    public virtual void OpenPanel()
    {
        this.isOpen = true;
        canvasGroup.alpha = 1f;
        OnOpenPanel.Invoke(this.PanelName);
    }

    public virtual void ClosePanel()
    {
        this.isOpen = false;
        canvasGroup.alpha = 0f;
        OnClosePanel.Invoke(this.PanelName);
        Destroy(this.gameObject);
    }

    public virtual void UpdatePanelData(int data)
    {
    }
}