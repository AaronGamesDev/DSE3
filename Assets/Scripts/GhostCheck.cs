using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhostCheck : MonoBehaviour
{
    public bool collisionDetected = false;

    bool m_Started;
    public LayerMask m_LayerMask = 1;//Default Layer
    public Vector3 colliderSize = new Vector3(1f, 1f, 1f);//Used to adjust size of Ghost Prefab colliders visually within unity inspector using OnDrawGizmos(), half this when used in Physics.OverlapBox as it uses a radius instead
    public float heightOffset = 1f;//Added to transform.position.y before passing into Physics.OverlapBox and Gizmos.DrawWireCube()
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
        Vector3 position = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(position, colliderSize / 2, Quaternion.identity, m_LayerMask, QueryTriggerInteraction.Ignore);//detects any colliders on the default layer mask 
        
        
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
        Vector3 position = new Vector3(transform.position.x, transform.position.y + heightOffset, transform.position.z);

        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(position, colliderSize);
    }
    
}
