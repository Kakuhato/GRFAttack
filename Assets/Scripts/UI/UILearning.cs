using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;
using Object = UnityEngine.Object;

public class UILearning : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public Tilemap tilemap;
    public TileBase tileBase;
    public Grid grid;
    
    public Animation animation;

    public Animator animator;
    // public SpriteResolver spriteResolver;
    private Dictionary<string, SpriteResolver> equipments = new Dictionary<string, SpriteResolver>();
    public void Test()
    {
        SpriteResolver[] spriteResolvers = this.GetComponentsInChildren<SpriteResolver>();
        foreach (var spriteResolver in spriteResolvers)
        {
            equipments.Add(spriteResolver.GetCategory(), spriteResolver);
        }
    }
    
    public void ChangeEquip(string category, string id)
    {
        if (equipments.ContainsKey(category))
        {
            equipments[category].SetCategoryAndLabel(category, id);
        }
    }
    
    



}
