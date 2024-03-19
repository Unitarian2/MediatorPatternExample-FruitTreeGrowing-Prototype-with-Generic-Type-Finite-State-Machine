using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

public class TreeStateMachine : StateManager<TreeStateMachine.ETreeState>
{
    public enum ETreeState
    {
        Idle,
        Active,
    }

    private TreeContext _context;

    [Header("Fruit References")]
    [SerializeField] private Transform spawnPointParent;
    [SerializeField] private Transform spawnParent;
    FruitFactory fruitFactory;

    [Header("ScriptableObject Data")]
    [SerializeField] private TreeSettings _settings;

    private void Awake()
    {
        fruitFactory = GetComponent<FruitFactory>();

        _context = new(_settings, spawnPointParent.GetAllChildrenAsGameObject(), spawnParent, fruitFactory);
        _context.Init();

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(ETreeState.Idle, new TreeIdleState(_context, ETreeState.Idle));
        States.Add(ETreeState.Active, new TreeActiveState(_context, ETreeState.Active));
        
        CurrentState = States[ETreeState.Idle];
    }

}
