using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public SkeletonAnimation sa;
    [SpineAnimation] public string walk;
    [SpineAnimation] public string idle;


    private TrackEntry currentEntry;
    private float lastDirection = 1f;


    private void Awake()
    {
        currentEntry = sa.AnimationState.SetAnimation(0, idle, true);
    }

    public void Idle()
    {
        if (currentEntry.Animation.Name == idle) return;
        currentEntry = sa.AnimationState.SetAnimation(0, idle, true);
    }

    public void Walk()
    {
        if (currentEntry.Animation.Name == walk) return;
        currentEntry = sa.AnimationState.SetAnimation(0, walk, true);
    }

    public TrackEntry Die()
    {
        return currentEntry = sa.AnimationState.SetAnimation(0, "die3", false);
    }


    public void ChangeDirection(Vector2 direction)
    {
        if (direction.x * lastDirection >= 0) return;
        lastDirection = direction.x > 0 ? 1f : -1f;
        if (direction.x < 0)
        {
            sa.Skeleton.ScaleX = -1;
        }
        else if (direction.x > 0)
        {
            sa.Skeleton.ScaleX = 1;
        }
    }
}