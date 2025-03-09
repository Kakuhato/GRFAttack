using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletData = Utils.BulletData;

public class Weapon191 : WeaponBase
{
    public override void SetHold()
    {
        isHold = true;
        
        print(this.transform.position);
    }
    
    public override void SetParent(Transform parent)
    {
        this.transform.parent = parent;
        this.transform.position = parent.position;
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        this.transform.localPosition = new Vector3(0, 2f, 0);
    }
    
    public override void Fire(Func<BulletData> func, float direction, float speed, float range)
    {
        if (isHold)
        {
            BulletData bullet = func();
            
            bullet.gameObject.transform.position = this.transform.position;
            bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, direction);
            bullet.bulletScript.SetSpeed(speed);
            bullet.bulletScript.SetRange(range);
            // bullet.gameObject.transform.rotation = this.transform.localRotation;
            bullet.gameObject.SetActive(true);
            
            BulletData bullet2 = func();
            bullet2.gameObject.transform.position = this.transform.position;
            bullet2.gameObject.transform.rotation = Quaternion.Euler(0, 0, direction + 180);
            bullet2.bulletScript.SetSpeed(speed * 2);
            bullet2.bulletScript.SetRange(range);
            bullet2.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetHold();
        }
    }
}
