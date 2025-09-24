using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum HealthType
{
    Red,
    Soul
}


[CreateAssetMenu(fileName = "HealthPatten", menuName = "ScriptableObjects/HealthData")]
public class HealthPatten : ScriptableObject
{
    [SerializeField] private Sprite redHeart;
    [SerializeField] private Sprite soulHeart;

    public GameObject CreateRed(GameObject heartPrefab, Transform parent)
    {
        GameObject heart = CreatHeart(heartPrefab, parent, this.redHeart);
        heart.transform.SetAsFirstSibling();
        return heart;
    }

    public GameObject CreateSoul(GameObject heartPrefab, Transform parent)
    {
        return CreatHeart(heartPrefab, parent, this.soulHeart);
    }

    private GameObject CreatHeart(GameObject heartPrefab, Transform parent, Sprite sprite)
    {
        GameObject heart = GameObject.Instantiate(heartPrefab, parent);

        Image sr = heart.GetComponent<Image>();
        sr.sprite = sprite;
        sr.color = new Color(255, 255, 255, 1);
        return heart;
    }
}