using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exercise : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.GetComponent<needFind>().myInt);
        this.transform.Sort();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
