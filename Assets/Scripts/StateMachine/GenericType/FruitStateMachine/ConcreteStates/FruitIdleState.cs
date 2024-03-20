using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitIdleState : FruitState
{
    //BOÞ STATE, DÝÐER STATE'LERE BAÐLANTI SAÐLAMAZ
    public FruitIdleState(FruitContext fruitContext, FruitStateMachine.EFruitState estate) : base(fruitContext, estate)
    {
        FruitContext fruitCtx = fruitContext;
    }


    public override void EnterState()
    {
        Context.Rigidbody.useGravity = false;
    }
    public override void ExitState()
    {

    }

    public override FruitStateMachine.EFruitState GetNextState()
    {
        return StateKey;
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
    }
}
