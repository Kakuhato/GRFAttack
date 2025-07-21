using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.ShowPanel<BeginPanel>();
        AudioManager.Instance.PlayBackGroundMusic("Audio/Cyborg");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
