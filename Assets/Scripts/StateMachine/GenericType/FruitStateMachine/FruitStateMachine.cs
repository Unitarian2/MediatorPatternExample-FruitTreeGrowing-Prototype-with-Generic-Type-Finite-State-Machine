using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitStateMachine : StateManager<FruitStateMachine.EFruitState>
{
    public enum EFruitState
    {
        Idle,
        Growing,
        Ripening,
        Decaying,
        Decayed,
        Junk
    }
    
    //Fields
    private FruitContext _context;

    [Header("ScriptableObject Data")]
    [SerializeField] private FruitSettings _settings;

    //Properties
    public FruitContext Context => _context;

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Material[] mats = GetComponent<MeshRenderer>().materials;

        _context = new(_settings, transform, mats, rb, gameObject, this);

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EFruitState.Growing, new FruitGrowingState(_context, EFruitState.Growing));
        States.Add(EFruitState.Ripening, new FruitRipeningState(_context, EFruitState.Ripening));
        States.Add(EFruitState.Decaying, new FruitDecayingState(_context, EFruitState.Decaying));
        States.Add(EFruitState.Idle, new FruitIdleState(_context, EFruitState.Idle));
        States.Add(EFruitState.Decayed, new FruitDecayedState(_context, EFruitState.Decayed));
        States.Add(EFruitState.Junk, new FruitJunkState(_context, EFruitState.Junk));

        CurrentState = States[EFruitState.Idle];
    }

    /// <summary>
    /// Bir Fruit'in StateMachine sürecini baþlatan metod. Fruit'in spawn edildiði Tree ve Slot'un verilmesi gereklidir.
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="treeContext"></param>
    public void ActivateFruit(GameObject spawnPoint, TreeContext treeContext)
    {
        _context.SetTreeInfo(treeContext, spawnPoint);
        CurrentState = States[EFruitState.Growing];
    }

    public void DeactivateFruit()
    {
        Destroy(gameObject);
    }

}
