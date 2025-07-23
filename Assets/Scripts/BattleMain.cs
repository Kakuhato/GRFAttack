using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.InitBattle();
    }

    // Update is called once per frame
    void Update()
    {
    }
}