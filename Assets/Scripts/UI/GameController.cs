// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class GameController : MonoBehaviour
// {
//     public GameObject player;
//     
//     private static GameController _instance;
//     public static GameController Instance
//     {
//         get
//         {
//             if (_instance == null)
//             {
//                 _instance = FindAnyObjectByType<GameController>();
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
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }
//     
//     
//     public Vector3 GetPlayerPosition()
//     {
//         return player.transform.position;
//     }
//     
//     
// }

