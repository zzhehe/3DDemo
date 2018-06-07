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
        RunState runState = new RunState();
        runState.onFixedUpdateMethod += OnFixedUpDateRunState;
        JumpState jumpState = new JumpState();
        jumpState.onEnterMethod += OnEnterJumpState;
        fsmSystem = new FsmSystem();
        fsmSystem.AddState(idleState);
        fsmSystem.AddState(runState);
        fsmSystem.AddState(jumpState);
        fsmSystem.ChangeState(StateType.FSM_IDLE);

    }

    // Update is called once per frame
    void Update()
    {
        fsmSystem.UpdateState();
        fsmSystem.currentState.TriggerEvent(fsmSystem, this.gameObject);
    }
    

    public void FixedUpdate()
    {
        fsmSystem.FixedUpdateState();
    }

    private void OnEnterIdleState()
    {
        player.OnEnterIdleState();
    }
    

    public void OnFixedUpDateRunState()
    {
        player.Move();
        player.Rotating();
    }

    private void OnEnterJumpState()
    {
        fsmSystem.currentState.IsCanChange = false;
        player.OnEnterJumpState();
    }
    
    
}
