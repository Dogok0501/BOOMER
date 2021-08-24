using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    public Gun pickupGun;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Weapon>().PickupWeapon(pickupGun.gunName); 
            transform.gameObject.SetActive(false);
            Managers.Sound.Play("Gun/Reload/"+pickupGun.gunName, SoundManager.SoundType.Effect);
        }
    }

    //IEnumerator PickupDisable()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    transform.gameObject.SetActive(false);
    //}
}
