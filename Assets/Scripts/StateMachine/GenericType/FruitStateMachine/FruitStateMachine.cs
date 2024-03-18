using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitStateMachine : StateManager<FruitStateMachine.EFruitState>
{
    public enum EFruitState
    {
        Growing,
        Ripening,
        Decaying
    }
    
    private FruitContext _context;

    [Header("ScriptableObject Data")]
    [SerializeField] private FruitSettings _settings;

    private void Awake()
    {
        _context = new(_settings, transform, GetComponent<MeshRenderer>().materials);

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EFruitState.Growing, new FruitGrowingState(_context, EFruitState.Growing));
        States.Add(EFruitState.Ripening, new FruitRipeningState(_context, EFruitState.Ripening));
        States.Add(EFruitState.Decaying, new FruitDecayingState(_context, EFruitState.Decaying));

        CurrentState = States[EFruitState.Growing];
    }

    
}
