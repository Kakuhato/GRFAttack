using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using BulletData = Utils.BulletData;

public class WeaponBase : MonoBehaviour
{
    public bool isHold = false;

    public virtual RoundPosition RP{get; set;}
    
    public virtual void SetHold() {}
    
    public virtual void ResetParent(Transform parent) {}
    
    public virtual void Fire(float direction, float speed, float range) {}
    
    
}
