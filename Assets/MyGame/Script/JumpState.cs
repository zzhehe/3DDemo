using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IStateBase
{

    public bool IsCanChange { get; set; }

    public StateType stateType { get { return StateType.FSM_JUMP; } }

    public delegate void onEnterEvent();
    public onEnterEvent onEnterMethod;
    public delegate void onUpdateEvent();
    public onEnterEvent onUpdateMethod;
    public delegate void onExitEvent();
    public onEnterEvent onExitMethod;
    public delegate void onFixedUpdateEvent();
    public onEnterEvent onFixedUpdateMethod;

    public void OnEnter()
    {
        if (onEnterMethod != null)
        {
            onEnterMethod();
        }
    }

    public void OnUpdate()
    {
        if (onUpdateMethod != null)
        {
            onUpdateMethod();
        }
    }

    public void OnExit()
    {
        if (onExitMethod != null)
        {
            onExitMethod();
        }
    }

    public void OnFixedUpdate()
    {
        if (onFixedUpdateMethod != null)
        {
            onFixedUpdateMethod();
        }
    }
    
}
