using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildProgression : MonoBehaviour
{
    public Material dissolveMat;
    public Material[] materials;
    public float buildTime = 15f;
    public float flashTime = 0.5f;
    public GameObject finalObj;
    public bool setup = false;

    public void SetValues(GameObject replacement, float Time)
    {
        buildTime = Time - (flashTime * 4);
        finalObj = replacement;

        setup = true;
    }


    // Start is called before the first frame update
    void Start()
    {
        if (setup)
        {
            StartCoroutine(buildTimer());
        }
    }

    public IEnumerator buildTimer()
    {
        float elapsedTime = 0;

        materials = GetComponent<MeshRenderer>().materials;
        foreach (Material mat in materials)
        {
            
            if (mat.name == "Dissolve Material (Instance)")
            {
                dissolveMat = mat;
            }
        }

        while (elapsedTime < buildTime)
        {
            elapsedTime += Time.deltaTime;
            //print(elapsedTime);

            dissolveMat.SetFloat("_dissolveStrength", Mathf.Lerp(1, 0, elapsedTime/buildTime));

            yield return null;
        }

        dissolveMat.SetFloat("_dissolveVisibility", 0f);

        //=============================================================================================================================

        elapsedTime = 0;
        
        while(elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            //print(elapsedTime);

            dissolveMat.SetFloat("_flashVisibility", Mathf.Lerp(0, 1, elapsedTime / flashTime));

            yield return null;
        }

        elapsedTime = 0;
        
        while(elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            //print(elapsedTime);

            dissolveMat.SetFloat("_flashStrength", Mathf.Lerp(1, 0, elapsedTime / flashTime));

            yield return null;
        }
        
        elapsedTime = 0;
        
        while(elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            //print(elapsedTime);

            dissolveMat.SetFloat("_flashStrength", Mathf.Lerp(0, 1, elapsedTime / flashTime));

            yield return null;
        }
        
        elapsedTime = 0;
        
        while(elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            //print(elapsedTime);

            dissolveMat.SetFloat("_flashVisibility", Mathf.Lerp(1, 0, elapsedTime / flashTime));

            yield return null;
        }

        Instantiate<GameObject>(finalObj, gameObject.transform.position, gameObject.transform.rotation);//instantiate finalObj to replace ghostObj
        Destroy(gameObject);//destroy itself

    }

}
