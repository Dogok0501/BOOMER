using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponbob : MonoBehaviour
{
    PlayerMovementController playerMovementController;

    Vector3 weaponOrigin;
    Vector3 weaponbobPosition;

    float movementCounter;
    float idleCounter;

    private void Start()
    {
        playerMovementController = GetComponentInParent<PlayerMovementController>();
        weaponOrigin = transform.localPosition;
    }
       
    private void OnDisable()
    {
        transform.localPosition = weaponOrigin;
    }

    private void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        if (horizontalMovement == 0 && verticalMovement == 0) // °¡¸¸È÷ ÀÖ±â
        {
            Headbobbing(idleCounter, 0.025f, 0.025f);
            idleCounter += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, weaponbobPosition, Time.deltaTime * 2f);
        }           
        else if(playerMovementController.isSprinting == false) // °È±â
        {
            Headbobbing(movementCounter, 0.035f, 0.035f);
            movementCounter += Time.deltaTime * 3f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, weaponbobPosition, Time.deltaTime * 6f);
        }        
        else
        {
            Headbobbing(movementCounter, 0.15f, 0.075f); // ¶Ù±â
            movementCounter += Time.deltaTime * 7f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, weaponbobPosition, Time.deltaTime * 10f);
        }

    }

    public void Headbobbing(float z, float x_Intensitiy, float y_Intensity)
    {
        weaponbobPosition = weaponOrigin + new Vector3(Mathf.Cos(z) * x_Intensitiy, Mathf.Sin(z * 2) * y_Intensity, weaponOrigin.z);
    }
}
