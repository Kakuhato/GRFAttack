using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ToggleChange : MonoBehaviour
{
    // Start is called before the first frame update
    
    public EventTrigger eventTrigger;
    public RectTransform rectTransform;
    public Image img;

    void Start()
    {
        img.alphaHitTestMinimumThreshold = 0.1f;
    }
}


