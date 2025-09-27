using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using BulletData = Utils.BulletData;

public class FireController : MonoBehaviour
{
    [SerializeField, Required] private GameObject bulletPrefab;

    private List<WeaponBase> weapons = new List<WeaponBase>();
    private BulletManager bulletManager;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        bulletManager = new BulletManager(bulletPrefab, "PlayerBullet");
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();
    }


    public void Fire(Vector3 position, float direction, float speed, float range)
    {
        bulletManager.SpawnBullet(position, direction, speed, range);
        // foreach(WeaponBase weapon in weapons)
        // {
        //     weapon.Fire(direction, speed, range);
        // }
    }


    public void AddWeapon(WeaponBase weapon)
    {
        weapons.Add(weapon);
    }

    public void DrawLines(Vector3 end)
    {
        lineRenderer.SetPosition(1, end);
    }
}