using System.Collections.Generic;
using UnityEngine;

public class DebugDrawer : MonoBehaviour
{
    [SerializeField] private Material debugMaterialOrange;
    [SerializeField] private Material debugMaterialGreen;
    [SerializeField] private Mesh debugMesh;
    private LineRenderer debugLine;
    private Material debugLineMaterialGreen;
    private Material debugLineMaterialOrange;

    public void DrawDebug(List<Vector3> wayPoints, int foregroundLayerMask)
    {
        if (debugLine == null)
        {
            debugLineMaterialOrange = new Material(debugMaterialOrange);
            debugLineMaterialOrange.color = new Color(debugLineMaterialOrange.color.r, debugLineMaterialOrange.color.g,
                debugLineMaterialOrange.color.b, 0.2f);
            debugLineMaterialGreen = new Material(debugMaterialGreen);
            debugLineMaterialGreen.color = new Color(debugLineMaterialGreen.color.r, debugLineMaterialGreen.color.g,
                debugLineMaterialGreen.color.b, 0.2f);
            debugLine = this.gameObject.AddComponent<LineRenderer>();
            debugLine.sortingLayerName = "Entity";
            debugLine.material = debugLineMaterialOrange;
            debugLine.startWidth = debugLine.endWidth = 0.3f;
        }

        debugLine.enabled = wayPoints.Count != 0;

        for (int i = 0; i < wayPoints.Count; i++)
        {
            Graphics.DrawMesh(debugMesh, wayPoints[i], Quaternion.identity,
                i == 0 ? debugMaterialGreen : debugMaterialOrange, 0);
            RaycastHit2D hit = Physics2D.CircleCast((Vector2)this.transform.position, 0.2f,
                (GameManager.Instance.PlayerTransform.position - this.transform.position).normalized,
                Vector2.Distance(this.transform.position, wayPoints[i]),
                foregroundLayerMask);
            if (hit)
            {
                debugLine.sharedMaterial = debugMaterialOrange;
                debugLine.SetPositions(new Vector3[] { this.transform.position, hit.point });
            }
            else
            {
                debugLine.sharedMaterial = debugMaterialGreen;
                debugLine.SetPositions(new Vector3[]
                    { this.transform.position, GameManager.Instance.PlayerTransform.position });
            }
        }
    }
}