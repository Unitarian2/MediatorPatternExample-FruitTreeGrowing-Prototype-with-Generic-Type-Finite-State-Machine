using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Junk State Player Decayed olmuþ bir fruit'i inventory'sine alýrsa baþlar. Inventory'den býrakýlýrsa tekrar Decayed State'e geçilir.
public class FruitJunkState : FruitState
{
    public FruitJunkState(FruitContext fruitContext, FruitStateMachine.EFruitState estate) : base(fruitContext, estate)
    {
        FruitContext fruitCtx = fruitContext;
    }


    public override void EnterState()
    {

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
