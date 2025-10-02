using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartyManager : MonoBehaviour
{
    public GameObject playerPrefab;

    [SerializeField] private List<GameObject> partyMembers = new List<GameObject>();
    [SerializeField] private int currentMemberIndex = 0;
}