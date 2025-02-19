
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static void Sort(this Transform transform)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.GetComponent<needFind>() != null)
            {
                Debug.Log(obj.GetComponent<needFind>().myInt);
            }
        }
    }
}
