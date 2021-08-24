using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAI
{
    void Initialize();

    void UpdateAction();

    void OnTriggerEnter(Collider enemy);

    void ToPatrolState();

    void ToAttackState();

    void ToAlertState();

    void ToChaseState();
}
