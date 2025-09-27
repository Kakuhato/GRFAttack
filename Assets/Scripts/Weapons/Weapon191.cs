using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using BulletData = Utils.BulletData;

public class Weapon191 : WeaponBase
{
    public RoundPosition rp = RoundPosition.Top;
    public override RoundPosition RP => rp;

    public override void SetHold()
    {
        isHold = true;

        // print(this.transform.position);
    }

    public override void ResetParent(Transform parent)
    {
        this.transform.SetParent(parent);
        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        this.transform.localPosition = Tool.GetRoundPosition(this.RP);
    }

    public override void Fire(float direction, float speed, float range)
    {
        if (isHold)
        {
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