using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletData = Utils.BulletData;
public class FireController : MonoBehaviour
{
    public GameObject bulletPrefab;
    
    private int poolSize = 20;
    private List<BulletData> bulletPool; 
    
    private List<WeaponBase> weapons = new List<WeaponBase>();

    
    public void Start()
    {
        bulletPool = new List<BulletData>();
        for(int i = 0; i < poolSize; i++)
        {
            bulletPool.Add(CreateBullet());
        }
    }

    public void Fire(Vector3 position, float direction, float speed, float range)
    {
        BulletData bullet = GetBullet();
        bullet.gameObject.transform.position = position;
        bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, direction);
        bullet.bulletScript.SetSpeed(speed);
        bullet.bulletScript.SetRange(range);
        bullet.gameObject.SetActive(true);
        foreach(WeaponBase weapon in weapons)
        {
            weapon.Fire(GetBullet, direction, speed, range);
        }
        
    }
    
    
    private BulletData CreateBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
        newBullet.tag = "PlayerBullet";
        newBullet.SetActive(false);
        
        Bullet newBulletScript = newBullet.GetComponent<Bullet>();
        BulletData newBulletData = new BulletData
        {
            gameObject = newBullet,
            bulletScript = newBulletScript
        };
        return newBulletData;
    }
    
    private BulletData GetBullet()
    {
        foreach(BulletData bullet in bulletPool)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                return bullet;
            }
        }
        
        BulletData newBulletData = CreateBullet();
        bulletPool.Add(newBulletData);
        return newBulletData;
    }
    
    public void AddWeapon(WeaponBase weapon)
    {
        weapons.Add(weapon);
    }

}
