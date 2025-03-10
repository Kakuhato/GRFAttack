using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public float maxHp;
    public float nowHp;
    public float moveSpeed;
    public float attackSpeed;
    public float attackRange;
    public float attackCd;
    
    public virtual void Move() { }
    
    public virtual void Attack() { }
    
    public virtual void GetDamage(float damage)
    {
        nowHp -= damage;
        if (nowHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
    
}
