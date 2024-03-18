using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new();

    protected BaseState<EState> CurrentState;

    protected bool IsTransitioningState = false;

    [Header("For Debug Purposes, Doesn't Affect State Machine")]
    public EState CurrentStateDebug;

    void Start()
    {
        CurrentState.EnterState();
    }
    void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if(!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
        
    }

    public void TransitionToState(EState stateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;

        CurrentStateDebug = stateKey;
    }

    void OnTriggerEnter(Collider other)
    {
        CurrentState.OnTriggerEnter(other);
    }

    void OnTriggerStay(Collider other)
    {
        CurrentState.OnTriggerStay(other);
    }

    void OnTriggerExit(Collider other)
    {
        CurrentState.OnTriggerExit(other);
    }
}

