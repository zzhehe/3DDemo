using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : MonoBehaviour {
    
    public FsmSystem fsmSystem;
    public EnemyCharacter enemy;

    GameObject player;

    private void Start()
    {
        EnemyFollowState followState = new EnemyFollowState();
        followState.onFixedUpdateMethod += onUpdateFollowState;
        EnemyPatrolState patrolState = new EnemyPatrolState();
        patrolState.onFixedUpdateMethod += onUpdatePatrolState;

        fsmSystem = new FsmSystem();
        fsmSystem.AddState(followState);
        fsmSystem.AddState(patrolState);
        fsmSystem.ChangeState(StateType.EnemyPatrol);

        enemy = this.gameObject.GetComponent<EnemyCharacter>();
        player = GameObject.FindGameObjectWithTag("Player");

    }
    
    private void Update()
    {
        fsmSystem.UpdateState();
        fsmSystem.currentState.TriggerEvent(fsmSystem, gameObject);
    }

    private void FixedUpdate()
    {
        fsmSystem.FixedUpdateState();
    }

    private void onUpdateFollowState()
    {
        enemy.onUpdateFollowState(player);
    }

    private void onUpdatePatrolState()
    {
        enemy.onUpdatePatrolState(player);
    }
}
