using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class PartySorting : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = this.gameObject.GetOrAdd<MeshRenderer>();
    }

    private void LateUpdate()
    {
        meshRenderer.sortingOrder = -(int)(this.transform.position.y * 100);
    }
}