using System;
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
    public TrackEntry CurrentEntry;
    private float lastDirection = 1f;
    private float lastScaleX = 1f;

    private bool isDead = true;

    // Start is called before the first frame update
    void Awake()
    {
        // sa.state.SetAnimation(0, walk, true);
        mainCamera = Camera.main;
        cross = sa.Skeleton.FindBone(boneName);
        CurrentEntry = sa.AnimationState.SetAnimation(0, idle, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        Aim();
        if (CurrentEntry.TimeScale < 0 && CurrentEntry.TrackTime <= 0f)
        {
            CurrentEntry.TrackTime = CurrentEntry.AnimationEnd;
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
        if (CurrentEntry.Animation != null && CurrentEntry.Animation.Name == idle) return;
        CurrentEntry = sa.AnimationState.SetAnimation(0, idle, true);
        isDead = false;
        sa.transform.localPosition = new Vector3(0, -0.5f, 0);
    }

    public void Walk(float direction)
    {
        if (CurrentEntry.Animation != null && CurrentEntry.Animation.Name == walk && direction * lastDirection >= 0 &&
            lastScaleX * sa.Skeleton.ScaleX >= 0) return;
        lastDirection = direction >= 0 ? 1f : -1f;
        lastScaleX = sa.Skeleton.ScaleX;
        CurrentEntry = sa.AnimationState.SetAnimation(0, walk, true);
        sa.transform.localPosition = new Vector3(0, -0.5f, 0); // TODO： 做成委托

        if (direction * sa.Skeleton.ScaleX < 0f)
        {
            CurrentEntry.TrackTime = CurrentEntry.AnimationEnd;
            CurrentEntry.TimeScale = -1;
        }
        else
        {
            CurrentEntry.TrackTime = 0f;
            CurrentEntry.TimeScale = 1;
        }
    }

    public void Dead()
    {
        Debug.Log("1: " + CurrentEntry);
        Debug.Log("2: " + CurrentEntry.Animation);
        Debug.Log("3: " + CurrentEntry);


        if (CurrentEntry.Animation != null && CurrentEntry.Animation.Name == "die") return;
        sa.AnimationState.SetAnimation(0, "die", false);
        isDead = true;
        sa.transform.localPosition = new Vector3(1.5f * sa.Skeleton.ScaleX, -0.5f, 0);
    }
}