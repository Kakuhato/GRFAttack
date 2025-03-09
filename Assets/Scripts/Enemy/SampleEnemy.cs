using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SampleEnemy : EnemyBase
{
    
    public override void Move()
    {
        // Move the enemy
        
    }
    
    public override void Die()
    {
        Destroy( this.gameObject );
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Die();
        }
    }
}

