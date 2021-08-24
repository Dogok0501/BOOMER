using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    float lifetime;
    float timer;

    private void Start()
    {
        lifetime = 2f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (this.GetComponent<Animator>())
        {
            if(timer > this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + lifetime)
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
