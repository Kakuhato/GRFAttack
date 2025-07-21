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
    
    public event Action<int> OnDollChosen = delegate { }; 
    
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < dollsList.Count; i++)
        {
            // 防止形成闭包
            int index = i;
            dollsList[i].OnChosen += () =>
            {
                GameDataManager.Instance.dollData.dollId = index;
                GameDataManager.Instance.dollData.dollName = dollsList[index].gameObject.name;
                OnDollChosen.Invoke(index);
            };
            if(i == GameDataManager.Instance.dollData.dollId) 
            {
                dollsList[i].toggle.isOn = true;
            }
        }
        
        
    }

    
    
}
