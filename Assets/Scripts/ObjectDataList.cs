using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDataList : MonoBehaviour
{
    public GameObject[] objects;
    public Vector3[,] offsets;
    public float[] buildTimes;
    public float[] metalCost, powerCost;
    public float[] powerConsumption;

    // Start is called before the first frame update
    void Start()
    {
        SetOffsets();
    }

    void SetOffsets()
    {
        if (objects != null)
        {
            offsets = new Vector3[objects.Length, 3];
            buildTimes = new float[objects.Length];
            metalCost = new float[objects.Length];
            powerCost = new float[objects.Length];
            powerConsumption = new float[objects.Length];
            for (int i  = 0; i < objects.Length; i++)
            {
                if (objects[i].name == "Ghost Metal Extractor" || objects[i].name == "Metal Extractor")
                {
                    offsets[i, 0] = new Vector3(0, 0.2f, 0);
                    offsets[i, 1] = new Vector3(2f, 0, 0);
                    offsets[i, 2] = new Vector3(0, 0, 2f);
                    buildTimes[i] = 10f;
                    metalCost[i] = 200f;
                    powerCost[i] = 250f;

                }
                else if (objects[i].name == "Ghost Solar Panel" || objects[i].name == "Solar Panel")
                {
                    offsets[i, 0] = new Vector3(0, 0f, 0);
                    offsets[i, 1] = new Vector3(2.5f, 0, 0);
                    offsets[i, 2] = new Vector3(0, 0, 2.5f);
                    buildTimes[i] = 10f;
                    metalCost[i] = 100f;
                    powerCost[i] = 10f;
                }
                else if (objects[i].name == "Ghost Turret" || objects[i].name == "Turret")
                {
                    offsets[i, 0] = new Vector3(0, 0f, 0);
                    offsets[i, 1] = new Vector3(3f, 0, 0);
                    offsets[i, 2] = new Vector3(0, 0, 3f);
                    buildTimes[i] = 5f;
                    metalCost[i] = 150f;
                    powerCost[i] = 150f;
                }
                else if (objects[i].name == "Ghost MP Storage" || objects[i].name == "Metal Storage")
                {
                    offsets[i, 0] = new Vector3(0, 0f, 0);
                    offsets[i, 1] = new Vector3(2.5f, 0, 0);
                    offsets[i, 2] = new Vector3(0, 0, 2.5f);
                    buildTimes[i] = 15f;
                    metalCost[i] = 500f;
                    powerCost[i] = 200f;
                }
                else if (objects[i].name == "Ghost MP Storage" || objects[i].name == "Power Storage")
                {
                    offsets[i, 0] = new Vector3(0, 0f, 0);
                    offsets[i, 1] = new Vector3(2.5f, 0, 0);
                    offsets[i, 2] = new Vector3(0, 0, 2.5f);
                    buildTimes[i] = 15f;
                    metalCost[i] = 500f;
                    powerCost[i] = 200f;
                }
            }
        }
    }
}
