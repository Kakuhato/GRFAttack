using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            print(Stats.ToString());
        }
    }
}