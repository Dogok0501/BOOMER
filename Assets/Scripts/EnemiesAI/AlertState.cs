using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyAI
{
    EnemyStates enemy;
    float timer = 0;

    public AlertState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void Initialize()
    {

    }

    public void UpdateAction()
    {
        Search();
        Watch();
        if(enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
        {
            LookAround();
            enemy.playerSpotted.gameObject.SetActive(false);
        }        
    }

    private void Search()
    {
        enemy.navMeshAgent.speed = enemy.angrySpeed;
        enemy.navMeshAgent.destination = enemy.lastKnownPosition;
        enemy.navMeshAgent.isStopped = false;
    }


    private void Watch()
    {
        if(enemy.EnemySpotted())
        {
            enemy.navMeshAgent.destination = enemy.lastKnownPosition;
            ToChaseState();
        }
    }

    private void LookAround()
    {
        timer += Time.deltaTime;
        if(timer >= enemy.stayAlertTime)
        {
            timer = 0;
            ToPatrolState();
        }
    }
    
    public void OnTriggerEnter(Collider enemy)
    {

    }

    public void ToPatrolState()
    {
        Debug.Log("ToPatrolState");
        enemy.currentState = enemy.patrolState;
    }

    public void ToAttackState()
    {
        
    }

    public void ToAlertState()
    {

    }

    public void ToChaseState()
    {
        Debug.Log("ToChaseState");
        enemy.currentState = enemy.chaseState;
    }
}
