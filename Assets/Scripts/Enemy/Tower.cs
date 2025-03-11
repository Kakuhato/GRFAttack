using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Tower : EnemyBase
{
    public override void Move()
    {
        // Move the enemy
        this.transform.Rotate(Vector3.forward, Time.deltaTime * this.moveSpeed, Space.Self);
    }
    
    private bool IsPlayerInRange()
    {
        return Vector3.Distance(GameController.Instance.GetPlayerPosition(), this.transform.position) < this.attackRange;
    }
    
    private float RotateTowardsPlayer()
    {
        Vector2 direction = GameController.Instance.GetPlayerPosition() - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f * Time.deltaTime);
        return (angle - transform.eulerAngles.z - 90f) % 90f;
    }

    public override void Attack()
    {
        this.attackCd += Time.deltaTime;
        if (this.attackCd >= this.attackSpeed)
        {
            // print("Attack");
            for(int i = 0; i < 2; i++)
            {
                BulletData bullet = BulletsPool.Instance.GetBullet("EnemyBullet");
                bullet.gameObject.transform.position = this.transform.TransformPoint(4f * (0.5f - i) * Vector3.up  );
                bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, this.transform.eulerAngles.z + 90 + 180 * i);
                bullet.bulletScript.SetSpeed(2f);
                bullet.bulletScript.SetRange(this.attackRange);
                bullet.gameObject.SetActive(true);
            }
            this.attackCd = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.attackSpeed = 1f;
        this.moveSpeed = 20f;
        this.attackRange = 5f;
        this.attackCd = 0f;
        this.maxHp = 10f;
        this.nowHp = this.maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInRange())
        {
            if (Mathf.Abs(RotateTowardsPlayer()) < 30)
            {
                Attack();
            }
        }
        else
        {
            Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            // Vector2 direction = other.GetComponent<Bullet>().GetDirection();
            // this.transform.position += new Vector3(direction.x, direction.y, 0) * 0.5f;
            base.GetDamage(1f);
        }
    }
}
