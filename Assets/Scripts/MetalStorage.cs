using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalStorage : StorageStructure
{
    // Start is called before the first frame update
    void Start()
    {
        InitialiseStructure();
    }

    public override void UpdatePlayerList()
    {
        if (!playerController.MetalStorages.Contains(this.GetComponent<MetalStorage>()))//if player does not already have this metal storage within its list
        {
            playerController.MetalStorages.Add(this.GetComponent<MetalStorage>());//add this storage to the players list
        }

    }

    public override void DestroyAndRemoveFromPlayer()
    {
        playerController.metalCap -= capacity;
        if (playerController.metal >= playerController.metalCap)
        {
            playerController.metal -= capacity;
        }
        playerController.MetalStorages.Remove(this.GetComponent<MetalStorage>());
        base.DestroyAndRemoveFromPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
        CheckAttacked();
    }

}
