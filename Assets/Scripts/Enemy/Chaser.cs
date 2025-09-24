using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chaser : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

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
    private float health;
    private Vector3 lastPosition;


    private Rigidbody2D rb;
    private float originalDrag;

    private int foregroundLayerMask;
    private int playerLayer;
    private bool getOutPointExist = false;

    private float stuckTime = -1; // TODO: 换成计时器

    void Start()
    {
        foregroundLayerMask = LayerMask.GetMask("Foreground");
        playerLayer = LayerMask.NameToLayer("Player");
        rb = GetComponent<Rigidbody2D>();
        originalDrag = rb.drag;
    }

    void Update()
    {
        if (debugMode) DrawDebug();
        Chase();
    }

    private void FixedUpdate()
    {
        PhysicChase();
    }

    void PhysicChase()
    {
        if (wayPoints.Count() == 0)
        {
            rb.drag = 1f;
            return;
        }
        else
        {
            rb.drag = originalDrag;
        }

        rb.velocity = (wayPoints[0] - this.transform.position).normalized * speed;

        float distanceTraveled = Vector2.Distance(lastPosition, rb.position);
        lastPosition = rb.position;

        // TODO: 这里的卡死检测有问题，先默认不卡死，实际有卡死再进行修改
        if (distanceTraveled < Time.fixedDeltaTime * -1)
        {
            if (stuckTime < 0) stuckTime = Time.time;

            if (Time.time - stuckTime > 1f)
            {
                Vector2 randomCircle = Random.insideUnitCircle.normalized * 4;
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

        if (wayPoints.Count() > 10) wayPoints.Clear();

        if (wayPoints.Count == 0) return;

        if (Vector2.Distance(this.transform.position, wayPoints[0]) < 1f)
        {
            wayPoints.RemoveAt(0);
            getOutPointExist = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Vector2 direction = other.GetComponent<Bullet>().GetDirection();
            this.transform.position += new Vector3(direction.x, direction.y, 0) * 0.5f;
            // base.GetDamage(1f);
        }
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