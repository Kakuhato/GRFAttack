using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChaseAI
{
    private Rigidbody2D rb;
    private EnemyAnimation enemyAnimation;
    private float dragRation;

    private Transform self;
    private Transform target;
    private float speed;
    private List<Vector3> wayPoints = new List<Vector3>();
    private int foregroundLayerMask;
    private float originalDrag;
    private Vector3 lastPosition;

    private float stuckTime = -1; // TODO: 换成计时器

    public List<Vector3> GetWayPoints() => wayPoints;


    public ChaseAI(Transform self, Rigidbody2D rb, EnemyAnimation enemyAnimation, float speed, float dragRation)
    {
        this.self = self;
        this.rb = rb;
        this.speed = speed;
        this.enemyAnimation = enemyAnimation;
        this.dragRation = dragRation;
        foregroundLayerMask = LayerMask.GetMask("Foreground");
        originalDrag = rb.drag;
    }

    public void Tick()
    {
        target = GameManager.Instance.PlayerTransform;
        if (target == null) return;
        Chase();
    }

    public void FixedTick(bool isKnockBack)
    {
        if (!isKnockBack)
            PhysicChase();
        else
        {
            if (rb.velocity.magnitude > 1f)
            {
                Drag();
            }
        }
    }

    private void Chase()
    {
        target = GameManager.Instance.PlayerTransform;
        if (target == null) return;
        float distance = Vector2.Distance(self.position, target.position);
        Vector2 direction = (target.position - self.position).normalized;
        if (!Physics2D.CircleCast((Vector2)self.position, 0.2f, direction, distance, foregroundLayerMask))
        {
            wayPoints.Clear();
            wayPoints.Add(target.position);
        }

        if (wayPoints.Count > 0 && (Vector2.Distance(wayPoints[wayPoints.Count - 1], target.position) > 1f))
        {
            wayPoints.Add(target.position);
        }

        if (wayPoints.Count() > 80) wayPoints.Clear();

        if (wayPoints.Count == 0) return;

        if (Vector2.Distance(self.position, wayPoints[0]) < 1f)
        {
            wayPoints.RemoveAt(0);
        }
    }

    private void PhysicChase()
    {
        if (wayPoints.Count() == 0)
        {
            rb.drag = 1f;
            enemyAnimation.Idle();
            return;
        }
        else
        {
            enemyAnimation.Walk();
            rb.drag = originalDrag;
        }

        rb.AddForce((wayPoints[0] - self.position).normalized * 10f);

        if (rb.velocity.magnitude > this.speed)
        {
            rb.velocity = rb.velocity.normalized * this.speed;
        }

        // rb.velocity = (wayPoints[0] - this.transform.position).normalized * speed;
        enemyAnimation.ChangeDirection(-(wayPoints[0] - self.position).normalized);

        float distanceTraveled = Vector2.Distance(lastPosition, rb.position);
        lastPosition = rb.position;

        if (distanceTraveled < Time.fixedDeltaTime)
        {
            if (stuckTime < 0) stuckTime = Time.time;

            if (Time.time - stuckTime > 0.3f)
            {
                Vector2 randomCircle = Random.insideUnitCircle.normalized * 1.5f;
                Vector3 getOutPosition = rb.position + randomCircle;

                if (!Physics2D.OverlapCircle(getOutPosition, 0.5f, foregroundLayerMask))
                {
                    if (wayPoints.Count == 0)
                    {
                        wayPoints.Insert(0, getOutPosition);
                    }
                    else
                    {
                        wayPoints[0] = getOutPosition;
                    }

                    stuckTime = -1;
                }
            }
        }
    }

    private void Drag()
    {
        Vector2 v = rb.velocity;
        rb.AddForce(-dragRation * rb.mass * v);
    }
}