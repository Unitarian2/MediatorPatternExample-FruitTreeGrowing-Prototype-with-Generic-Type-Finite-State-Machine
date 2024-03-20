using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FruitDecayedState : FruitState
{
    public FruitDecayedState(FruitContext fruitContext, FruitStateMachine.EFruitState estate) : base(fruitContext, estate)
    {
        FruitContext fruitCtx = fruitContext;
    }


    public override void EnterState()
    {
        CurrentTimer = 0f;
    }
    public override void ExitState()
    {

    }

    public override FruitStateMachine.EFruitState GetNextState()
    {
        if (!Context.IsInventoryItem)
        {
            if (Context.DestroyTime <= CurrentTimer)
            {
                Context.StateMachine.DeactivateFruit();
                Debug.Log("Fruit Destroyed");
            }
            return StateKey;
        }
        else//Envantere alýnmýþ.
        {
            return FruitStateMachine.EFruitState.Junk;
        }

    }

    public override void OnTriggerEnter(Collider other)
    {

    }

    public override void OnTriggerExit(Collider other)
    {

    }

    public override void OnTriggerStay(Collider other)
    {

    }

    public override void UpdateState()
    {
        CurrentTimer += Time.deltaTime;
    }
}
