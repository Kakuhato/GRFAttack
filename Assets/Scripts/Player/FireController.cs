using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletData = Utils.BulletData;
public class FireController : MonoBehaviour
{
    
    private List<WeaponBase> weapons = new List<WeaponBase>();
    
    public void Start()
    {

    }

    public void Fire(Vector3 position, float direction, float speed, float range)
    {
        BulletData bullet = BulletsPool.Instance.GetBullet("PlayerBullet");
        bullet.gameObject.transform.position = position;
        bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, direction);
        bullet.bulletScript.SetSpeed(speed);
        bullet.bulletScript.SetRange(range);
        bullet.gameObject.SetActive(true);
        foreach(WeaponBase weapon in weapons)
        {
            weapon.Fire(direction, speed, range);
        }
    }
    
    
    public void AddWeapon(WeaponBase weapon)
    {
        weapons.Add(weapon);
    }

}
