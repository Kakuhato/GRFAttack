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

    [SerializeField] Rigidbody2D rb;

    private Camera mainCamera;
    private FireController fireController;
    private float aimDirection;

    private float horizontalMove;
    private float verticalMove;
    private Vector2 moveInput;

    private bool isMainController = false;
    private bool isFollowing = false;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        fireController = weapon.GetComponent<FireController>();
        entity = GetComponent<Entity>();
        animanationController = GetComponent<AnimationController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMainController)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            verticalMove = Input.GetAxisRaw("Vertical");
            moveInput.x = horizontalMove;
            moveInput.y = verticalMove;
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
        if (!isMainController) return;
        Move();
    }


    public void Move()
    {
        rb.velocity = entity.Stats.Speed * moveInput.normalized;
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
        else if (distance <= 0.01f)
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
            print("?");
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

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("ForeGround"))
        {
            isFollowing = false;
        }
    }
}