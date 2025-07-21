using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField, Required] private HealthData healthData;
    private GameObject heartItem;
    private LinkedList<GameObject> heartList = new LinkedList<GameObject>();
    
    // 后面可采用工厂模式
    [Button("Add Health")]
    public void AddHealth()
    {
        if (RedCount < RedLimit)
        {
            RedCount++;
            GameObject heart = GameObject.Instantiate(heartItem,transform);
            Image sr = heart.GetComponent<Image>();
            sr.sprite = healthData.redHeart;
            sr.color = new Color(255, 255, 255, 1);
            heartList.AddLast(heart);
        }
        else
        {
            GameObject heart = GameObject.Instantiate(heartItem,transform);
            Image sr = heart.GetComponent<Image>();
            sr.sprite = healthData.soulHeart;
            sr.color = new Color(255, 255, 255, 1);
            heartList.AddLast(heart);
        }
        HealthCount++;
    }
    
    
    private int HealthLimit = 10;
    private int HealthCount;
    private int RedLimit;
    private int RedCount;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (healthData == null)
        {
            Debug.LogError("HealthData is not assigned in the inspector.");
            return;
        }

        RedLimit = healthData.initialHealth;
        RedCount = RedLimit - 1;
        HealthCount = RedCount;

        heartItem = Resources.Load<GameObject>("UI/Heart");
        for (int i = 0; i < HealthCount; i++)
        {
            // TODO: 这部分逻辑需要抽出
            GameObject heart = GameObject.Instantiate(heartItem);
            heart.transform.SetParent(transform, false);
            Image sr = heart.GetComponent<Image>();
            if (i < RedLimit)
            {
                sr.sprite = healthData.redHeart;
                sr.color = new Color(255, 255, 255, 1);
            }
            heartList.AddLast(heart);
        }
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
