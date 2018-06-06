using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateBase
{
    //能否改变状态
    bool IsCanChange { get; set; }
    //枚举状态
    StateType stateType { get; }
    void OnEnter();
    void OnUpdate();
    void OnFixedUpdate();
    void OnExit();
    
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

public class IdleState : IStateBase
{


    public bool IsCanChange { get; set; }

    public StateType stateType { get { return StateType.FSM_IDLE; } }

    //public onEnterEvent onEnterMethod { get; private set; }
    public delegate void onEnterEvent();
    public event onEnterEvent onEnterMethod;
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

public class RunState : IStateBase
{
    
    public bool IsCanChange { get; set; }

    public StateType stateType { get { return StateType.FSM_RUN; } }

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

