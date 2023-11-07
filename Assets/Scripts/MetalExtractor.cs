using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalExtractor : PowerUsingResourceStructure
{
    public bool nodePermission = false;
    public Material onlineMat, offlineMat;
    public MeshRenderer[] children;

    public GameObject node;
    public MetalNode mNode;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        children = GetComponentsInChildren<MeshRenderer>();
        InitialiseStructure();
        StartCoroutine(ConsumptionRate());
        StartCoroutine(GenerateRate());

    }

    // Update is called once per frame
    void Update()
    {
        if (!nodePermission)
        {
            if (!GameSettings.needNodePermission)
            {
                BypassPermission();
            }
        }
        else
        {
            if (GameSettings.needNodePermission && node == null)
            {
                nodePermission = false;
            }
        }

        UpdateStats();
        CheckAttacked();
        CheckPower();
        SetOnline();

    }

    void BypassPermission()
    {
        nodePermission = true;
        resourceAmount = Random.Range(GameSettings.minMetal, GameSettings.maxMetal);
    }

    public void ReceiveNode(GameObject pNode)
    {
        node = pNode;
        mNode = node.GetComponent<MetalNode>();
    }

    public override void UpdatePlayerList()
    {
        if (!playerController.Extractors.Contains(this.GetComponent<MetalExtractor>()))//if the player does not have this extractor in its list
        {
            playerController.Extractors.Add(this.GetComponent<MetalExtractor>());
        }
    }

    public override void ApplyTier1Stats()
    {
        maxHp = 500f;
        powerUsage = 5f;
        multiplier = 1f;
        generationRate = 1f;
        base.ApplyTier1Stats();
    }

    public override void ApplyTier2Stats()
    {
        maxHp = 1000f;
        powerUsage = 10f;
        multiplier = 2.5f;
        generationRate = 1f;
        base.ApplyTier2Stats();
    }

    public override void DestroyAndRemoveFromPlayer()
    {
        playerController.Extractors.Remove(this.GetComponent<MetalExtractor>());
        if (node != null)//if within a metal node's list of extractors
        {
            mNode.extractors.Remove(gameObject);
        }
        base.DestroyAndRemoveFromPlayer();
    }

    public override void GenerateResource()
    {
        if (online)
        {
            if (playerController.metal < playerController.metalCap)
            {
                playerController.metal += resourceAmount * multiplier;
            }
            else
            {
                playerController.metal = playerController.metalCap;
                playerController.overflowM = true;
                Debug.Log("Overflowing Metal!");
            }
        }
    }

    public void ReceivePermision(bool perm)
    {
        nodePermission = perm;
    }

    public override void SetOnline()
    {
        if (nodePermission && CheckPower())//if nodePermission is true AND has enough power
        {
            if (!online)//if not online, set online to true, set animController hasPower to true
            {
                online = true;
                anim.SetBool("hasPower", true);
            }
            foreach (MeshRenderer meshR in children)//set lights to onlineMat
            {
                if (meshR.gameObject.tag == "Light")
                {
                    meshR.material = onlineMat;

                }
            }
        }
        else if (!nodePermission || !CheckPower())//if no nodePermission OR not enough power
        {
            if (online)//if still online, set online to false, set animController hasPower to false
            {
                online = false;
                anim.SetBool("hasPower", false);
            }
            foreach (MeshRenderer meshR in children)//set lights to offlineMat
            {
                if (meshR.gameObject.tag == "Light")
                {
                    meshR.material = offlineMat;

                }
            }
        }
    }

    public void ReceiveRate(float rate)
    {
        resourceAmount = rate;
    }
}
