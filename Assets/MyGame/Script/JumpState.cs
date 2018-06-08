using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IStateBase
{
    public JumpState()
    {
        stateType = StateType.FSM_JUMP;
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
        //现在的动画状态
        AnimatorStateInfo currentBaseState = gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (currentBaseState.normalizedTime > 1 && currentBaseState.IsName("Jump"))
        {
            fsmSystem.IsCanChange = true;
            fsmSystem.ChangeState(StateType.FSM_IDLE);
        }
    }
}
