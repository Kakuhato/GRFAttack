using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : RegulatorSingleton<UIManager>
{
    
    private Dictionary<string, BasePanel> panels = new Dictionary<string, BasePanel>();
    
    private Transform canvasTransform;

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
        if (prefab== null)
        {
            Debug.LogError($"Panel prefab {panelName} not found in Resources/UI.");
            return null;
        }
        GameObject panelObject= GameObject.Instantiate(prefab);
        panelObject.transform.SetParent(canvasTransform, false);
        
        T panel = panelObject.GetComponent<T>();
        
        panels[panelName] = panel;
        
        panel.OnClosePanel += _ =>
        {
            panels.Remove(panelName);
        };
        
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
    

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        this.canvasTransform = GameObject.Find("Canvas").transform;
        DontDestroyOnLoad(this.canvasTransform.gameObject);
    }
}
