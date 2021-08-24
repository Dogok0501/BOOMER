using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float speed;
    Transform player;
    float lifetime;
    float timer;

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lifetime = 3f;
    }

    void OnDisable()
    {
        transform.rotation = Quaternion.identity;
    }

    void MissileHitByPlayer()
    {
        Poolable poolable = this.gameObject.GetComponent<Poolable>();
        Managers.Pool.Push(poolable);
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        PushAfterSec();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.SendMessage("HitByEnemy", damage, SendMessageOptions.DontRequireReceiver);
        }

        Poolable poolable = this.gameObject.GetComponent<Poolable>();
        Managers.Pool.Push(poolable);
        timer = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.layer == 12)
        {
            Poolable poolable = this.gameObject.GetComponent<Poolable>();
            Managers.Pool.Push(poolable);
            timer = 0;
        }
    }


    void PushAfterSec()
    {
        timer += Time.deltaTime;

        if (this.GetComponent<Animator>())
        {
            if (timer > this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + lifetime)
            {
                Poolable poolable = this.gameObject.GetComponent<Poolable>();
                Managers.Pool.Push(poolable);
                timer = 0;
            }
        }
        else
        {
            if (timer > lifetime)
            {
                Poolable poolable = this.gameObject.GetComponent<Poolable>();
                Managers.Pool.Push(poolable);
                timer = 0;
            }
        }
    }
}
