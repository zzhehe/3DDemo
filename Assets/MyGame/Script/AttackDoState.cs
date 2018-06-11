using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDoState : IStateBase {

    public AttackDoState()
    {
        stateType = StateType.FSM_ATTACK;
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
        //
        AnimatorStateInfo currentBaseState = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1);
        if (currentBaseState.normalizedTime > 1 && (currentBaseState.IsName("Jab") || currentBaseState.IsName("Jab 0")))
        {
            fsmSystem.IsCanChange = true;
            fsmSystem.ChangeState(StateType.FSM_ATTACKIDLE);
        }
    }
}
