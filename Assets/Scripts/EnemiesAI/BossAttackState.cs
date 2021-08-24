using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : IEnemyAI
{
    EnemyStates enemy;
    float timer;
    bool isLook;
    CoroutineHandler coroutineHandler;
    Transform bullletSpreadPoint;

    public BossAttackState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void Initialize()
    {
        
    }

    public void UpdateAction()
    {        
        timer += Time.deltaTime;

        if (isLook)
        {
            enemy.transform.LookAt(new Vector3(enemy.chaseTarget.position.x, 0, enemy.chaseTarget.position.z));
        }

        if (enemy.EnemySpotted())
        {
            float distance = Vector3.Distance(enemy.chaseTarget.transform.position, enemy.transform.position);

            if (distance <= enemy.shootRange && distance > enemy.attackRange && enemy.onlyMelee == false && timer >= enemy.attackDelay)
            {
                int ranAction = Random.Range(0, 3);
                switch (ranAction)
                {
                    case 0:
                        Shot();
                        timer = 0;
                        break;
                    case 1:
                        CoroutineHandler.Start_Coroutine(Shockwave());
                        timer = 0;
                        break;
                    case 2:
                        BulletSpread();
                        timer = 0;
                        break;
                }                
            }            
        }        
    }

    private void Shot()
    {
        enemy.anim.SetTrigger("triShot");
        Debug.Log("Shot");

        Managers.Sound.Play("DuckQuack", SoundManager.SoundType.Effect);

        GameObject missile = Managers.Pool.Pop(enemy.missile).gameObject;
        missile.transform.position = Util.FindChild(enemy.gameObject, "Missile Port A", true).transform.position;
        missile.GetComponent<Missile>().speed = enemy.missileSpeed;
        missile.GetComponent<Missile>().damage = enemy.missileDamage;

        GameObject missile2 = Managers.Pool.Pop(enemy.missile).gameObject;
        missile2.transform.position = Util.FindChild(enemy.gameObject, "Missile Port B", true).transform.position;
        missile2.GetComponent<Missile>().speed = enemy.missileSpeed;
        missile2.GetComponent<Missile>().damage = enemy.missileDamage;
    }

    IEnumerator Shockwave()
    {
        Managers.Sound.Play("DuckQuack", SoundManager.SoundType.Effect);

        GameObject slash = enemy.effect.transform.Find("Slash").gameObject;

        enemy.navMeshAgent.enabled = false;

        enemy.boxCollider.enabled = false;
        enemy.navMeshAgent.enabled = true;
        enemy.navMeshAgent.isStopped = false;
        isLook = false;

        enemy.anim.SetTrigger("triShockwave");
        slash.SetActive(true);
        enemy.shockwave.SetActive(true);
        enemy.navMeshAgent.speed = enemy.angrySpeed;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;               
        
        yield return new WaitForSeconds(2.0f);

        if(slash.activeSelf == true)
            slash.SetActive(false);
        if(enemy.shockwave.activeSelf == true)
            enemy.shockwave.SetActive(false);

        enemy.boxCollider.enabled = true;
        enemy.navMeshAgent.isStopped = true;
        isLook = true;

        yield return null;
    }

    void BulletSpread()
    {
        Managers.Sound.Play("DuckQuack", SoundManager.SoundType.Effect);

        enemy.anim.SetTrigger("triBulletSpread");

        for (int i = 0; i < 5; i++)
        {
            Vector2 duckGrenadeOffset = Random.insideUnitCircle * 0.5f;
            Vector3 duckGrenaderandomTarget = new Vector3(Util.FindChild(enemy.gameObject, "BulletSpreadPoint2", true).transform.localPosition.x + duckGrenadeOffset.x, 0, Util.FindChild(enemy.gameObject, "BulletSpreadPoint2", true).transform.localPosition.z + duckGrenadeOffset.y);

            GameObject _duckGrenade = MonoBehaviour.Instantiate(enemy.duckGrenade).gameObject; //Managers.Pool.Pop(enemy.duckGrenade).gameObject;
            _duckGrenade.transform.position = Util.FindChild(enemy.gameObject, "BulletSpreadPoint", true).transform.position;
            _duckGrenade.GetComponent<Rigidbody>().AddForce(duckGrenaderandomTarget * 80, ForceMode.Impulse);
        }        
    }

    public void OnTriggerEnter(Collider player)
    {
        
    }

    public void ToPatrolState()
    {

    }

    public void ToAttackState()
    {

    }

    public void ToAlertState()
    {
        
    }

    public void ToChaseState()
    {
        
    }
}
