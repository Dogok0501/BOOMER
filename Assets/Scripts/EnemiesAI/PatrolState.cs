using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyAI
{
    EnemyStates enemy;
    int nextWayPoint = 0;

    public PatrolState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void Initialize()
    {

    }

    public void UpdateAction()
    {
        Watch();
        Patrol();
    }

    private void Watch()
    {
        if(enemy.EnemySpotted())
        {
            Debug.Log("추적");
            Managers.Sound.Play("Alert");
            ToChaseState();
        }
    }

    private void Patrol()
    {
        enemy.navMeshAgent.speed = enemy.normalSpeed;
        if (enemy.CompareTag("Boss"))
        {
            enemy.anim.SetFloat("MoveSpeed", enemy.normalSpeed);
        }
        enemy.navMeshAgent.destination = enemy.waypoints[nextWayPoint].position;
        enemy.navMeshAgent.isStopped = false;
        if(enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance && !enemy.navMeshAgent.pathPending)
        {
            nextWayPoint = (nextWayPoint + 1) % enemy.waypoints.Length;
        }
    }

    public void OnTriggerEnter(Collider enemy)
    {
        if(enemy.gameObject.CompareTag("Player"))
        {
            ToAlertState();
        }
    }

    public void ToPatrolState()
    {
        Debug.Log("I am patrolling already");
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
        Debug.Log("ToChaseState");
        enemy.currentState = enemy.chaseState;
    }
}
