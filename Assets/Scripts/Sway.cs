using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    public float intensity;
    public float smooth;
    float mouseX;
    float mouseY;

    Quaternion originTargetRotation;

    private void Start()
    {        
        originTargetRotation = transform.localRotation;
    }

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
    }

    private void FixedUpdate()
    {
        UpdateSway();
    }

    void UpdateSway()
    {       
        Quaternion adjX = Quaternion.AngleAxis(-intensity * mouseX, Vector3.up);
        Quaternion adjY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        Quaternion targetRotation = originTargetRotation * adjX * adjY;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
    }
}
