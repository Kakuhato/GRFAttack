using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Entity entity;
    public AnimationController animanationController;

    public Transform crosshair;
    public Transform weapon;

    [SerializeField] Rigidbody2D rb;

    private Camera mainCamera;
    private FireController fireController;
    private float aimDirection;

    private float horizontalMove;
    private float verticalMove;

    private Vector2 moveIput;

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
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        moveIput.x = horizontalMove;
        moveIput.y = verticalMove;

        aimDirection = Target();

        crosshair.rotation = Quaternion.Euler(0, 0, aimDirection);
        weapon.rotation = Quaternion.Euler(0, 0, aimDirection);
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (Input.GetMouseButtonDown(1))
        {
            print("!");
            AOE();
        }

        fireController.DrawLines(new Vector3(entity.Stats.ShootRange, 0));
    }

    private void FixedUpdate()
    {
        Move();
    }


    public void Move()
    {
        // this.transform.Translate(horizontalMove * entity.Stats.Speed * Time.deltaTime * Vector3.right);
        // this.transform.Translate(verticalMove * entity.Stats.Speed * Time.deltaTime * Vector3.up);
        rb.velocity = entity.Stats.Speed * moveIput.normalized;
        // rb.MovePosition(rb.position + entity.Stats.Speed * Time.fixedDeltaTime * moveIput.normalized);
        if (moveIput.magnitude > 0) animanationController.Walk(horizontalMove);
        // if (horizontalMove != 0 || verticalMove != 0) animanationController.Walk(horizontalMove);
        else animanationController.Idle();
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

    // public void TakeDamage(int damage)
    // {
    //     {
    //         #region Temeporary code! Needs modification
    //
    //         // 临时写法！ 需要修改
    //         GamePanel gamePanel = UIManager.Instance.GetPanel<GamePanel>();
    //         gamePanel.healthBar.RemoveHealth();
    //
    //         #endregion
    //     }
    // }
    //
    // public void Die()
    // {
    //     GameManager.Instance.GameOver();
    // }

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Reward"))
        {
            WeaponBase we = other.GetComponent<WeaponBase>();
            we.ResetParent(this.weapon);
            fireController.AddWeapon(we);
        }
    }
}