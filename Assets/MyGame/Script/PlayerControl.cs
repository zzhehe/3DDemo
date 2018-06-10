using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public FsmSystem fsmSystem;
    private Player player;
    // Use this for initialization
    void Start()
    {

        player = FindObjectOfType<Player>();

        IdleState idleState = new IdleState();
        idleState.onEnterMethod += OnEnterIdleState;
        idleState.onUpdateMethod += OnUpDateIdleState;
        idleState.onFixedUpdateMethod += OnFixedUpDateIdleState;
        RunState runState = new RunState();
        runState.onFixedUpdateMethod += OnFixedUpDateRunState;
        runState.onUpdateMethod += OnUpDateRunState;
        runState.onEnterMethod += OnEnterRunState;
        JumpState jumpState = new JumpState();
        jumpState.onEnterMethod += OnEnterJumpState;
        AttackIdleState AttackIdleState = new AttackIdleState();
        AttackIdleState.onEnterMethod += OnEnterAttackIdleState;
        AttackIdleState.onUpdateMethod += OnUpdateAttackIdleState;

        AttackWalkState attackWalkState = new AttackWalkState();
        attackWalkState.onFixedUpdateMethod += OnFixedUpDateAttackWalkState;
        attackWalkState.onUpdateMethod += OnUpDateAttackWalkState;
        attackWalkState.onEnterMethod += OnEnterAttackWalkState;

        fsmSystem = new FsmSystem();
        fsmSystem.AddState(idleState);
        fsmSystem.AddState(runState);
        fsmSystem.AddState(jumpState);
        fsmSystem.AddState(AttackIdleState);
        fsmSystem.AddState(attackWalkState);
        fsmSystem.ChangeState(StateType.FSM_IDLE);

    }



    // Update is called once per frame
    void Update()
    {
        fsmSystem.UpdateState();
        fsmSystem.currentState.TriggerEvent(fsmSystem, this.gameObject);
    }


    private void FixedUpdate()
    {
        fsmSystem.FixedUpdateState();
    }

    private void OnEnterIdleState()
    {
        player.OnEnterIdleState();
    }

    private void OnUpDateIdleState()
    {
        player.OnUpDateIdleState();
    }
    
    private void OnFixedUpDateIdleState()
    {
        player.Move();
        player.Rotating();
    }

    private void OnEnterRunState()
    {
        player.OnEnterRunState();
    }

    private void OnUpDateRunState()
    {
        player.OnUpDateRunState();
    }

    public void OnFixedUpDateRunState()
    {
        player.Move();
        player.Rotating();
    }

    private void OnEnterJumpState()
    {
        fsmSystem.IsCanChange = false;
        player.OnEnterJumpState();
    }

    private void OnEnterAttackIdleState()
    {
        player.OnEnterAttackIdleState();
    }

    private void OnUpdateAttackIdleState()
    {
        player.OnUpdateAttackIdleState();
    }

    private void OnEnterAttackWalkState()
    {
        player.OnEnterAttackWalkState();
    }

    private void OnUpDateAttackWalkState()
    {
        player.OnUpDateAttackWalkState();
    }

    public void OnFixedUpDateAttackWalkState()
    {
        player.Move();
        player.Rotating();
    }

}
