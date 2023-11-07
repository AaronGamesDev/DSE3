using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStorage : StorageStructure
{

    // Start is called before the first frame update
    void Start()
    {
        InitialiseStructure();
    }

    public override void UpdatePlayerList()
    {
        if (!playerController.PowerStorages.Contains(this.GetComponent<PowerStorage>()))//if player does not already have this power storage within its list
        {
            playerController.PowerStorages.Add(this.GetComponent<PowerStorage>());//add this storage to the players list
        }

    }

    public override void DestroyAndRemoveFromPlayer()
    {
        playerController.powerCap -= capacity;
        if (playerController.power >= playerController.powerCap)
        {
            playerController.power -= capacity;
        }
        playerController.PowerStorages.Remove(this.GetComponent<PowerStorage>());
        base.DestroyAndRemoveFromPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
        CheckAttacked();
    }
}
