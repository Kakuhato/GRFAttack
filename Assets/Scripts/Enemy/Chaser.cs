using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : EnemyBase
{

    public override void Move()
    {
        // Move the enemy
        this.transform.Translate(Time.deltaTime * this.moveSpeed * (GameController.Instance.GetPlayerPosition() - this.transform.position));
    }
    
    private bool IsPlayerInRange()
    {
        return Vector3.Distance(GameController.Instance.GetPlayerPosition(), this.transform.position) < this.attackRange;
    }

    
    void Start()
    {
        this.attackSpeed = 1f;
        this.moveSpeed = 0.3f;
        this.attackRange = 5f;
        this.attackCd = 0f;
        this.maxHp = 10f;
        this.nowHp = this.maxHp;
    }
    
    
    void Update()
    {
        if (IsPlayerInRange())
        {
            Move();
        }
    }
    
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Vector2 direction = other.GetComponent<Bullet>().GetDirection();
            this.transform.position += new Vector3(direction.x, direction.y, 0) * 0.5f;
            base.GetDamage(1f);
        }
    }
}
