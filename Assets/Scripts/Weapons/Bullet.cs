using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = System.Numerics.Vector3;

public class Bullet : MonoBehaviour, IVisitor
{
    public Rigidbody2D rb;
    [SerializeField] private float speed = 1;
    [SerializeField] private float range = 10;
    [SerializeField] private float damage = 1.5f;
    [SerializeField] private IPoolable poolable;


    private Vector2 direction;
    private IVisitor iVisitorImplementation;

    private void Awake()
    {
        poolable = GetComponent<IPoolable>();
    }


    protected void OnEnable()
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

    public void SetDirection(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (rb != null)
        {
            rb.velocity = transform.right * speed;
            direction = rb.velocity.normalized;
        }
    }

    public Vector2 GetDirection()
    {
        return direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") && this.gameObject.CompareTag("PlayerBullet")) ||
            (other.CompareTag("Player") && this.gameObject.CompareTag("EnemyBullet")) ||
            other.CompareTag("ForeGround")
           )
        {
            other.GetComponent<IVisitable>()?.Accept(this);
            poolable.Dispose();
        }
    }

    IEnumerator MyCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        poolable.Dispose();
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is Entity entity)
        {
            // TODO: 冲击力需要注入
            entity.TakeDamage(damage, rb.velocity.normalized * 15f);
        }
    }
}