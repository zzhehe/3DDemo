﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RunState : IStateBase
{

    public RunState()
    {
        stateType = StateType.FSM_RUN;
    }

    public delegate void onEnterEvent();
    public onEnterEvent onEnterMethod;
    public delegate void onUpdateEvent();
    public onEnterEvent onUpdateMethod;
    public delegate void onExitEvent();
    public onEnterEvent onExitMethod;
    public delegate void onFixedUpdateEvent();
    public onEnterEvent onFixedUpdateMethod;

    public override void OnEnter()
    {
        if (onEnterMethod != null)
        {
            onEnterMethod();
        }
    }

    public override void OnUpdate()
    {
        if (onUpdateMethod != null)
        {
            onUpdateMethod();
        }
    }

    public override void OnExit()
    {
        if (onExitMethod != null)
        {
            onExitMethod();
        }
    }

    public override void OnFixedUpdate()
    {
        if (onFixedUpdateMethod != null)
        {
            onFixedUpdateMethod();
        }
    }

    public override void TriggerEvent(FsmSystem fsmSystem, GameObject gameObject)
    {
        Player player = gameObject.GetComponent<Player>();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            fsmSystem.IsCanChange = true;
            fsmSystem.ChangeState(StateType.FSM_JUMP);
        }

        if (Mathf.Abs(player.h) == 0 && Mathf.Abs(player.v) == 0)
        {
            fsmSystem.IsCanChange = true;
            if (player.IsInBattle)
            {
                fsmSystem.ChangeState(StateType.FSM_ATTACKIDLE);
            }
            else
            {
                fsmSystem.ChangeState(StateType.FSM_IDLE);
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (player.IsInBattle)
            {
                fsmSystem.IsCanChange = true;
                fsmSystem.ChangeState(StateType.FSM_ATTACK);
            }
        }

    }
}