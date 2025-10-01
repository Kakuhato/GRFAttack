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

    // void PhysicChase()
    // {
    //     if (wayPoints.Count() == 0)
    //     {
    //         rb.drag = 1f;
    //         enemyAnimation.Idle();
    //         return;
    //     }
    //     else
    //     {
    //         enemyAnimation.Walk();
    //         rb.drag = originalDrag;
    //     }
    //
    //     rb.AddForce((wayPoints[0] - this.transform.position).normalized * 10f);
    //
    //     if (rb.velocity.magnitude > Stats.Speed)
    //     {
    //         rb.velocity = rb.velocity.normalized * Stats.Speed;
    //     }
    //
    //     // rb.velocity = (wayPoints[0] - this.transform.position).normalized * speed;
    //     enemyAnimation.ChangeDirection(-(wayPoints[0] - this.transform.position).normalized);
    //
    //     float distanceTraveled = Vector2.Distance(lastPosition, rb.position);
    //     lastPosition = rb.position;
    //
    //     if (distanceTraveled < Time.fixedDeltaTime)
    //     {
    //         if (stuckTime < 0) stuckTime = Time.time;
    //
    //         if (Time.time - stuckTime > 0.3f)
    //         {
    //             Vector2 randomCircle = Random.insideUnitCircle.normalized * 1.5f;
    //             Vector3 getOutPosition = rb.position + randomCircle;
    //             getOutPosition = new Vector3(getOutPosition.x, getOutPosition.y, 0);
    //
    //             if (!Physics2D.OverlapCircle(getOutPosition, 0.5f, foregroundLayerMask))
    //             {
    //                 if (!getOutPointExist)
    //                 {
    //                     wayPoints.Insert(0, getOutPosition);
    //
    //                     getOutPointExist = true;
    //                 }
    //                 else
    //                 {
    //                     // 上次尝试脱离卡死失败
    //                     wayPoints[0] = getOutPosition;
    //                 }
    //
    //                 stuckTime = -1;
    //             }
    //         }
    //     }
    // }

    // void Chase()
    // {
    //     target = GameManager.Instance.PlayerTransform;
    //     if (target == null) return;
    //     float distance = Vector2.Distance(this.transform.position, TargetPosition);
    //     Vector2 direction = (TargetPosition - this.transform.position).normalized;
    //
    //     if (!Physics2D.CircleCast((Vector2)this.transform.position, 0.2f, direction, distance, foregroundLayerMask))
    //     {
    //         wayPoints.Clear();
    //         wayPoints.Add(TargetPosition);
    //     }
    //
    //
    //     if (wayPoints.Count > 0 && (Vector2.Distance(wayPoints[wayPoints.Count - 1], TargetPosition) > 1f))
    //     {
    //         wayPoints.Add(TargetPosition);
    //     }
    //
    //     if (wayPoints.Count() > 80) wayPoints.Clear();
    //
    //     if (wayPoints.Count == 0) return;
    //
    //     if (Vector2.Distance(this.transform.position, wayPoints[0]) < 1f)
    //     {
    //         wayPoints.RemoveAt(0);
    //         getOutPointExist = false;
    //     }
    // }


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
            EventBus<ScoreEvent>.Raise(new ScoreEvent { ScoreGained = (int)originHelth });

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