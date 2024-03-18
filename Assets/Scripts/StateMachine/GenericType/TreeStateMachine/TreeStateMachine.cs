using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TreeStateMachine : StateManager<TreeStateMachine.ETreeState>
{
    public enum ETreeState
    {
        Idle,
        Active,
    }

    private TreeContext _context;

    [Header("ScriptableObject Data")]
    [SerializeField] private FruitSettings _settings;

    private void Awake()
    {
        _context = new(_settings);

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(ETreeState.Idle, new TreeIdleState(_context, ETreeState.Idle));
        States.Add(ETreeState.Active, new TreeActiveState(_context, ETreeState.Active));
        

        CurrentState = States[ETreeState.Idle];
    }
}
