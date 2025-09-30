using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Utils
{
    public struct BulletData
    {
        public GameObject gameObject;
        public Bullet bulletScript;
    }

    public enum PoolType
    {
        None,
        Bullet,
        Enemy,
    }

    public enum RoundPosition
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public static class Tool
    {
        private const float Offset = 1.5f;

        public static Vector3 GetRoundPosition(RoundPosition rp)
        {
            Vector3 pos = Vector3.zero;
            switch (rp)
            {
                case RoundPosition.Top:
                    pos = new Vector3(0, Offset, 0);
                    break;
                case RoundPosition.Bottom:
                    pos = new Vector3(0, -Offset, 0);
                    break;
                case RoundPosition.Left:
                    pos = new Vector3(-Offset, 0, 0);
                    break;
                case RoundPosition.Right:
                    pos = new Vector3(Offset, 0, 0);
                    break;
            }

            return pos;
        }

        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            return component != null ? component : gameObject.AddComponent<T>();
        }
    }
}