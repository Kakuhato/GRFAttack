using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChosenDoll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Required] public SkeletonGraphic skeletonGraphic;
    [Required] public Toggle toggle;
    private bool isChosen = false;

    public event Action OnChosen = delegate { };
    
    private void Start()
    {
        if(skeletonGraphic == null)
        {
            skeletonGraphic = GetComponent<SkeletonGraphic>();
        }
        skeletonGraphic.AnimationState.SetAnimation( 0, "wait", true);
        if(toggle == null)
        {
            toggle = GetComponent<Toggle>();
            if (toggle == null)
            {
                toggle = gameObject.AddComponent<Toggle>();
                toggle.group = this.transform.parent.GetComponent<ToggleGroup>();
            }
        }
        
        if (toggle.isOn)
        {
            skeletonGraphic.AnimationState.SetAnimation(0, "move", true);
        }
        
        toggle.onValueChanged.AddListener( isOn =>
        {
            if (isOn)
            {
                skeletonGraphic.AnimationState.SetAnimation( 0, "victory", false);
                skeletonGraphic.AnimationState.AddAnimation( 0, "victoryloop", true, 0);
            }
            else
            {
                skeletonGraphic.AnimationState.SetAnimation( 0, "wait", true);
            }
        });
        
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!toggle.isOn)
        {
            skeletonGraphic.AnimationState.SetAnimation(0, "move", true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!toggle.isOn)
        {
            skeletonGraphic.AnimationState.SetAnimation( 0, "wait", true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isChosen = !isChosen;
        toggle.isOn = isChosen;
        OnChosen.Invoke();
    }

    public void check()
    {
        print("checked!");
    }
    
    
}
