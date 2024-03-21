using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
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
    [SerializeField] private TreeMediator _mediator;

    //Properties
    public TreeContext Context => _context;

    private void Awake()
    {
        fruitFactory = GetComponent<FruitFactory>();

        _context = new(_settings, spawnPointParent.GetAllChildGameObject(), spawnParent, fruitFactory, _mediator);
        _context.Init();

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(ETreeState.Idle, new TreeIdleState(_context, ETreeState.Idle));
        States.Add(ETreeState.Active, new TreeActiveState(_context, ETreeState.Active));
        
        CurrentState = States[ETreeState.Idle];
    }

    public void ActivateTree()
    {
        CurrentState = States[ETreeState.Active];
    }

    private void OnEnable()
    {
        _context.OnEnable();
    }

    private void OnDisable()
    {
        _context.OnDisable();
    }
}
