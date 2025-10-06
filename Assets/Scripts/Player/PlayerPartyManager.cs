using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerPartyManager : MonoBehaviour
{
    [Header("Party Settings")] [SerializeField]
    private int maxPartySize = 5;

    [SerializeField] private List<GameObject> partyMembers = new List<GameObject>();
    [SerializeField] private int currentMemberIndex = 0;

    public Transform CameraFocusPoint => partyMembers[currentMemberIndex].transform;

    private Vector3 lastMainPosition;
    private Vector3 lastMoveDirection;
    private PlayerController currentController;
    private bool isOver = false;

    private EventBinding<PlayerDieEvent> playerDieEventBinding;

    private void Awake()
    {
        playerDieEventBinding = new EventBinding<PlayerDieEvent>(OnPlayerDie);
        EventBus<PlayerDieEvent>.Register(playerDieEventBinding);
    }

    private void OnDestroy()
    {
        EventBus<PlayerDieEvent>.Unregister(playerDieEventBinding);
    }

    private void Start()
    {
        lastMainPosition = partyMembers[currentMemberIndex].transform.position;
        lastMoveDirection = Vector3.zero;

        if (partyMembers.Count == 0)
        {
            Debug.LogError("No party members assigned!");
            return;
        }

        currentController = partyMembers[currentMemberIndex].GetComponent<PlayerController>();
        this.SetMainController(0);
        currentController.Revive();
        // Debug.Log("Current Controller: " + currentController.gameObject.name + ", IsDead: " + currentController.IsDead);
    }

    private void Update()
    {
        if (currentController == null) return;
        // Debug.Log("Current Controller: " + currentController.gameObject.name + ", IsDead: " + currentController.IsDead);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TrySetMainController();
        }

        PartyMove();
    }


    public void PartyMove()
    {
        Vector3[] triangleOffsets = new Vector3[]
        {
            Vector3.zero,
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0),
            new Vector3(-2, -2, 0),
            new Vector3(2, -2, 0)
        };

        Vector3[] crossOffsets = new Vector3[]
        {
            Vector3.zero,
            new Vector3(0, -1.5f, 0),
            new Vector3(-1.5f, 0, 0),
            new Vector3(1.5f, 0, 0),
            new Vector3(0, 1.5f, 0)
        };

        // Debug.Log("Current Member Index: " + currentMemberIndex);
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
            // Debug.Log(pc.gameObject.name + " is dead: " + pc.IsDead);

            if (pc.IsDead) continue;

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, lastMoveDirection.normalized);
            Vector3 offset = rotation * crossOffsets[offsetIndex++];

            pc.Follow(partyMembers[currentMemberIndex].transform.position + offset,
                partyMembers[currentMemberIndex].transform.position, offset.magnitude);
        }
    }

    public bool TrySetMainController()
    {
        int nextIdx = (currentMemberIndex + 1) % partyMembers.Count;
        while (partyMembers[nextIdx].GetComponent<PlayerController>().IsDead)
        {
            nextIdx = (nextIdx + 1) % partyMembers.Count;
            if (nextIdx == currentMemberIndex) return false; // All are dead
        }

        this.SetMainController(nextIdx);
        List<int> newHealth = new List<int>(currentController.entity.GetHealthInfo());
        EventBus<FreshHealthEvent>.Raise(new FreshHealthEvent { red = newHealth[0], soul = newHealth[1] });
        return true;
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
        currentController = partyMembers[currentMemberIndex].GetComponent<PlayerController>();
        partyMembers[currentMemberIndex].GetComponent<PlayerController>().SetMainController(true);
    }

    private void OnPlayerDie(PlayerDieEvent e)
    {
        Debug.Log("PlayerPartyManager received PlayerDieEvent from: " + e.playerTransform.name);
        if (e.playerTransform == partyMembers[currentMemberIndex].transform)
        {
            Debug.Log("Current main player died. Trying to switch...");
            bool alive = TrySetMainController();
            if (!alive)
            {
                EventBus<GameOverEvent>.Raise(new GameOverEvent());
            }
        }
    }
}