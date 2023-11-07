using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalNode : MonoBehaviour
{
    public float metalRate;
    public List<GameObject> extractors = new List<GameObject>();
    public bool nearExtractor = false;
    // Start is called before the first frame update
    void Start()
    {
        metalRate = Random.Range(1.0f, 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        GivePermission();
        SetMetalRate();
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("something detected");
        if (other.transform.tag == "Metal Extractor")
        {
            nearExtractor = true;
            //Debug.Log("Object name:" + other.gameObject.name);
            if (!CheckExtractors(other.gameObject))//if its NOT within the existing list of extractors
            {
                //Debug.Log("Object name:" + other.gameObject.name + " was added to list");
                extractors.Add(other.gameObject);//add to the list
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("something detected");
        if (other.transform.tag == "Metal Extractor")
        {
            nearExtractor = false;
            if (CheckExtractors(other.gameObject))//if it IS within the existing list of extractors
            {
                if (extractors[0] == other.gameObject)//if its the first in list and therefore the extractor with node permission
                {
                    RemovePermission();
                }
                extractors.Remove(other.gameObject);//remove from the list
            }
        }
    }

    bool CheckExtractors(GameObject obj)
    {
        for (int i = 0; i < extractors.Count; i++)
        {
            if (extractors[i] == obj)//if obj is already in list
            {
                return true;
            }

        }
        return false;
    }

    void GivePermission()
    {
        for (int i = 0; i < extractors.Count; i++)
        {
            if (i == 0)
            {
                extractors[i].GetComponent<MetalExtractor>().ReceivePermision(true);
                extractors[i].GetComponent<MetalExtractor>().ReceiveNode(gameObject);
            }
            else
            {
                extractors[i].GetComponent<MetalExtractor>().ReceivePermision(false);
                extractors[i].GetComponent<MetalExtractor>().ReceiveNode(gameObject);
            }

        }
    }

    void RemovePermission()
    {
        extractors[0].GetComponent<MetalExtractor>().ReceivePermision(false);
    }

    void SetMetalRate()
    {
        for (int i = 0; i < extractors.Count; i++)
        {
            extractors[i].GetComponent<MetalExtractor>().ReceiveRate(metalRate);
        }
    }
}
