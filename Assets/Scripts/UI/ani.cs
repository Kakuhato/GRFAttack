using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Spine;
using UnityEngine;

public class ani : MonoBehaviour
{
    public SkeletonAnimation sa;
    private Rigidbody2D rb;

    [SpineAnimation] public string walk;
    [SpineAnimation] public string attack;
    [SpineBone] public string boneName;
    [SpineSlot] public string slotName;
    [SpineAttachment] public string attachmentName;

    private Vector2 mousePos;
    private Camera mainCamera;
    private Bone cross;

    // Start is called before the first frame update
    void Start()
    {
        sa.state.SetAnimation(0, walk, true);
        mainCamera = Camera.main;
        cross = sa.Skeleton.FindBone(boneName);
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector3 skeletonSpacePoint = sa.transform.InverseTransformPoint(worldMousePosition);
        skeletonSpacePoint.x *= sa.Skeleton.ScaleX;
        skeletonSpacePoint.y *= sa.Skeleton.ScaleY;
        cross.SetLocalPosition(skeletonSpacePoint);
        
        
    }
}
