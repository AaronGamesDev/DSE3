using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public float speed = 25f;
    public GameObject instantiator;
    public bool enemyDestroyed;
    public GameObject muzzleFlashPrefab, impactPrefab;
    public Vector3 startPos, currentPos;
    public float range = 50f;

    private void Start()
    {
        startPos = transform.position;
        if (muzzleFlashPrefab != null)
        {
            var muzzleFlashFX = Instantiate<GameObject>(muzzleFlashPrefab, transform.position, Quaternion.identity);
            muzzleFlashFX.transform.forward = gameObject.transform.forward;
            var psMuzzle = muzzleFlashFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null)
            {
                Destroy(muzzleFlashFX, psMuzzle.main.duration);
            }
            else
            {
                var psChild = muzzleFlashFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleFlashFX, psChild.main.duration);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        OnTurretDeath();
        FalloffDist();
        Move();

    }

    void FalloffDist()
    {
        currentPos = transform.position;
        if (Vector3.Distance(startPos, currentPos) >= range)
        {
            if (instantiator != null)
            {
                instantiator.GetComponent<Turret>().projectiles.Remove(gameObject);//remove from turrets list of projectiles

            }
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (speed != 0)
        {
            transform.position = transform.position + transform.forward * (speed * Time.deltaTime);
        }
    }

    private void OnTurretDeath()
    {
        if (instantiator == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0f;

        ContactPoint contact = collision.contacts[0];

        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (impactPrefab != null)
        {
            var impactFX = Instantiate<GameObject>(impactPrefab, pos, rot);
            var psImpact = impactFX.GetComponent<ParticleSystem>();
            if (psImpact != null)
            {
                Destroy(impactFX, psImpact.main.duration);
            }
            else
            {
                var psChild = impactFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(impactFX, psChild.main.duration);
            }
        }

        if (collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<NewEnemyBot>() != null)
        {
            GameObject tempEnemy = collision.gameObject;
            enemyDestroyed = collision.gameObject.GetComponent<NewEnemyBot>().ReceiveDmg(instantiator.GetComponent<Turret>().dmg);//apply turrets dmg value to enemy and stores if enemy hp <= 0 or not

            if (enemyDestroyed)
            {
                Destroy(collision.gameObject);//destroy the bot
                instantiator.GetComponent<Turret>().enemies.Remove(tempEnemy);
            }
        }

        

        instantiator.GetComponent<Turret>().projectiles.Remove(gameObject);//remove from turrets list of projectiles
        Destroy(gameObject);
    }
}
