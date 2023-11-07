using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyBot : MonoBehaviour
{
    public float currentDist = 0;
    public GameObject laser;
    public GameObject target = null;
    public float hp = 200f;
    public float dmg = 25f;
    public float rotateSpeed = 1f;
    public float speed = 5f;
    public Rigidbody rb;
    public float attackRange = 5f;
    public float attackRate = 0.25f;
    public bool isAttacking, attackStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetTarget();
        CheckTarget();
    }

    //Used in ProjectileMove Script to apply damage to enemies on collision. Returns true when hp is less than or equal to 0 for the projectile to then Destroy the enemy gameObject
    /// <summary>
    /// Applies incomingDmg to the bot and returns true or false based on hp. Returns False if hp is greater than 0, Returns True if hp is less than or equal to 0
    /// </summary>
    /// <param name="incomingDmg"></param>
    /// <returns></returns>
    public bool ReceiveDmg(float incomingDmg)
    {
        if (hp > 0)
        {
            hp -= incomingDmg;

            if (hp > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
        else
        {
            return true;
        }
    }

    //Waits for attackRate, before calling Attack() function
    IEnumerator AttackRate()
    {
        yield return new WaitForSeconds(attackRate);
        Attack();
    }

    //Checks target, distance to target, and if target hp is greater than 0 before setting isAttacking to true and applying damage to the target.
    //If isAttacking is true, it will start AttackRate() coroutine which waits before calling the Attack() function again
    void Attack()
    {

        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
            {
                if (target.GetComponent<Structure>().hp > 0)
                {
                    isAttacking = true;
                    target.GetComponent<Structure>().ReceiveDmg(dmg);
                }
            }
        }


        if (isAttacking)
        {
            StartCoroutine(AttackRate());
        }
    }

    //Checks if there is a target, if so calculates direction which is then passed into LookAtTarget() and MoveTowardsTarget(). If there is no target, checks attacking variables and sets them to false if needed
    void CheckTarget()
    {
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            LookAtTarget(dir);
            MoveTowardsTarget(target, dir);
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;

            }
            if (laser.activeSelf)
            {
                laser.SetActive(false);

            }
            if (attackStarted)
            {
                attackStarted = false;

            }
        }
    }

    //Rotates the enemy to face the direction of the target
    void LookAtTarget(Vector3 dir)
    {
        Quaternion toRotatation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotatation, rotateSpeed * Time.deltaTime);
    }

    //Move towards the target, while moving attack variables are set to false. When within attack range, stops moving, and sets attack variables to true and starts attacking.
    void MoveTowardsTarget(GameObject pTarget, Vector3 dir)
    {
        if (Vector3.Distance(pTarget.transform.position, transform.position) > attackRange)
        {
            if (isAttacking)
            {
                isAttacking = false;

            }
            if (laser.activeSelf)
            {
                laser.SetActive(false);

            }
            if (attackStarted)
            {
                attackStarted = false;

            }
            rb.drag = 1f;
            rb.AddForce(dir.normalized * speed, ForceMode.Force);
            Vector3 vel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            //Debug.Log("vel after: " + vel.magnitude);
            if (vel.magnitude > speed)
            {
                //Debug.Log("limiting speed before: " + vel.magnitude);
                Vector3 newVel = vel.normalized * speed;
                rb.velocity = new Vector3 (newVel.x, rb.velocity.y, newVel.z);
                //Debug.Log("limiting speed after: " + vel.magnitude);
            }
        }
        else if (Vector3.Distance(pTarget.transform.position, transform.position) <= attackRange)
        {
            rb.drag = 5f;
            if (!attackStarted)//if attack has not already been started
            {
                laser.SetActive(true);
                attackStarted = true;
                Attack();
                Debug.Log("Attack Started");
                
            }
        }

    }

    //Loops through global list to find closest distance and sets that as its target
    void SetTarget()
    {
        if (GlobalPlayerObjectsList.GlobalList.Count > 0)
        {
            foreach (GameObject obj in GlobalPlayerObjectsList.GlobalList)
            {
                if (obj != null)
                {
                    if (target == null)
                    {
                        currentDist = Vector3.Distance(transform.position, obj.transform.position);
                        target = obj;
                    }
                    else if (target != null && currentDist != 0)
                    {
                        if (currentDist > Vector3.Distance(transform.position, obj.transform.position))
                        {
                            currentDist = Vector3.Distance(transform.position, obj.transform.position);
                            target = obj;
                        }
                    }
                }
                else
                {
                    target = null;
                    currentDist = 0;
                }
                
            }
        }
        else
        {
            target = null;
            currentDist = 0;
        }
        
    }
}
