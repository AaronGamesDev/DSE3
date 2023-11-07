using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerObjectsList : MonoBehaviour
{
    public static List<GameObject> GlobalList = new List<GameObject>();
    public List<GameObject> inspectorGlobalList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGlobalList();
    }

    void UpdateGlobalList()
    {
        List<GameObject> tempObjList = new List<GameObject>();
        tempObjList.AddRange(FindObjectsOfType<GameObject>());

        foreach (GameObject obj in tempObjList)
        {
            if (obj.GetComponent<DisplayTeam>() != null && obj.GetComponent<DisplayTeam>().teamName == "Player" && !GlobalList.Contains(obj))
            {
                GlobalList.Add(obj);
            }
        }

        for (int i = 0; i < GlobalList.Count; i++)
        {
            if (GlobalList[i] == null)
            {
                GlobalList.RemoveAt(i);
            }
        }


        inspectorGlobalList = GlobalList;
    }
}
