using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0,1)]
    public float mSensitivityX = 0.5f, mSensitivityY = 0.5f, mSensitivityScroll = 0.5f;
    public bool screenGrab = false;
    public Vector3 newPos;
    public float lerpSpeed = 2f;
    public float currentYPos;

    // Update is called once per frame
    void Update()
    {
        CheckScreenGrab();
        CameraMovement();
        CameraZoom();
    }

    void CheckScreenGrab()
    {
        if (Input.GetMouseButtonDown(2))
        {
            screenGrab = true;
            currentYPos = transform.position.y;
        }

        if (Input.GetMouseButtonUp(2))
        {
            screenGrab = false;
        }
    }

    void CameraMovement()
    {
        if (screenGrab)
        {
            float applyX = mSensitivityX * Input.GetAxis("Mouse X");
            float applyY = mSensitivityY * Input.GetAxis("Mouse Y");

            newPos = new Vector3(transform.position.x - applyX, currentYPos, transform.position.z - applyY);
            transform.Translate(new Vector3(-applyX, 0, -applyY), Space.World);

        }
        
    }

    void CameraZoom()
    {
        float applyScroll = 0;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            applyScroll = mSensitivityScroll * Input.mouseScrollDelta.y;
        }

        if (transform.position.y <= 5)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 5, transform.position.z), lerpSpeed * Time.deltaTime);
            if (applyScroll > 0)
            {
                applyScroll = 0;
            }
        }

        transform.Translate(new Vector3(0, 0, applyScroll), Space.Self);
    }
}
