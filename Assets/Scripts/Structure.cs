using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public GameObject player;
    public PlayerController playerController;

    public bool tier1 = true;
    public bool tier2 = false;
    public float hp = 150f;
    public float maxHp = 150f;
    public float regenSpeed = 3f;
    public float regenAmount = 2f;
    public float regenWaitTime = 5f;
    public bool attacked = false;
    public bool appliedStats = false;
    public bool regenStarted = false;
    public bool waiting = false;

    private void OnEnable()
    {
        GlobalPlayerObjectsList.GlobalList.Add(gameObject);//Adds to Global List when structure is made
    }

    private void OnDisable()
    {
        GlobalPlayerObjectsList.GlobalList.Remove(gameObject);//Removes from Global List when structure is destroyed
    }

    /// <summary>
    /// Sets the player and playerController, calls UpdateStats() which should apply their current tiers stats if not applied before,
    /// then finally calls UpdatePlayerList() which adds it to the player's corresponding List of its Structure Type.
    /// </summary>
    public void InitialiseStructure()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        playerController = player.GetComponent<PlayerController>();
        UpdateStats();
        UpdatePlayerList();
    }

    public virtual void UpdatePlayerList()
    {

    }

    public void UpdateStats()
    {
        if (tier1 && !appliedStats)
        {
            ApplyTier1Stats();
        }
        else if (tier2 && !appliedStats)
        {
            ApplyTier2Stats();
        }

        if (hp <= 0)
        {
            DestroyAndRemoveFromPlayer();
        }
    }

    /// <summary>
    /// This applies base tier 1 stats across all structures, including hp = maxHp, regenAmount, regenSpeed, and then sets appliedStats to equal true.
    /// Since it sets hp to equal maxHp, when overriding make sure to set maxHp before calling the base function
    /// </summary>
    public virtual void ApplyTier1Stats()
    {
        hp = maxHp;
        regenAmount = 2f;
        regenSpeed = 3f;
        appliedStats = true;
    }

    /// <summary>
    /// This applies base tier 2 stats across all structures, including hp = maxHp, regenAmount, regenSpeed, and then sets appliedStats to equal true.
    /// Since it sets hp to equal maxHp, when overriding make sure to set maxHp before calling the base function
    /// </summary>
    public virtual void ApplyTier2Stats()
    {
        hp = maxHp;
        regenAmount = 5f;
        regenSpeed = 2f;
        appliedStats = true;
    }

    public virtual void DestroyAndRemoveFromPlayer()
    {
        Destroy(gameObject);
    }

    public void Upgrade()
    {
        if (tier1 && appliedStats)
        {
            tier2 = true;
            tier1 = false;
            appliedStats = false;
        }
    }

    /// <summary>
    /// Used to apply damage to all structure types, passes a float value representing damage which is applied to the structures hp, the structure's attacked status is also 
    /// set to true (which is needed to start regen).
    /// </summary>
    /// <param name="dmg"></param>
    public void ReceiveDmg(float dmg)//public ReceiveDmg function to make it simpler in the enemybot script to apply damage to objects related to structures
    {
        hp -= dmg;
        attacked = true;
    }

    /// <summary>
    /// Checks if attacked status is true and if not already waiting, then starts the wait before beginning regen process
    /// </summary>
    public void CheckAttacked()//check if attacked and if so then starts the wait before beginning regen process
    {
        if (attacked && !waiting)//if attacked and not already waiting, start waiting coroutine
        {
            StartCoroutine(WaitBeforeRegen());
        }
    }

    private IEnumerator WaitBeforeRegen()//sets attacked to false and waits, before checking attacked status again which determines whether or not to begin regeneration
    {
        if (attacked && !regenStarted)//if attacked is true and regenStarted is false
        {
            attacked = false;//set attacked to false
            waiting = true;//set waiting to true
            yield return new WaitForSeconds(regenWaitTime);//wait for set time
            waiting = false;//set waiting to false as its done waiting
            if (!attacked && !regenStarted)//if attacked is still false (has not been attacked again within the wait time) and regenStarted is still false, start RegenRate
            {
                StartCoroutine(RegenRate());
            }
        }
        else
        {
            yield return null;
        }
        
    }

    private void Regen()
    {
        if (hp < maxHp)
        {
            hp += regenAmount;
        }

        if (hp >= maxHp)
        {
            hp = maxHp;
        }
    }

    private IEnumerator RegenRate()//checks hp and attacked status before proceeding with regen function, waiting, and then starting the coroutine again to continue regerating at its set rate.
    {
        
        if (hp < maxHp && !attacked)//if hp is not full and is not currently being attacked, Regen(), wait for regenSpeed, then StartCoroutine again
        {
            regenStarted = true;
            Regen();
            yield return new WaitForSeconds(regenSpeed);
            StartCoroutine(RegenRate());
        }
        else //when hp is full or over maxHp, regardless of being attacked, call Regen to cap health, then set regenStarted to false
        {
            Regen();
            regenStarted = false;
            yield return null;
        }
        
    }
}
