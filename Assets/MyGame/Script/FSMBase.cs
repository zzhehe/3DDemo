using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStateBase
{

    public StateType stateType { get; set; }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public abstract void OnUpdate();

    public abstract void OnFixedUpdate();

    public abstract void TriggerEvent(FsmSystem fsmSystem, GameObject obj);


}


public enum StateType
{
    FSM_IDLE,
    FSM_RUN,
    FSM_DEAD,
    FSM_ATTACKWALK,
    FSM_ADMOVE,
    FSM_JUMP,
    FSM_ATTACKIDLE,
    FSM_ATTACK,
    FSM_DAMAGE,
    EnemyPatrol,
    EnemyFollews,
    EnemyAttack
}

public class FsmSystem
{
    List<IStateBase> statesList = new List<IStateBase>();

    //能否改变状态
    public bool IsCanChange { get; set; }

    public IStateBase currentState = null;

    public void AddState(IStateBase state)
    {
        if (!statesList.Contains(state))
        {
            statesList.Add(state);
        }
    }

    public void DeleteState(StateType stateType)
    {
        foreach (var item in statesList)
        {
            if (item.stateType == stateType)
            {
                statesList.Remove(item);
                return;
            }
        }
    }

    public void ChangeState(StateType stateType)
    {
        foreach (var item in statesList)
        {
            if (item.stateType == stateType)
            {
                if (currentState != null && IsCanChange)
                {
                    currentState.OnExit();
                    currentState = item;
                    currentState.OnEnter();
                }
                else if (currentState == null)
                {
                    currentState = item;
                    currentState.OnEnter();
                }
            }
        }
    }

    public void UpdateState()
    {
        currentState.OnUpdate();
    }

    public void FixedUpdateState()
    {
        currentState.OnFixedUpdate();
    }

}

