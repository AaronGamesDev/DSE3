using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarPanel : ResourceStructure
{

    // Start is called before the first frame update
    void Start()
    {
        InitialiseStructure();
        StartCoroutine(GenerateRate());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateStats();
        CheckAttacked();
        
    }

    public override void UpdatePlayerList()
    {
        if (!playerController.SolarPanels.Contains(this.GetComponent<SolarPanel>()))//if the player does not have this solar panel in its list
        {
            playerController.SolarPanels.Add(this.GetComponent<SolarPanel>());
        }
    }

    public override void ApplyTier1Stats()
    {
        maxHp = 250f;
        multiplier = 1f;
        resourceAmount = 7f;
        base.ApplyTier1Stats();
    }

    public override void ApplyTier2Stats()
    {
        maxHp = 500f;
        multiplier = 2.5f;
        resourceAmount = 7f;
        base.ApplyTier2Stats();
    }

    public override void DestroyAndRemoveFromPlayer()
    {
        playerController.SolarPanels.Remove(this.GetComponent<SolarPanel>());
        base.DestroyAndRemoveFromPlayer();
    }

    public override void GenerateResource()
    {

        if (playerController.power < playerController.powerCap)
        {
            playerController.power += resourceAmount * multiplier;
            //Debug.Log("Generating Power");
        }
        else
        {
            playerController.power = playerController.powerCap;
            playerController.overflowM = true;
            Debug.Log("Overflowing Power!");
        }
    }
}
