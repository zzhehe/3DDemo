using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWalkState : IStateBase
{

    public AttackWalkState()
    {
        stateType = StateType.FSM_ATTACKWALK;
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

    public override void OnExit()
    {
        if (onExitMethod != null)
        {
            onExitMethod();
        }
    }


    public override void OnUpdate()
    {
        if (onUpdateMethod != null)
        {
            onUpdateMethod();
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
        

        if (Mathf.Abs(player.h) < 0.1f && Mathf.Abs(player.v) < 0.1f)
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
    }
}
