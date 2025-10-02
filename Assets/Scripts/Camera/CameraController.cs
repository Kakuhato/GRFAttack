using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerPartyManager player;
    public Vector2 minBoundary;
    public Vector2 maxBoundary;

    public float smoothTime = 0.3f;
    private Camera mainCamera;
    private float halfHeight;
    private float halfWidth;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        halfHeight = mainCamera.orthographicSize;
        halfWidth = halfHeight * mainCamera.aspect;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayer(player.CameraFocusPoint);
    }

    public void FollowPlayer(Transform player)
    {
        Vector3 targetPosition = player.position;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minBoundary.x + halfWidth, maxBoundary.x - halfWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minBoundary.y + halfHeight, maxBoundary.y - halfHeight);

        targetPosition = Vector3.Lerp(this.transform.position, targetPosition, smoothTime);
        this.transform.position = new Vector3(targetPosition.x, targetPosition.y, this.transform.position.z);
    }
}