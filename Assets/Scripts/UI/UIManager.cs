using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIManager : RegulatorSingleton<UIManager>
{
    private Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();

    private Transform canvasTransform;

    private Camera uiCamera;

    public T ShowPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;

        if (panels.ContainsKey(panelName))
        {
            Debug.LogWarning($"Panel {panelName} is already open.");
            return panels[panelName] as T;
        }

        // 判断是否存在面板预制体
        GameObject prefab = Resources.Load<GameObject>($"UI/{panelName}");
        if (prefab == null)
        {
            Debug.LogError($"Panel prefab {panelName} not found in Resources/UI.");
            return null;
        }

        GameObject panelObject = GameObject.Instantiate(prefab);
        panelObject.transform.SetParent(canvasTransform, false);

        T panel = panelObject.GetComponent<T>();

        panels[panelName] = panel;

        panel.OnClosePanel += _ => { panels.Remove(panelName); };

        panel.OpenPanel();

        return panel;
    }

    public void HidePanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;

        if (panels.ContainsKey(panelName))
        {
            panels[panelName].ClosePanel();
        }
        else
        {
            Debug.LogWarning($"Panel {panelName} is not open.");
        }
    }

    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;

        if (panels.ContainsKey(panelName))
        {
            return panels[panelName] as T;
        }

        Debug.LogWarning($"Panel {panelName} is not found.");
        return null;
    }

    public void CloseAllPanels()
    {
        List<BasePanel> panelsToClose = panels.Values.ToList(); // .ToList()为浅拷贝
        foreach (var panel in panelsToClose)
        {
            panel.ClosePanel();
        }
        // 在打开面板时加上了关闭时自动移出字典的事件
        // panels.Clear();
    }

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        this.canvasTransform = GameObject.Find("Canvas")?.transform;
        // 如果没找到，则加载Canvas预制体
        if (this.canvasTransform == null)
        {
            GameObject canvasPrefab = Resources.Load<GameObject>("UI/Canvas");
            if (canvasPrefab == null)
            {
                Debug.LogError("Canvas prefab not found in Resources/UI.");
                return;
            }

            GameObject canvasObject = GameObject.Instantiate(canvasPrefab);
            this.canvasTransform = canvasObject.transform;
        }


        this.uiCamera = GameObject.Find("UI Camera")?.GetComponent<Camera>();
        if (uiCamera == null)
        {
            GameObject uiCameraPrefab = Resources.Load<GameObject>("UI/UICamera");
            if (uiCameraPrefab == null)
            {
                Debug.LogError("UI Camera prefab not found in Resources/UI.");
                return;
            }

            GameObject uiCameraObject = GameObject.Instantiate(uiCameraPrefab);
            this.uiCamera = uiCameraObject.GetComponent<Camera>();
        }

        // 将uiCamera作为Canvas的渲染相机
        this.canvasTransform.GetComponent<Canvas>().worldCamera = this.uiCamera;

        DontDestroyOnLoad(this.uiCamera.gameObject);
        DontDestroyOnLoad(this.canvasTransform.gameObject);
    }
}