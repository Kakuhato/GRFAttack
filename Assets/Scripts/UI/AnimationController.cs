using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Spine;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public SkeletonAnimation sa;

    [SpineAnimation] public string walk;
    [SpineAnimation] public string idle;
    [SpineBone] public string boneName;

    private Vector2 mousePos;
    private Camera mainCamera;
    private Bone cross;
    private TrackEntry currentEntry;
    private float lastDirection = 1f;
    private float lastScaleX = 1f;


    // Start is called before the first frame update
    void Start()
    {
        // sa.state.SetAnimation(0, walk, true);
        mainCamera = Camera.main;
        cross = sa.Skeleton.FindBone(boneName);
        currentEntry = sa.AnimationState.SetAnimation(0, idle, true);
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        if (currentEntry.TimeScale < 0 && currentEntry.TrackTime <= 0f)
        {
            currentEntry.TrackTime = currentEntry.AnimationEnd;
        }
    }

    void Aim()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector3 skeletonSpacePoint = sa.transform.InverseTransformPoint(worldMousePosition);
        skeletonSpacePoint.x *= sa.Skeleton.ScaleX;
        skeletonSpacePoint.y *= sa.Skeleton.ScaleY;
        cross.SetLocalPosition(skeletonSpacePoint);

        if (worldMousePosition.x < sa.transform.position.x)
        {
            sa.Skeleton.ScaleX = -1;
        }
        else
        {
            sa.Skeleton.ScaleX = 1;
        }
    }

    public void Idle()
    {
        if (currentEntry.Animation.Name == idle) return;
        currentEntry = sa.AnimationState.SetAnimation(0, idle, true);
    }

    public void Walk(float direction)
    {
        if (currentEntry.Animation.Name == walk && direction * lastDirection >= 0 &&
            lastScaleX * sa.Skeleton.ScaleX >= 0) return;
        lastDirection = direction >= 0 ? 1f : -1f;
        lastScaleX = sa.Skeleton.ScaleX;
        currentEntry = sa.AnimationState.SetAnimation(0, walk, true);
        if (direction * sa.Skeleton.ScaleX < 0f)
        {
            currentEntry.TrackTime = currentEntry.AnimationEnd;
            currentEntry.TimeScale = -1;
        }
        else
        {
            currentEntry.TrackTime = 0f;
            currentEntry.TimeScale = 1;
        }
    }
}