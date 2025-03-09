using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public Rigidbody2D rb;
    private float speed = 1;
    private float range = 10;
    
    void OnEnable()
    {
        if (rb != null)
        {
            rb.velocity = transform.right * speed;
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
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
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
