using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilitySpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> availableSpawnPoints = new();
    [SerializeField] private List<Transform> usedSpawnPoints = new();

    [SerializeField] private List<AbilityPickup> prefabList = new();
    AbilityPickup pickupToSpawn;

    [SerializeField] private PlayerLoadoutSO loadout;

    private void Start()
    {
        for (int i = 0; i < prefabList.Count; i++)
        {
            if (loadout.AbilityType == prefabList[i].Type)
            {
                pickupToSpawn = prefabList[i];
            }
        }

        InvokeRepeating(nameof(SpawnAbilityPickup), 1, 1);
    }

    private void SpawnAbilityPickup()
    {
        if (availableSpawnPoints.Count == 0)
        {
            CancelInvoke();
            return;
        }
        int random = Random.Range(0, availableSpawnPoints.Count);
        Instantiate(pickupToSpawn, availableSpawnPoints[random]);

        availableSpawnPoints.RemoveAt(random);
        usedSpawnPoints.Add(availableSpawnPoints[random]);
    }
}
