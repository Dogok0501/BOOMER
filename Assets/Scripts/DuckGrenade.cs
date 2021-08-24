using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckGrenade : MonoBehaviour
{
    public ParticleSystem energyExplosionEffect;
    public float delay = 2f;

    public float explosionForce = 150f;
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
            if (near.CompareTag("Player"))
            {
                near.SendMessage("HitByEnemy", damage, SendMessageOptions.DontRequireReceiver);
                near.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, radius, 2f, ForceMode.Impulse);
            }
        }

        Managers.Sound.Play("GrenadeExplosion", SoundManager.SoundType.Effect);
        energyExplosionEffect.Play();
        transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        transform.Find("Model").gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

    void MissileHitByPlayer()
    {
        Destroy(gameObject);
    }
}
