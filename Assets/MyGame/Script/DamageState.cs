using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : IStateBase {
    public DamageState()
    {
        stateType = StateType.FSM_DAMAGE;
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
        //如果离敌人距离过远
        

    }
}
