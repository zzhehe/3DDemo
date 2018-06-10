using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIdleState : IStateBase {

    public AttackIdleState()
    {
        stateType = StateType.FSM_ATTACKIDLE;
    }

    public delegate void onEnterEvent();
    public onEnterEvent onEnterMethod;
    public delegate void onUpdateEvent();
    public onEnterEvent onUpdateMethod;
    public delegate void onExitEvent();
    public onEnterEvent onExitMethod;
    public delegate void onFixedUpdateEvent();
    public onEnterEvent onFixedUpdateMethod;
    private bool IsChanging;

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

        if (player.IsInBattle && Mathf.Abs(player.h) > 0.1f || Mathf.Abs(player.v) > 0.1f)
        {
            fsmSystem.IsCanChange = true;
            fsmSystem.ChangeState(StateType.FSM_ATTACKWALK);
        }

        if (!player.IsInBattle && Mathf.Abs(player.h) > 0.1f || Mathf.Abs(player.v) > 0.1f)
        {
            fsmSystem.IsCanChange = true;
            fsmSystem.ChangeState(StateType.FSM_RUN);
        }

        if (Mathf.Abs(player.h) < 0.1f && Mathf.Abs(player.v) < 0.1f)
        {
            //如果离敌人距离过远
            if (true)
            {

                //if (IsChanging)
                //{
                //    return;
                //}
                player.StartCoroutine(DelayToInvokeDo(() => {
                    fsmSystem.IsCanChange = true;
                    player.IsInBattle = false;
                    IsChanging = false;
                    fsmSystem.ChangeState(StateType.FSM_IDLE);
                }, 2.0f));
             //   IsChanging = true;

            }
        }
    }

    public IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
}
