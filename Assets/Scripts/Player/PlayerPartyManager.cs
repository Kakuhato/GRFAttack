using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerPartyManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject dummyPrefab;

    [Header("Party Settings")] [SerializeField]
    private int maxPartySize = 5;

    [SerializeField] private List<GameObject> partyMembers = new List<GameObject>(5);
    [SerializeField] private int currentMemberIndex = 0;

    public Transform CameraFocusPoint => partyMembers[currentMemberIndex].transform;

    private Vector3 lastMainPosition;
    private Vector3 lastMoveDirection;

    private void Start()
    {
        SetMainController(0);
        lastMainPosition = partyMembers[currentMemberIndex].transform.position;
        lastMoveDirection = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetMainController((currentMemberIndex + 1) % partyMembers.Count);
        }

        Vector3[] triangleOffsets = new Vector3[]
        {
            Vector3.zero,
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0),
            new Vector3(-2, -2, 0),
            new Vector3(2, -2, 0)
        };

        Vector3 mainPosition = partyMembers[currentMemberIndex].transform.position;
        Vector3 moveDirection = mainPosition - lastMainPosition;
        if (moveDirection.magnitude < 0.01f)
        {
            moveDirection = lastMoveDirection;
        }
        else
        {
            lastMoveDirection = moveDirection;
        }

        lastMainPosition = mainPosition;


        int offsetIndex = 1;
        for (int i = 0; i < partyMembers.Count; i++)
        {
            if (i == currentMemberIndex) continue;

            var pc = partyMembers[i].GetComponent<PlayerController>();

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, lastMoveDirection.normalized);
            Vector3 offset = rotation * triangleOffsets[offsetIndex++];

            pc.Follow(partyMembers[currentMemberIndex].transform.position + offset,
                partyMembers[currentMemberIndex].transform.position, 1.5f);
        }
    }

    public void SetMainController(int index)
    {
        if (index < 0 || index >= partyMembers.Count)
        {
            Debug.LogWarning("Index out of range");
            return;
        }

        partyMembers[currentMemberIndex].GetComponent<PlayerController>().SetMainController(false);
        currentMemberIndex = index;
        partyMembers[currentMemberIndex].GetComponent<PlayerController>().SetMainController(true);
    }
}