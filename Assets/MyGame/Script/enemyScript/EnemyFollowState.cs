using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowState : IStateBase {

    public EnemyFollowState()
    {
        stateType = StateType.EnemyFollews;
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        float distance = (player.transform.position - gameObject.transform.position).magnitude;
        if (distance > 5)
        {
            fsmSystem.IsCanChange = true;
            fsmSystem.ChangeState(StateType.EnemyPatrol);
        }
    }
}
