using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStructure : Structure
{
    public float resourceAmount;
    public float generationRate = 1f;
    public float multiplier = 1f;

    public virtual void GenerateResource()
    {

    }

    public IEnumerator GenerateRate()
    {
        yield return new WaitForSeconds(generationRate);
        GenerateResource();
        StartCoroutine(GenerateRate());
    }
}
