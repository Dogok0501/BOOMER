using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyAI
{
    EnemyStates enemy;

    public ChaseState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void Initialize()
    {

    }

    public void UpdateAction()
    {
        Watch();
        Chase();
    }

    private void Watch()
    {
        if(!enemy.EnemySpotted())
        {
            ToAlertState();
        }
    }

    private void Chase()
    {
        Debug.Log($"에너미타겟 : {enemy.chaseTarget}");

        enemy.playerSpotted.gameObject.SetActive(true);

        enemy.navMeshAgent.speed = enemy.angrySpeed;
        enemy.navMeshAgent.destination = enemy.chaseTarget.position;
        enemy.navMeshAgent.isStopped = false;
        if(enemy.navMeshAgent.remainingDistance <= enemy.attackRange && enemy.onlyMelee == true)
        {
            enemy.navMeshAgent.isStopped = true;
            ToAttackState();
        }
        else if (enemy.navMeshAgent.remainingDistance <= enemy.shootRange && enemy.onlyMelee == false)
        {
            enemy.navMeshAgent.isStopped = true;
            ToAttackState();
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
        Debug.Log("ToAttackState");
        enemy.currentState = enemy.attackState;
    }

    public void ToAlertState()
    {
        Debug.Log("ToAlertState");
        enemy.currentState = enemy.alertState;
    }

    public void ToChaseState()
    {

    }
}
