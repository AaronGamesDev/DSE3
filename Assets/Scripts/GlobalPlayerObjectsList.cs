using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerObjectsList : MonoBehaviour
{
    public static List<GameObject> GlobalList = new List<GameObject>();
    public List<GameObject> inspectorGlobalList = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        inspectorGlobalList = GlobalList;
    }
}
