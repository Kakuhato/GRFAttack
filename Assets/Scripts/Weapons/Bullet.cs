using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Bullet : MonoBehaviour
{
    
    public Rigidbody2D rb;
    private float speed = 1;
    private float range = 10;
    
    private Vector2 direction;
    
    void OnEnable()
    {
        if (rb != null)
        {
            rb.velocity = transform.right * speed;
            direction = rb.velocity.normalized;
        }
        StartCoroutine(MyCoroutine(range / speed));
    }
    
    public void SetSpeed(float s)
    {
        this.speed = s;
    }
    
    public void SetRange(float r)
    {
        this.range = r;
    }
    
    public Vector2 GetDirection()
    {
        return direction;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") && this.gameObject.CompareTag("PlayerBullet")) ||
            (other.CompareTag("Player") && this.gameObject.CompareTag("EnemyBullet")))
        {
            this.gameObject.SetActive(false);
        }
    }
    
    IEnumerator MyCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.gameObject.SetActive(false);
    }
    
    
    
}
