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
    private bool stateChanging;

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
        AnimatorStateInfo currentBaseState = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1);
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
                if (!stateChanging)
                {
                    player.StartCoroutine(DelayToInvokeDo(() => {
                        fsmSystem.IsCanChange = true;
                        player.IsInBattle = false;
                        stateChanging = false;
                        Debug.Log("进入攻击准备状态");
                        fsmSystem.ChangeState(StateType.FSM_IDLE);
                    }, 5.0f));
                    stateChanging = true;
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            fsmSystem.IsCanChange = true;
            fsmSystem.ChangeState(StateType.FSM_ATTACK);
        }
    }

    public IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
}
