using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateBase
{
    //能否改变状态
    bool IsCanChange { get; set; }
    //枚举状态
    StateType stateType { get; }
    void OnEnter(IStateChangeable stateChangeable);
    void OnUpdate(IStateChangeable stateChangeable);
    void OnFixedUpdate(IStateChangeable stateChangeable);
    void OnExit(IStateChangeable stateChangeable);
}

//状态之间的切换工作
public interface IStateChangeable
{
    void OnUpDateIdleState();
    void OnFixedUpDateIdleState();
    void OnEnterIdleState();
    void OnExitIdleState();

    void OnUpDateRunState();
    void OnFixedUpDateRunState();
    void OnEnterRunState();
    void OnExitRunState();
}


public enum StateType
{
    FSM_IDLE,
    FSM_RUN,
    FSM_DEAD,
    FSM_WALK,
    FSM_ADMOVE
}

public class IdleState : IStateBase
{


    public bool IsCanChange { get; set; }

    public StateType stateType { get { return StateType.FSM_IDLE; } }

    public void OnEnter(IStateChangeable stateChangeable)
    {
        stateChangeable.OnEnterIdleState();
    }

    public void OnUpdate(IStateChangeable stateChangeable)
    {
        stateChangeable.OnUpDateIdleState();
    }

    public void OnExit(IStateChangeable stateChangeable)
    {
        stateChangeable.OnExitIdleState();
    }

    public void OnFixedUpdate(IStateChangeable stateChangeable)
    {
        stateChangeable.OnFixedUpDateIdleState();
    }
}

public class RunState : IStateBase
{
    public bool IsCanChange { get; set; }

    public StateType stateType { get { return StateType.FSM_RUN; } }

    public void OnEnter(IStateChangeable stateChangeable)
    {
        stateChangeable.OnEnterRunState();
    }

    public void OnUpdate(IStateChangeable stateChangeable)
    {
        stateChangeable.OnUpDateRunState();
    }

    public void OnExit(IStateChangeable stateChangeable)
    {
        stateChangeable.OnExitRunState();
    }

    public void OnFixedUpdate(IStateChangeable stateChangeable)
    {
        stateChangeable.OnFixedUpDateRunState();
    }

}

public class FsmSystem
{
    List<IStateChangeable> stateMethodlist = new List<IStateChangeable>();
    List<IStateBase> statesList = new List<IStateBase>();

    public IStateBase currentState = null;

    public IStateChangeable gameObject;

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
                    currentState.OnExit(gameObject);
                    currentState = item;
                    currentState.OnEnter(gameObject);
                }
                else if (currentState == null)
                {
                    currentState = item;
                    currentState.OnEnter(gameObject);
                }
            }
        }
    }

    public void UpdateState()
    {
        currentState.OnUpdate(gameObject);
    }

    public void FixedUpdateState()
    {
        currentState.OnFixedUpdate(gameObject);
    }

}

