using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    
    public float atk;
    public float maxHP;
    public float nowHP;
    public float moveSpeed;
    public float attackSpeed;
    
    public virtual void Move()
    {
        // Move the enemy
    }

    public virtual void Die()
    {
        Destroy( this.gameObject );
    }
    
}
