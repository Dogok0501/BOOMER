using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPop : MonoBehaviour
{
    private new Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddExplosionForce(20, this.transform.position, 5, 1, ForceMode.Impulse);
    }
}
