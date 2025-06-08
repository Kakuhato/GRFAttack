using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float atk = 10f;
    public int maxHp = 10;
    public int nowHp = 10;

    public float moveSpeed = 10;
    public float attackSpeed = 1;
    public float bulletSpeed = 10;
    public float attackRange = 2;
    
    public Transform crosshair;
    public Transform weapon;
    
    private Camera mainCamera;
    private FireController fireController;
    private float horizontalMove;
    

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        fireController = weapon.GetComponent<FireController>();

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        crosshair.rotation = Quaternion.Euler(0, 0, Target());
        weapon.rotation = Quaternion.Euler(0, 0, Target());
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        if (Input.GetMouseButtonDown(1))
        {
            print("!");
            AOE();
        }
    }



    public void Move()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        this.transform.Translate( horizontalMove * moveSpeed * Time.deltaTime * Vector3.right);
        this.transform.Translate(Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * Vector3.up);
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
        fireController.Fire(this.transform.position, Target(), this.bulletSpeed, this.attackRange);
    }
    
    public void TakeDamage(int damage)
    {
        this.nowHp -= damage;
        if (this.nowHp <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        Destroy(this.gameObject);
    }

    public void AOE()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            this.transform.position, 
            this.attackRange,
            1 << LayerMask.NameToLayer("Enemy")
            );
        if(colliders.Length > 0)
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this.attackRange);
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
