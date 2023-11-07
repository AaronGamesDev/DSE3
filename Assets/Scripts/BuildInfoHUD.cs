using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildInfoHUD : MonoBehaviour
{
    public GameObject buildPanel;
    public Image image;
    public Text objectNameText, objectDescriptionText, buildCostText, buildTimeText, powerUsageText;
    public GameObject player;
    public PlayerController playerController;
    public Sprite[] previewImages = new Sprite[5];
    public ObjectDataList objectDataList;

    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.gameObject;
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.btnTracker == 0)
        {
            Display(false);
        }
        else if (playerController.btnTracker == 1)
        {
            Display(true);

            image.sprite = previewImages[0];
            objectNameText.text = "Metal Extractor";
            objectDescriptionText.text = "Metal Extractors produce a certain amount of metal resources, dicatated by there assigned Metal Node." +
                " Extractors NEED to be placed on a Metal Node in order to produce metal (unless 'needNodes' rule is turned off), and also use power to work.";
            buildCostText.text = "Build Cost: " + objectDataList.powerCost[0] + "P | " + objectDataList.metalCost[0] + "M";
            buildTimeText.text = "Build Time: " + objectDataList.buildTimes[0] + ".sec";
            powerUsageText.text = "Power Usage: 5 p/s";
        }
        else if (playerController.btnTracker == 2)
        {
            Display(true);

            image.sprite = previewImages[1];
            objectNameText.text = "Solar Panel";
            objectDescriptionText.text = "Solar Panels produce a certain amount of power resources. They should be used to maintain a steady power income or increase power reserves." +
                " Solar Panels do NOT need power to work, just the initial building cost.";
            buildCostText.text = "Build Cost: " + objectDataList.powerCost[2] + "P | " + objectDataList.metalCost[2] + "M";
            buildTimeText.text = "Build Time: " + objectDataList.buildTimes[2] + ".sec";
            powerUsageText.text = "Power Usage: 0 p/s";
        }
        else if (playerController.btnTracker == 3)
        {
            Display(true);

            image.sprite = previewImages[2];
            objectNameText.text = "Turret";
            objectDescriptionText.text = "Turrets shoot at enemy bots when they are within range. They should be used to protect your structures and base." +
                " Turrets NEED power to work.";
            buildCostText.text = "Build Cost: " + objectDataList.powerCost[4] + "P | " + objectDataList.metalCost[4] + "M";
            buildTimeText.text = "Build Time: " + objectDataList.buildTimes[4] + ".sec";
            powerUsageText.text = "Power Usage: 10 p/s";
        }
        else if (playerController.btnTracker == 4)
        {
            Display(true);

            image.sprite = previewImages[3];
            objectNameText.text = "Metal Storage";
            objectDescriptionText.text = "Metal Storages increase the maximum Metal resources you can hold. They should be used if you are producing more Metal than you are using." +
                " Metal Storages do NOT need power to work.";
            buildCostText.text = "Build Cost: " + objectDataList.powerCost[6] + "P | " + objectDataList.metalCost[6] + "M";
            buildTimeText.text = "Build Time: " + objectDataList.buildTimes[6] + ".sec";
            powerUsageText.text = "Power Usage: 0 p/s";
        }
        else if (playerController.btnTracker == 5)
        {
            Display(true);

            image.sprite = previewImages[4];
            objectNameText.text = "Power Storage";
            objectDescriptionText.text = "Power Storages increase the maximum Power resources you can hold. They should be used if you are producing more Power than you are using." +
                " Power Storages do NOT need power to work.";
            buildCostText.text = "Build Cost: " + objectDataList.powerCost[8] + "P | " + objectDataList.metalCost[8] + "M";
            buildTimeText.text = "Build Time: " + objectDataList.buildTimes[8] + ".sec";
            powerUsageText.text = "Power Usage: 0 p/s";
        }
    }

    void Display(bool setTo)
    {
        if (buildPanel.activeSelf != setTo)
        {
            buildPanel.SetActive(setTo);
        }
        
    }
}
