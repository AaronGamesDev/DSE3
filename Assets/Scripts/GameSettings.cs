using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{

    //Static variables are referenced in other scripts to act as game settings
    //Variables prefixed with "inspector" are assigned to their static variable counterpart but allow for visual representation within Unity's inspector
    public bool inspectorNeedNodePermission = true;
    public static bool needNodePermission;//Determines whether metalExtractors need a node to produce Metal
    public float inspectorMinMetal = 1.0f, inspectorMaxMetal = 5.0f;
    public static float minMetal, maxMetal;//Controls the range in which metalExtractors' resourceAmount is determined from
    public bool inspectorUnlimitedResources;
    public static bool unlimitedResources;//Used within playerController to set Metal and Power Resources to a fixed amount regardless of any structures producing resources.

    private void Update()
    {
        needNodePermission = inspectorNeedNodePermission;
        minMetal = inspectorMinMetal;
        maxMetal = inspectorMaxMetal;
        unlimitedResources = inspectorUnlimitedResources;

    }

}
