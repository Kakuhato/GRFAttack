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
    [SpineAnimation] public string die;
    [SpineBone] public string boneName;

    private Vector2 mousePos;
    private Camera mainCamera;
    private Bone cross;
    public TrackEntry CurrentEntry;
    private float lastDirection = 1f;
    private float lastScaleX = 1f;

    private bool isDead = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (sa == null)
        {
            sa = GetComponent<SkeletonAnimation>();
        }
    }

    private void Start()
    {
        if (!sa.valid)
            sa.Initialize(false);

        mainCamera = Camera.main;
        cross = sa.Skeleton.FindBone(boneName);
        // CurrentEntry = sa.AnimationState.SetAnimation(0, idle, true);
        // PlayAnimation(idle, true);
        // isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || sa == null || sa.Skeleton == null) return;

        Aim();

        if (CurrentEntry != null && CurrentEntry.TimeScale < 0 && CurrentEntry.TrackTime <= 0f)
        {
            CurrentEntry.TrackTime = CurrentEntry.AnimationEnd;
        }
    }

    private void PlayAnimation(string animationName, bool loop)
    {
        if (sa == null || sa.AnimationState == null) return;
        CurrentEntry = sa.AnimationState.SetAnimation(0, animationName, loop);
        // Debug.Log($"Playing animation: {CurrentEntry.Animation.Name}");

        if (CurrentEntry == null || CurrentEntry.Animation == null)
        {
            Debug.LogError($"Animation {animationName} not found in the skeleton.");
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
        if (isDead) return;

        if (CurrentEntry == null || CurrentEntry.Animation == null || CurrentEntry.Animation.Name != idle)
        {
            PlayAnimation(idle, true);
        }
    }

    public void Walk(float direction)
    {
        if (isDead) return;

        if (CurrentEntry.Animation != null && CurrentEntry.Animation.Name == walk && direction * lastDirection >= 0 &&
            lastScaleX * sa.Skeleton.ScaleX >= 0) return;
        lastDirection = direction >= 0 ? 1f : -1f;
        lastScaleX = sa.Skeleton.ScaleX;
        PlayAnimation(walk, true);

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
        if (isDead) return;
        isDead = true;

        PlayAnimation(die, false);

        sa.transform.localPosition = new Vector3(1.5f * sa.Skeleton.ScaleX, -0.5f, 0);
    }

    public void Revive()
    {
        isDead = false;
        PlayAnimation(idle, true);
        sa.transform.localPosition = new Vector3(0, -0.5f, 0);
    }
}