using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vision : MonoBehaviour
{
    Vector3 destination;
    NavMeshAgent nma;

    private void Start()
    {
        nma = transform.parent.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (nma.enabled)
            destination = transform.parent.GetComponent<NavMeshAgent>().destination;
        else
            destination = transform.position;

        transform.LookAt(destination);
    }
}
