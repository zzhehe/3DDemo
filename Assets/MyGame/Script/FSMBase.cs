using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IStateBase
{
    //能否改变状态
    public bool IsCanChange { get; set; }

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
    FSM_WALK,
    FSM_ADMOVE,
    FSM_JUMP
}

public class FsmSystem
{
    List<IStateBase> statesList = new List<IStateBase>();
    
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
                if (currentState != null && currentState.IsCanChange)
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

