using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyAI
{
    EnemyStates enemy;
    float timer;

    public AttackState(EnemyStates enemy)
    {
        this.enemy = enemy;        
    }

    public void Initialize()
    {

    }

    public void UpdateAction()
    {
        timer += Time.deltaTime;
        float distance = Vector3.Distance(enemy.chaseTarget.transform.position, enemy.transform.position);
        if(distance > enemy.attackRange && enemy.onlyMelee == true)
        {
            ToChaseState();
        }

        if(distance > enemy.shootRange && enemy.onlyMelee == false)
        {
            ToChaseState();
        }

        Watch();      

        if(distance <= enemy.shootRange && distance > enemy.attackRange && enemy.onlyMelee == false && timer >= enemy.attackDelay)
        {
            if(enemy.CompareTag("Enemy"))
            {
                Attack(true);
                timer = 0;
            }
        }

        if(distance <= enemy.attackRange && timer >= enemy.attackDelay)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Attack(false);
                timer = 0;
            }
        }
    }

    private void Attack(bool shoot)
    {
        Debug.Log("공격");
        if(shoot == false)
        {
            enemy.chaseTarget.SendMessage("HitByEnemy", enemy.meleeDamage, SendMessageOptions.DontRequireReceiver);
        }
        else if(shoot == true)
        {
            GameObject missile = Managers.Pool.Pop(enemy.missile).gameObject;
            missile.transform.position = enemy.transform.Find("Fire Point").position;
            missile.GetComponent<Missile>().speed = enemy.missileSpeed;
            missile.GetComponent<Missile>().damage = enemy.missileDamage;
        }
    }

    private void Watch()
    {
        if(!enemy.EnemySpotted())
        {
            ToAlertState();
        }
    }

    public void OnTriggerEnter(Collider enemy)
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
        Debug.Log("ToAlertState");
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {
        Debug.Log("ToChaseState");
        enemy.currentState = enemy.chaseState;
    }
}
