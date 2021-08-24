using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    Vector3 cameraDirection;
    SpriteRenderer sr;

    private void Start()
    {
        if(sr)
        {
            sr = GetComponent<SpriteRenderer>();
            sr.flipX = true;
        }       
    }

    private void Update()
    {
        if (sr)
        {
            cameraDirection = -Camera.main.transform.forward;
            cameraDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(cameraDirection);
        }
        else
        {
            cameraDirection = -Camera.main.transform.forward;
            cameraDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(cameraDirection);
        }
    }
}
