using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other);
            other.SendMessage("HitByEnemy", 50, SendMessageOptions.DontRequireReceiver);
            other.GetComponent<Rigidbody>().AddExplosionForce(150, transform.position, 30, 2f, ForceMode.Impulse);
            this.gameObject.SetActive(false);
        }
    }
}
