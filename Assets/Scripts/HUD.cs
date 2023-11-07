using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public GameObject player;
    public PlayerController playerController;
    public Text amount, usage;

    void Start()
    {
        player = Camera.main.gameObject;
        slider = GetComponent<Slider>();
        playerController = player.GetComponent<PlayerController>();
        amount = GetComponentInChildren<Text>();

        if (gameObject.name == "Power Bar")
        {
            usage = transform.Find("Power Usage Text").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Power Bar")
        {
            slider.maxValue = playerController.powerCap;
            slider.value = playerController.power;
            //amount.text = slider.value.ToString("f1") + "/" + playerController.powerCap + " (" + (playerController.incomingPower - playerController.totalPConsumption) + ")";//amount out of total + (Net Total Power)
            amount.text = slider.value.ToString("f1") + "/" + playerController.powerCap + " (" + playerController.incomingPower.ToString("f1") + ")";//amount out of total + (Incoming Power)
            usage.text = "Power Usage: " + playerController.totalPConsumption;
        }
        else if (gameObject.name == "Metal Bar")
        {
            slider.maxValue = playerController.metalCap;
            slider.value = playerController.metal;
            amount.text = slider.value.ToString("f1") + "/" + playerController.metalCap + " (" + (playerController.incomingMetal.ToString("f1")) + ")";//amount out of total + (Incoming Metal)
        }


    }
}
