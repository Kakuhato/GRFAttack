using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class BulletManager
{
    // Start is called before the first frame update
    private GameObject bulletPrefab;
    private string tag;

    public BulletManager(GameObject prefab, string tag)
    {
        this.bulletPrefab = prefab;
        this.tag = tag;
    }

    public void SpawnBullet(Vector3 position, float attack, float direction, float speed, float range)
    {
        GameObject bullet = PoolManager.Instance.SpawnObject(this.bulletPrefab, position, PoolType.Bullet);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetAttack(attack);
        bulletScript.SetSpeed(speed);
        bulletScript.SetRange(range);
        bulletScript.SetDirection(direction);
        bullet.tag = this.tag;
        bullet.SetActive(true);
    }
}