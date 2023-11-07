using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageStructure : Structure
{
    public float capacity;
    public bool capAdded = false;

    public override void ApplyTier1Stats()
    {
        maxHp = 150f;
        capacity = 500f;
        base.ApplyTier1Stats();
    }

    public override void ApplyTier2Stats()
    {
        maxHp = 300f;
        capacity = 1500f;
        capAdded = false;
        base.ApplyTier2Stats();
    }

}
