using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : PowerUsingStructure
{
    public GameObject gun;
    public float aimSpeed = 15f;
    public float dmg = 25f;
    public float fireRate = 4f;
    private float timeToFire = 0f;
    public List<GameObject> enemies;
    public float closestDist = 0;
    public GameObject currentTarget;
    public List<GameObject> projectiles;
    public Transform projectileSpawn;
    public GameObject projectilePrefab;
    public RaycastHit hit;
    public bool aimed = false;
    // Start is called before the first frame update
    void Start()
    {
        InitialiseStructure();
        StartCoroutine(ConsumptionRate());
    }

    public override void UpdatePlayerList()
    {
        if (!playerController.Turrets.Contains(this.GetComponent<Turret>()))//if player does not already have this power storage within its list
        {
            playerController.Turrets.Add(this.GetComponent<Turret>());//add this storage to the players list
        }

    }

    // Update is called once per frame
    void Update()
    {
        SetOnline();
        UpdateStats();
        CheckAttacked();
        SetTarget();

        if (currentTarget != null && online)
        {
            AimAtTarget(currentTarget);
            if (aimed)
            {
                Shoot();
            }
        }
        else
        {
            ResetAim();
        }
    }




    void ResetAim()
    {
        gun = transform.Find("Base").gameObject.transform.Find("Gun").gameObject;
        gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, transform.rotation, aimSpeed * Time.deltaTime);
    }

    void Shoot()
    {
        if (Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            projectiles.Add(Instantiate<GameObject>(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation));
            projectiles[projectiles.Count - 1].GetComponent<ProjectileMove>().instantiator = gameObject;
        }
    }


    void AimAtTarget(GameObject target)
    {
        gun = transform.Find("Base").gameObject.transform.Find("Gun").gameObject;
        Quaternion toRotatation = Quaternion.LookRotation(target.transform.position - gun.transform.position);
        gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, toRotatation, aimSpeed * Time.deltaTime);

        
        Debug.DrawRay(projectileSpawn.transform.position, gun.transform.forward * 300, Color.red);
        if (Physics.Raycast(projectileSpawn.transform.position, gun.transform.forward, out hit, Vector3.Distance(gun.transform.position, target.transform.position), ~LayerMask.GetMask("Ignore Raycast"), QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.name == target.name)
            {
                aimed = true;
            }
            else
            {
                Debug.Log("aiming at: " + hit.collider.name);
                aimed = false;
            }
        }
        
    }

    public override void ApplyTier1Stats()
    {
        maxHp = 200f;
        dmg = 25f;
        aimSpeed = 15f;
        fireRate = 2f;
        powerUsage = 10f;
        base.ApplyTier1Stats();
    }

    public override void ApplyTier2Stats()
    {
        maxHp = 400f;
        dmg = 50f;
        aimSpeed = 30f;
        fireRate = 4f;
        powerUsage = 15f;
        base.ApplyTier2Stats();
    }

    public override void DestroyAndRemoveFromPlayer()
    {
        playerController.Turrets.Remove(this.GetComponent<Turret>());
        base.DestroyAndRemoveFromPlayer();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!enemies.Contains(other.gameObject))//if not in list, add to list
            {
                enemies.Add(other.gameObject);
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (enemies.Contains(other.gameObject))//if in list, remove to list
            {
                if (currentTarget = other.gameObject)
                {
                    currentTarget = null;
                    closestDist = 0;
                }
                enemies.Remove(other.gameObject);
            }
        }
    }

    void SetTarget()
    {
        if (enemies.Count > 0)//if enemies list is not empty
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                {
                    float currentDist = Vector3.Distance(transform.position, enemies[i].transform.position);

                    if (closestDist == 0 || currentTarget == null)
                    {
                        closestDist = currentDist;
                        currentTarget = enemies[i];
                    }
                    else if (closestDist > currentDist)
                    {
                        closestDist = currentDist;
                        currentTarget = enemies[i];
                    }
                }
                else
                {
                    enemies.RemoveAt(i);
                    closestDist = 0;
                    currentTarget = null;
                }


                
            }
        }
        else if (enemies.Count <= 0)
        {
            closestDist = 0;
            currentTarget = null;
        }
        
    }
}
