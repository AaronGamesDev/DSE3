using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUsingStructure : Structure
{
    public bool online = false;
    public bool paused = false;
    public bool usingPower = false;
    public float powerUsage;
    public float consumptionRate = 1f;



    public IEnumerator ConsumptionRate()
    {
        yield return new WaitForSeconds(consumptionRate);
        if (online)
        {
            playerController.power -= powerUsage;
            usingPower = true;
        }
        else
        {
            usingPower = false;
        }
        StartCoroutine(ConsumptionRate());
    }

    public virtual void SetOnline()
    {
        if (CheckPower() && !online)//if has enough power and is not already online, set online to true
        {
            online = true;
        }
        else if (!CheckPower() && online)// if doesn't have enough power and is already online, set online to false
        {
            online = false;
        }
    }



    public bool CheckPower()
    {
        if (paused)
        {
            return false;
        }
        else
        {
            if (playerController.power >= powerUsage)
            {
               return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void TogglePaused()
    {
        paused = !paused;
    }
}
