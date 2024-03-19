using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitStateMachine : StateManager<FruitStateMachine.EFruitState>
{
    public enum EFruitState
    {
        Growing,
        Ripening,
        Decaying,
        Idle
    }
    
    private FruitContext _context;
    private GameObject _spawnPoint;
    private TreeContext _treeContext;

    [Header("ScriptableObject Data")]
    [SerializeField] private FruitSettings _settings;

    private void Awake()
    {
        _context = new(this, _settings, transform, GetComponent<MeshRenderer>().materials);

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EFruitState.Growing, new FruitGrowingState(_context, EFruitState.Growing));
        States.Add(EFruitState.Ripening, new FruitRipeningState(_context, EFruitState.Ripening));
        States.Add(EFruitState.Decaying, new FruitDecayingState(_context, EFruitState.Decaying));

        CurrentState = States[EFruitState.Idle];
    }

    public void ActivateFruit(GameObject spawnPoint, TreeContext treeContext)
    {
        _treeContext = treeContext;
        _spawnPoint = spawnPoint;
        CurrentState = States[EFruitState.Growing];
    }

    public void FreeTheFruit()
    {
        _treeContext.DeactivateSpawnPoint(_spawnPoint);//Fruit'i aðaçtaki slot'undan çýkartýyoruz.
        //Burada context'ten Rigidbody ulaþ ve aktifleþtir.
    }

    public void DeactivateFruit()
    {    
        CurrentState = States[EFruitState.Idle];
    }

    
}
