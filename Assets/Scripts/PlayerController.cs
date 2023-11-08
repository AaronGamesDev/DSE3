using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject placeHolder;
    public GameObject finalObj;
    public RaycastHit hit;
    public Vector3 mousePos;
    public float dist = 300.0f;
    public Vector3 posOffset, gridOffsetX, gridOffsetZ;
    public Vector3 lastPos;
    public bool isShift = false;
    public float gridDist;
    public bool left = false;
    public bool right = false;
    public bool up = false;
    public bool down = false;
    public List<List<GameObject>> placeGrid = new List<List<GameObject>>();
    public float power = 1000f;
    public float powerCap = 1250f;
    public float metal = 1000f;
    public float metalCap = 1250f;
    public bool overflowM = false;
    public bool overflowP = false;
    public int btnTracker = 0;
    public bool objectSelected = false;
    public ObjectDataList objectDataList;
    public List<MetalExtractor> Extractors;
    public float incomingMetal = 0f;
    public float passiveMetal = 1f;
    public float passivePower = 2f;
    public List<SolarPanel> SolarPanels;
    public float incomingPower = 0f;
    public List<PowerStorage> PowerStorages;
    public List<MetalStorage> MetalStorages;
    public bool checkUI = false;
    public bool ghostCheck = false;
    public int collisiionCount = 0;
    public float buildTime;
    public bool togglePlaceMode = false;
    public float metalCost, powerCost;
    public float totalPConsumption;
    public List<Turret> Turrets;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        StartCoroutine(IncomeRate());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateIncome();
        SetObject();
        CheckPowerCapacity();
        CheckMetalCapacity();
        CalculateConsumptions();

        if (objectSelected)
        {
            CalculateOffsets();
            PlaceObjects();
        }

        if (GameSettings.unlimitedResources)
        {
            SetUnlimitedResources();
        }
        
    }

    void SetUnlimitedResources()
    {
        metalCap = 100000f;
        powerCap = 100000f;
        if(metal < metalCap)
        {
            metal = metalCap;
        }
        
        if (power < powerCap)
        {
            power = powerCap;
        }
    }

    void CalculateConsumptions()
    {
        float tempPTotal = 0;
        foreach (MetalExtractor met in Extractors)
        {
            if (!met.paused && met.online)
            {
                tempPTotal += met.powerUsage;
            }
        }
        foreach (Turret tur in Turrets)
        {
            if (!tur.paused && tur.online)
            {
                tempPTotal += tur.powerUsage;
        }
    }

        totalPConsumption = tempPTotal;

    }

    void CheckMetalCapacity()
    {
        foreach (MetalStorage mStor in MetalStorages)
        {
            if (mStor.capAdded == false)
            {
                metalCap += mStor.capacity;
                mStor.capAdded = true;
                Debug.Log("Capactity Added");

            }
        }
    }

    void CheckPowerCapacity()
    {
        foreach (PowerStorage pStor in PowerStorages)
        {
            if (pStor.capAdded == false)
            {
                powerCap += pStor.capacity;
                pStor.capAdded = true;
                Debug.Log("Capactity Added");

            }
        }
    }

    void CalculateIncome()
    {
        float extractorIncome = 0f;
        foreach (MetalExtractor metalEx in Extractors)
        {
            if (metalEx.nodePermission == true)
            {
                extractorIncome += metalEx.resourceAmount * metalEx.multiplier;
            }
        }
        incomingMetal = extractorIncome + passiveMetal;

        float solarIncome = 0f;
        foreach (SolarPanel solarPan in SolarPanels)
        {
            solarIncome += solarPan.resourceAmount * solarPan.multiplier;
        }
        incomingPower = solarIncome + passivePower;
    }



    void PassiveIncome()
    {
        power += passivePower;
        metal += passiveMetal;
    }

    IEnumerator IncomeRate()
    {
        yield return new WaitForSeconds(1);
        PassiveIncome();
        StartCoroutine(IncomeRate());
    }

    void SetObject()
    {
        if (togglePlaceMode)
        {
            if (placeHolder != null && placeHolder.activeSelf)
            {
                placeHolder.SetActive(false);
            }
        }
        

        switch (btnTracker)
        {
            case 0:
                objectSelected = false;
                finalObj = null;
                placeHolder = null;
                buildTime = 0;
                metalCost = 0;
                powerCost = 0;
                break;
            case 1:
                objectSelected = true;
                finalObj = objectDataList.objects[0];
                placeHolder = objectDataList.objects[1];
                buildTime = objectDataList.buildTimes[0];
                metalCost = objectDataList.metalCost[0];
                powerCost = objectDataList.powerCost[0];
                break;
            case 2:
                objectSelected = true;
                finalObj = objectDataList.objects[2];
                placeHolder = objectDataList.objects[3];
                buildTime = objectDataList.buildTimes[2];
                metalCost = objectDataList.metalCost[2];
                powerCost = objectDataList.powerCost[2];
                break;
            case 3:
                objectSelected = true;
                finalObj = objectDataList.objects[4];
                placeHolder = objectDataList.objects[5];
                buildTime = objectDataList.buildTimes[4];
                metalCost = objectDataList.metalCost[4];
                powerCost = objectDataList.powerCost[4];
                break;
            case 4:
                objectSelected = true;
                finalObj = objectDataList.objects[6];
                placeHolder = objectDataList.objects[7];
                buildTime = objectDataList.buildTimes[6];
                metalCost = objectDataList.metalCost[6];
                powerCost = objectDataList.powerCost[6];
                break;
            case 5:
                objectSelected = true;
                finalObj = objectDataList.objects[8];
                placeHolder = objectDataList.objects[9];
                buildTime = objectDataList.buildTimes[8];
                metalCost = objectDataList.metalCost[8];
                powerCost = objectDataList.powerCost[8];
                break;
        }

        if (togglePlaceMode)
        {
            if (placeHolder != null && !placeHolder.activeSelf)
            {
                placeHolder.SetActive(true);
            }
        }
    }

    public void SetBtnTracker(int button)
    {
        Debug.Log("Clicked");
        btnTracker = button;
        togglePlaceMode = true;
    }

    public void CheckMouseOnUI(bool check)
    {
        checkUI = check;
    }
    
    public void CheckGhostCollisions()
    {
        collisiionCount = 0;

        foreach (List<GameObject> ghostList in placeGrid)
        {
            foreach (GameObject ghost in ghostList)
            {
                if (ghost != null)//if gameobject isnt empty
                {
                    if (ghost.GetComponent<GhostCheck>().collisionDetected)//check if each ghost has detected a collision
                    {
                        collisiionCount += 1; //add 1 to counter for every collision thats true
                        Debug.Log("Counter: " + collisiionCount);
                    }
                }
                
            }
            
        }

        if (collisiionCount > 0)//check if counter is greater than 0
        {
            ghostCheck = true;
            Debug.Log("ghostCheck set to true");
        }
        else
        {
            ghostCheck = false;
            Debug.Log("ghostCheck set to false");
        }
        
    }


    void CalculateOffsets()
    {
        for(int i = 0; i < objectDataList.objects.Length; i++)
        {
            if (finalObj == objectDataList.objects[i])
            {
                posOffset = objectDataList.offsets[i, 0];
                gridOffsetX = objectDataList.offsets[i, 1];
                gridOffsetZ = objectDataList.offsets[i, 2];
            }
        }
    }

    void PlaceObjects()
    {
        mousePos = Input.mousePosition;
        Ray mousePosRay = Camera.main.ScreenPointToRay(mousePos);
        int mask = ~LayerMask.GetMask("Ghost Objects");

        Debug.DrawRay(transform.position, mousePosRay.direction, Color.red);
        if (!checkUI)
        {
            if (Physics.Raycast(transform.position, mousePosRay.direction, out hit, dist, mask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.tag == "Ground" && togglePlaceMode)
                {
                    //show ghost cube at the cursors position on the plane
                    placeHolder.transform.position = hit.point + posOffset;
                    //Debug.Log("cursor is touching plane");

                    if (Input.GetMouseButtonDown(1))//if right click on ground/not on UI or an object
                    {
                        togglePlaceMode = false;
                        placeHolder.SetActive(false);
                        btnTracker = 0;
                    }

                    if (Input.GetMouseButtonDown(0) && !isShift)//if mouse1 is clicked and shift is not being held down
                    {
                        if (!placeHolder.GetComponent<GhostCheck>().collisionDetected)//if ghost collision check is false
                        {
                            if (metal >= metalCost && power >= powerCost) //if has enough metal and power to cover the build costs
                            {
                                GameObject tempPlaceHolder = Instantiate<GameObject>(placeHolder, hit.point + posOffset, placeHolder.transform.rotation);
                                tempPlaceHolder.AddComponent<BuildProgression>().SetValues(finalObj, buildTime);
                                metal -= metalCost;
                                power -= powerCost;

                            }
                            else //if not enough metal or power to build
                            {
                                Debug.Log("CANNOT PLACE, NOT ENOUGH RESOURCES");

                            }
                            
                        }
                        else //if ghost collision check is true
                        {
                            Debug.Log("CANNOT PLACE HERE, ghost object is clipping other objects");
                            
                            if (metal < metalCost || power < powerCost)//if not enough metal or power to build
                            {
                                Debug.Log("CANNOT PLACE, NOT ENOUGH RESOURCES");

                            }

                        }
                    }
                    else if (isShift == true)//if shift is being held down
                    {
                        if (Mathf.RoundToInt(lastPos.x - hit.point.x) < 0)//less than 0
                        {
                            right = true;
                            left = false;
                        }
                        else if (Mathf.RoundToInt(lastPos.x - hit.point.x) > 0)//greater than 0
                        {
                            left = true;
                            right = false;
                        }
                        else //equal to 0
                        {
                            left = false;
                            right = false;
                        }

                        if (Mathf.RoundToInt(lastPos.z - hit.point.z) < 0)//less than 0
                        {
                            up = true;
                            down = false;
                        }
                        else if (Mathf.RoundToInt(lastPos.z - hit.point.z) > 0)//greater than 0
                        {
                            down = true;
                            up = false;
                        }
                        else //equal to 0
                        {
                            up = false;
                            down = false;
                        }

                        float gridDistX = Vector3.Distance(new Vector3(lastPos.x, 0 ,0), new Vector3(hit.point.x, 0, 0));
                        float gridDistZ = Vector3.Distance(new Vector3(0, 0 , lastPos.z), new Vector3(0, 0, hit.point.z));
                        int arraySizeX = Mathf.RoundToInt(gridDistX / gridOffsetX.x);//calculate arraySize for x axis
                        int arraySizeZ = Mathf.RoundToInt(gridDistZ / gridOffsetZ.z);//calculate arraySize for z axis

                        if(arraySizeX < 1)//clamp to minimum 1
                        {
                            arraySizeX = 1;
                        }

                        if (arraySizeZ < 1)//clamp to minimum 1
                        {
                            arraySizeZ = 1;
                        }

                        if (placeGrid.Count == 0)
                        {
                            List<GameObject> placeRow = new List<GameObject>();
                            if (placeRow.Count == 0)
                            {
                                    placeRow.Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset, placeHolder.transform.rotation));
                            }
                            placeGrid.Add(placeRow);
                        }

                        if (placeGrid.Count > 0)//if not empty
                        {
                            int count = placeGrid.Count;//store count
                            if (count < arraySizeZ)//check if less than needed
                            {
                                List<GameObject> placeRow = new List<GameObject>();//create new list
                                for (int i = count; i < arraySizeZ; i++)//loop until count is equal to arraySizeZ
                                {
                                    for (int i3 = 0; i3 < arraySizeX; i3++)//loop until rowSize is equal to arraySizeX
                                    {
                                        //create new rows
                                        if (up && left)
                                        {
                                            placeRow.Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset - gridOffsetX * i3 + gridOffsetZ * i, placeHolder.transform.rotation));
                                        }
                                        else if (down && left)
                                        {
                                            placeRow.Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset - gridOffsetX * i3 - gridOffsetZ * i, placeHolder.transform.rotation));
                                        }
                                        else if (up && right)
                                        {
                                            placeRow.Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset + gridOffsetX * i3 + gridOffsetZ * i, placeHolder.transform.rotation));
                                        }
                                        else if (down && right)
                                        {
                                            placeRow.Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset + gridOffsetX * i3 - gridOffsetZ * i, placeHolder.transform.rotation));
                                        }
                                    }
                                    placeGrid.Add(placeRow);//add new rows to grid
                                }

                            }
                            else if (count > arraySizeZ)//if z is too big
                            {
                                for (int i = count; i > arraySizeZ; i--)//loop until count is equal to arraySizeZ
                                {
                                    int count2 = placeGrid[i - 1].Count;//store/update inner list count (size of row)

                                    for (int i2 = count2; i2 > 0; i2--)
                                    {
                                        Destroy(placeGrid[i - 1][i2 - 1]);//destroy gameobject
                                        placeGrid[i - 1].RemoveAt(i2 - 1);//remove from list
                                    }
                                    placeGrid.RemoveAt(i-1);//remove row from grid
                                }
                            }
                            else if (count == arraySizeZ)//if z is equal to arraySizeZ
                            {
                                for (int i = count; i > 0; i--)//loop backwards until through count
                                {
                                    int count2 = placeGrid[i-1].Count;//store/update inner list count (size of row)
                                    if (count2 < arraySizeX)//check if row size is less than needed
                                    {
                                        for (int i2 = count2; i2 < arraySizeX; i2++)//loop until count is equal to arraySizeX
                                        {

                                            if (up && left)
                                            {
                                                placeGrid[i - 1].Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset - gridOffsetX * i2 + gridOffsetZ * (i-1), placeHolder.transform.rotation));
                                            }
                                            else if (down && left)
                                            {
                                                placeGrid[i - 1].Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset - gridOffsetX * i2 - gridOffsetZ * (i-1), placeHolder.transform.rotation));
                                            }
                                            else if (up && right)
                                            {
                                                placeGrid[i - 1].Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset + gridOffsetX * i2 + gridOffsetZ * (i-1), placeHolder.transform.rotation));
                                            }
                                            else if (down && right)
                                            {
                                                placeGrid[i - 1].Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset + gridOffsetX * i2 - gridOffsetZ * (i-1), placeHolder.transform.rotation));
                                            }
                                            else if (left)
                                            {
                                                placeGrid[i - 1].Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset - gridOffsetX * i2, placeHolder.transform.rotation));
                                            }
                                            else if (right)
                                            {
                                                placeGrid[i - 1].Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset + gridOffsetX * i2, placeHolder.transform.rotation));
                                            }
                                            else if (up)
                                            {
                                                placeGrid[i - 1].Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset + gridOffsetZ * (i - 1), placeHolder.transform.rotation));
                                            }
                                            else if (down)
                                            {
                                                placeGrid[i - 1].Add(Instantiate<GameObject>(placeHolder, lastPos + posOffset - gridOffsetZ * (i - 1), placeHolder.transform.rotation));
                                            }
                                        }
                                    }
                                    else if (count2 > arraySizeX)//check if row size is greater than needed
                                    {
                                        for (int i2 = count2; i2 > arraySizeX; i2--)//loop until count is equal to arraySizeX
                                        {
                                            Destroy(placeGrid[i-1][i2 - 1]);//destroy gameobject
                                            placeGrid[i-1].RemoveAt(i2 - 1);//remove from list
                                        }
                                    }
                                }
                            }
                        }

                        if (placeGrid.Count > 0 && Input.GetMouseButtonDown(0))//if the grid is not empty and mouse button 1 is clicked
                        {
                            float total = placeGrid.Count * placeGrid[0].Count;
                            float totalMetalCost = metalCost * total; 
                            float totalPowerCost = powerCost * total; 
                            CheckGhostCollisions();
                            int count = placeGrid.Count;
                            if (!ghostCheck)//if ghost objects are not colliding with other objects
                            {
                                if (metal >= totalMetalCost && power >= totalPowerCost)
                                {
                                    for (int i = count; i > 0; i--)//for loop to go through grid
                                    {
                                        int count2 = placeGrid[i - 1].Count;
                                        for (int i2 = count2; i2 > 0; i2--)
                                        {
                                            if (placeGrid[i - 1][i2 - 1] != null)
                                            {

                                                //instantiate the placeGrid[i-1][i2-1] and add the new BuildProgression script
                                                placeGrid[i - 1][i2 - 1].AddComponent<BuildProgression>().SetValues(finalObj, buildTime);
                                                Instantiate<GameObject>(placeGrid[i - 1][i2 - 1], placeGrid[i - 1][i2 - 1].transform.position, placeGrid[i - 1][i2 - 1].transform.rotation);
                                                Destroy(placeGrid[i - 1][i2 - 1]);
                                            }
                                        }
                                    }
                                    metal -= totalMetalCost;
                                    power -= totalPowerCost;
                                    isShift = false;
                                }
                                else
                                {
                                    Debug.Log("CANNOT PLACE, NOT ENOUGHT RESOURCES");
                                }
                                

                            }
                            else
                            {
                                Debug.Log("CANNOT PLACE HERE, grid is clipping other objects");
                            }
                        }

                    }

                    if (Input.GetKeyDown(KeyCode.LeftShift) && isShift == false)
                    {
                        isShift = true;
                        lastPos = hit.point;
                    }

                    if (Input.GetKeyUp(KeyCode.LeftShift))
                    {
                        isShift = false;

                        if (placeGrid.Count > 0)
                        {
                            int count = placeGrid.Count;
                            
                            for (int i = count; i > 0; i--)
                            {
                                int count2 = placeGrid[i-1].Count;
                                for (int i2 = count2; i2 > 0; i2--)
                                {
                                    Destroy(placeGrid[i-1][i2-1]);
                                    placeGrid[i -1].RemoveAt(i2-1);
                                }
                                placeGrid.RemoveAt(i - 1);

                            }
                            
                        }
                    }
                }
            }
        }

    }
}
