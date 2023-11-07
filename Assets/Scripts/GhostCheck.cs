using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCheck : MonoBehaviour
{
    public bool collisionDetected = false;

    bool m_Started;
    public LayerMask m_LayerMask = 1;//Default Layer
    // Start is called before the first frame update
    void Start()
    {
        //Use this to ensure that the Gizmos are being drawn when in Play Mode.
        m_Started = true;
    }

    void FixedUpdate()
    {
        MyCollisions();
    }

    void MyCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale, Quaternion.identity, m_LayerMask, QueryTriggerInteraction.Ignore);//any colliders on the default layer mask 
        
        //DISCLAIMER: THIS DOESN'T WORK AS INTENDED DUE TO IMPORTED SCALE OF OBJECTS, HENCE IN EDITOR THE CUBES DRAWN ARE SMALLER THAN INTENDED AND ARE NOT ACCURATE TO DETECT ALL COLLISIONS
        
        if (hitColliders.Length > 0)//if hitColliders is greater than 0 meaning there is one or more collisions
        {
            collisionDetected = true;
        }
        else
        {
            collisionDetected = false;
        }
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
    
}
