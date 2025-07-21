using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DollsList : MonoBehaviour
{
    public ToggleGroup dollToggleGroup;
    
    // TODO: 动态从本地json添加角色
    public List<ChosenDoll> dollsList;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < dollsList.Count; i++)
        {
            // 防止形成闭包
            int index = i;
            dollsList[i].dollId = index;
            dollsList[i].dollName = "Doll " + index;
            if(i == GameDataManager.Instance.dollData.dollId) 
            {
                dollsList[i].toggle.isOn = true;
            }
        }
    }
    
    public void AddOnChooseListener(Action<int> listener)
    {
        foreach (var doll in dollsList)
        {
            doll.OnChosen += listener;
        }
    }
    
}
