using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Spine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chaser : Entity, IVisitor
{
    [SerializeField] private float dragRation;
    [SerializeField] private bool debugMode = false;

    [SerializeField] private EnemyAnimation enemyAnimation;
    [SerializeField] DebugDrawer debugDrawer;


    private ChaseAI chaseAI;
    private IPoolable poolable;
    private bool isKnockback = false;
    private bool isDead = false;


    private Transform target;
    private float originHelth = 2f;
    private float curhealth = 2f;
    private Vector3 lastPosition;


    private Rigidbody2D rb;
    private Collider2D col;

    private int foregroundLayerMask;


    protected override void Awake()
    {
        base.Awake();
        poolable = GetComponent<IPoolable>();
        col = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        isDead = false;
        col.enabled = true;
    }


    void Start()
    {
        foregroundLayerMask = LayerMask.GetMask("Foreground");
        rb = GetComponent<Rigidbody2D>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        chaseAI = new ChaseAI(this.transform, rb, enemyAnimation, Stats.Speed, dragRation);

        debugDrawer = GetComponent<DebugDrawer>();
    }

    protected override void Update()
    {
        base.Update();
        chaseAI.Tick();
        if (debugMode) debugDrawer.DrawDebug(chaseAI.GetWayPoints(), foregroundLayerMask);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        chaseAI.FixedTick(isKnockback);

        if (isKnockback && rb.velocity.magnitude < 1.5f)
        {
            isKnockback = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IVisitable>()?.Accept(this);
        }
    }

    public void Visit<T>(T visitable) where T : Component, IVisitable
    {
        if (visitable is PlayerStats playerStats)
        {
            playerStats.TakeDamage(Stats.Attack, Vector2.zero);
            Die();
        }
    }

    public override void TakeDamage(float damage, Vector2 direction)
    {
        curhealth -= damage;
        if (curhealth <= 0)
        {
            Die();
        }
        else
        {
            KickBack(direction);
            isKnockback = true;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        col.enabled = false;

        rb.velocity = Vector2.zero;

        enemyAnimation.Die().Complete += OnDeathAnimationComplete;
    }

    private void OnDeathAnimationComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == "die3")
        {
            trackEntry.Complete -= OnDeathAnimationComplete;

            curhealth = originHelth;

            Debug.Log("Chaser Die" + Time.time);

            EventBus<EnemyDieEvent>.Raise(new EnemyDieEvent
                { ScoreGained = (int)originHelth, Position = this.transform.position });

            poolable.Dispose();
        }
    }

    public override List<int> GetHealthInfo()
    {
        return new List<int> { (int)originHelth };
    }

    public override void KickBack(Vector2 direction)
    {
        rb.AddForce(direction, ForceMode2D.Impulse);
    }
}