using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public ParticleSystem explosionEffect;
    public float delay = 2f;

    public float explosionForce = 10f;
    public float radius = 15f;
    public float damage = 30f;

    private void Start()
    {
        StartCoroutine(Explode());
        Util.FindChild<ParticleSystem>(transform.gameObject, null, true);
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(delay);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider near in colliders)
        {
            if (near.CompareTag("Enemy"))
            {
                near.SendMessage("HitByPlayer", damage, SendMessageOptions.DontRequireReceiver);                
                if (near.gameObject.GetComponent<EnemyStates>().currentState == near.gameObject.GetComponent<EnemyStates>().patrolState || near.gameObject.GetComponent<EnemyStates>().currentState == near.gameObject.GetComponent<EnemyStates>().alertState)
                {
                    near.gameObject.SendMessage("HiddenShot", GameObject.Find("Player").transform.position, SendMessageOptions.DontRequireReceiver);
                }
            }
            else if(near.CompareTag("Boss"))
            {
                near.SendMessage("HitByPlayer", damage, SendMessageOptions.DontRequireReceiver);
            }
            else if (near.CompareTag("Missile"))
            {
                near.SendMessage("MissileHitByPlayer", damage, SendMessageOptions.DontRequireReceiver);
            }
        }

        Managers.Sound.Play("GrenadeExplosion", SoundManager.SoundType.Effect);
        explosionEffect.Play();
        transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        transform.Find("Model").gameObject.SetActive(false);

        yield return new WaitForSeconds(2);

        Destroy(gameObject);
    }
}
