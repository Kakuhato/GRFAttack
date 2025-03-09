using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletData = Utils.BulletData;

public class WeaponBase : MonoBehaviour
{
    public bool isHold = false;
    
    public virtual void SetHold() {}
    
    public virtual void SetParent(Transform parent) {}
    
    public virtual void Fire(Func<BulletData> func, float direction, float speed, float range) {}
}
