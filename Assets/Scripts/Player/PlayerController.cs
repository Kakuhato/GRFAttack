using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public Entity entity;
    public AnimationController animanationController;

    public Transform weapon;

    public bool IsDead => entity.IsDead;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerStatsChannel playerStatsChannel;

    private Camera mainCamera;
    private FireController fireController;
    private float aimDirection;

    private float horizontalMove;
    private float verticalMove;
    private Vector2 moveInput;

    private bool isMainController = false;
    private bool isFollowing = false;


    private void Awake()
    {
        mainCamera = Camera.main;
        fireController = weapon.GetComponent<FireController>();
        entity = GetComponent<Entity>();
        animanationController = GetComponent<AnimationController>();
        rb = GetComponent<Rigidbody2D>();
        this.fireController.gameObject.SetActive(false);

        entity.OnDeath += HandleDeath;
        entity.OnRevive += HandleRevive;
        entity.Die();
        // 之后变成prefab感觉这个Awake逻辑绝对会出问题
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(this.gameObject.name + IsDead);
        if (IsDead) return;

        if (isMainController)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            verticalMove = Input.GetAxisRaw("Vertical");
            moveInput.x = horizontalMove;
            moveInput.y = verticalMove;

            playerStatsChannel?.Invoke(entity.Stats.ToPublish());
        }

        aimDirection = Target();

        weapon.rotation = Quaternion.Euler(0, 0, aimDirection);
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        fireController.DrawLines(new Vector3(entity.Stats.ShootRange, 0));
    }

    private void FixedUpdate()
    {
        if (!isMainController || IsDead) return;
        Move();
    }


    public void Move()
    {
        rb.velocity = entity.Stats.Speed * moveInput.normalized;
        // Debug.Log(this.gameObject.name + " move: " + rb.velocity);
        if (moveInput.magnitude > 0) animanationController.Walk(horizontalMove);
        else animanationController.Idle();
    }

    public void Follow(Vector3 targetPosition, Vector3 mainPosition, float tolerantDistance = 1.5f)
    {
        Vector3 direction = targetPosition - this.transform.position;
        float lazyDistance = (mainPosition - this.transform.position).magnitude;
        float distance = direction.magnitude;

        if (lazyDistance > tolerantDistance * 1.5f)
        {
            isFollowing = true;
        }
        else if (distance <= 0.1f)
        {
            isFollowing = false;
        }

        if (isFollowing)
        {
            rb.velocity = entity.Stats.Speed * 0.8f * direction.normalized;
            animanationController.Walk(direction.x);
        }
        else
        {
            rb.velocity = Vector2.zero;
            // Debug.Log(this.gameObject.name + " stop follow.");
            animanationController.Idle();
        }
    }

    public float Target()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }

    public void Fire()
    {
        fireController.Fire(this.transform.position, entity.Stats.Attack, aimDirection, entity.Stats.ShootSpeed,
            entity.Stats.ShootRange);
    }

    public void AOE()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            this.transform.position,
            entity.Stats.ShootRange,
            1 << LayerMask.NameToLayer("Enemy")
        );
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<EnemyBase>().GetDamage(2f);
                }
            }
        }
    }

    public void SetMainController(bool isMain)
    {
        isMainController = isMain;
    }


    private void HandleDeath()
    {
        rb.velocity = Vector2.zero;
        animanationController.Dead();
        this.gameObject.layer = LayerMask.NameToLayer("DeadPlayer");
        this.fireController.gameObject.SetActive(false);
        // Debug.Log(this.gameObject.name + " is dead.");
    }

    private void HandleRevive()
    {
        animanationController.Revive();
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        this.fireController.gameObject.SetActive(true);
    }

    public void Revive()
    {
        entity.Revive();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("ForeGround"))
        {
            isFollowing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var sc = other.GetComponent<PlayerController>();
            if (sc != null && sc.IsDead)
            {
                sc.Revive();
            }
        }
    }
}