// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using BulletData = Utils.BulletData;
//
// public class BulletsPool : MonoBehaviour
// {
//     public GameObject bulletPrefab;
//     private int poolSize = 0;
//     private List<BulletData> bulletPool;
//     private List<WeaponBase> weapons = new List<WeaponBase>();
//
//     private static BulletsPool _instance;
//
//     public static BulletsPool Instance
//     {
//         get
//         {
//             if (_instance == null)
//             {
//                 _instance = FindObjectOfType<BulletsPool>();
//                 // 其他脚本访问这个属性时，可能这个脚本的Awake()还没有运行，因此_instance可能仍然为null
//                 if (_instance == null)
//                 {
//                     GameObject obj = new GameObject("BulletsPool");
//                     _instance = obj.AddComponent<BulletsPool>();
//                 }
//             }
//
//             return _instance;
//         }
//     }
//
//     private void Awake()
//     {
//         if (_instance == null)
//         {
//             _instance = this;
//             // DontDestroyOnLoad(gameObject); // 确保在场景切换时不被销毁
//         }
//         else
//         {
//             Destroy(gameObject); // 如果已存在实例，销毁新的实例
//         }
//     }
//
//     public void Start()
//     {
//         bulletPool = new List<BulletData>();
//         for (int i = 0; i < poolSize; i++)
//         {
//             bulletPool.Add(CreateBullet());
//         }
//     }
//
//     private BulletData CreateBullet()
//     {
//         GameObject newBullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity, this.transform);
//         // newBullet.tag = "PlayerBullet";
//         newBullet.SetActive(false);
//         Bullet newBulletScript = newBullet.GetComponent<Bullet>();
//         BulletData newBulletData = new BulletData
//         {
//             gameObject = newBullet,
//             bulletScript = newBulletScript
//         };
//         return newBulletData;
//     }
//
//     public BulletData GetBullet(string t)
//     {
//         foreach (BulletData bullet in bulletPool)
//         {
//             if (!bullet.gameObject.activeInHierarchy)
//             {
//                 bullet.gameObject.tag = t;
//                 return bullet;
//             }
//         }
//
//         BulletData newBulletData = CreateBullet();
//         bulletPool.Add(newBulletData);
//         return newBulletData;
//     }
// }

