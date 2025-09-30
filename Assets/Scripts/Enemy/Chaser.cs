using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chaser : Entity, IVisitor
{
    [SerializeField] private IPoolable poolable;
    [SerializeField] private EnemyAnimation enemyAnimation;

    [SerializeField] private float dragRation;

    private bool isKnockback = false;

    [SerializeField] private Material debugMaterialOrange;
    [SerializeField] private Material debugMaterialGreen;
    [SerializeField] private Mesh debugMesh;
    [SerializeField] private bool debugMode = false;
    private LineRenderer debugLine;
    private Material debugLineMaterialGreen;
    private Material debugLineMaterialOrange;

    private Vector3 TargetPosition => target.position;
    private List<Vector3> wayPoints = new List<Vector3>();

    private Transform target;
    private float originHelth = 2f;
    private float curhealth = 2f;
    private Vector3 lastPosition;


    private Rigidbody2D rb;
    private float originalDrag;

    private int foregroundLayerMask;
    private bool getOutPointExist = false;

    private float stuckTime = -1; // TODO: 换成计时器

    protected override void Awake()
    {
        base.Awake();
        poolable = GetComponent<IPoolable>();
    }

    void Start()
    {
        foregroundLayerMask = LayerMask.GetMask("Foreground");
        rb = GetComponent<Rigidbody2D>();
        originalDrag = rb.drag;
    }

    protected override void Update()
    {
        base.Update();
        if (debugMode) DrawDebug();
        Chase();
    }

    private void FixedUpdate()
    {
        if (!isKnockback)
        {
            PhysicChase();
        }
        else
        {
            if (rb.velocity.magnitude < 1.5f)
            {
                isKnockback = false;
            }
            else
            {
                Drag();
            }
        }
    }

    void PhysicChase()
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

        rb.AddForce((wayPoints[0] - this.transform.position).normalized * 10f);

        if (rb.velocity.magnitude > Stats.Speed)
        {
            rb.velocity = rb.velocity.normalized * Stats.Speed;
        }

        // rb.velocity = (wayPoints[0] - this.transform.position).normalized * speed;
        enemyAnimation.ChangeDirection(-(wayPoints[0] - this.transform.position).normalized);

        float distanceTraveled = Vector2.Distance(lastPosition, rb.position);
        lastPosition = rb.position;

        if (distanceTraveled < Time.fixedDeltaTime)
        {
            if (stuckTime < 0) stuckTime = Time.time;

            if (Time.time - stuckTime > 0.3f)
            {
                Vector2 randomCircle = Random.insideUnitCircle.normalized * 1.5f;
                Vector3 getOutPosition = rb.position + randomCircle;
                getOutPosition = new Vector3(getOutPosition.x, getOutPosition.y, 0);

                if (!Physics2D.OverlapCircle(getOutPosition, 0.5f, foregroundLayerMask))
                {
                    if (!getOutPointExist)
                    {
                        wayPoints.Insert(0, getOutPosition);

                        getOutPointExist = true;
                    }
                    else
                    {
                        // 上次尝试脱离卡死失败
                        wayPoints[0] = getOutPosition;
                    }

                    stuckTime = -1;
                }
            }
        }
    }

    void Chase()
    {
        target = GameManager.Instance.PlayerTransform;
        if (target == null) return;
        float distance = Vector2.Distance(this.transform.position, TargetPosition);
        Vector2 direction = (TargetPosition - this.transform.position).normalized;

        if (!Physics2D.CircleCast((Vector2)this.transform.position, 0.2f, direction, distance, foregroundLayerMask))
        {
            wayPoints.Clear();
            wayPoints.Add(TargetPosition);
        }


        if (wayPoints.Count > 0 && (Vector2.Distance(wayPoints[wayPoints.Count - 1], TargetPosition) > 1f))
        {
            wayPoints.Add(TargetPosition);
        }

        if (wayPoints.Count() > 80) wayPoints.Clear();

        if (wayPoints.Count == 0) return;

        if (Vector2.Distance(this.transform.position, wayPoints[0]) < 1f)
        {
            wayPoints.RemoveAt(0);
            getOutPointExist = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IVisitable>()?.Accept(this);
        }
    }

    private void Drag()
    {
        Vector2 v = rb.velocity;

        rb.AddForce(-dragRation * rb.mass * v);
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
            curhealth = originHelth;
            poolable.Dispose();
        }
        else
        {
            KickBack(direction);
            isKnockback = true;
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

    private void DrawDebug()
    {
        if (debugLine == null)
        {
            debugLineMaterialOrange = new Material(debugMaterialOrange);
            debugLineMaterialOrange.color = new Color(debugLineMaterialOrange.color.r, debugLineMaterialOrange.color.g,
                debugLineMaterialOrange.color.b, 0.2f);
            debugLineMaterialGreen = new Material(debugMaterialGreen);
            debugLineMaterialGreen.color = new Color(debugLineMaterialGreen.color.r, debugLineMaterialGreen.color.g,
                debugLineMaterialGreen.color.b, 0.2f);
            debugLine = this.gameObject.AddComponent<LineRenderer>();
            debugLine.sortingLayerName = "Entity";
            debugLine.material = debugLineMaterialOrange;
            debugLine.startWidth = debugLine.endWidth = 0.3f;
        }

        debugLine.enabled = wayPoints.Count() != 0;

        for (int i = 0; i < wayPoints.Count; i++)
        {
            Graphics.DrawMesh(debugMesh, wayPoints[i], Quaternion.identity,
                i == 0 ? debugMaterialGreen : debugMaterialOrange, 0);
            RaycastHit2D hit = Physics2D.CircleCast((Vector2)this.transform.position, 0.2f,
                (TargetPosition - this.transform.position).normalized,
                Vector2.Distance(this.transform.position, wayPoints[i]),
                foregroundLayerMask);
            if (hit)
            {
                debugLine.sharedMaterial = debugMaterialOrange;
                debugLine.SetPositions(new Vector3[] { this.transform.position, hit.point });
            }
            else
            {
                debugLine.sharedMaterial = debugMaterialGreen;
                debugLine.SetPositions(new Vector3[] { this.transform.position, TargetPosition });
            }
        }
    }
}