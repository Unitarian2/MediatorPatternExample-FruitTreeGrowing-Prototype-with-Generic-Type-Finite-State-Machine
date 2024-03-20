using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Junk State Player Decayed olmu� bir fruit'i inventory'sine al�rsa ba�lar. Inventory'den b�rak�l�rsa tekrar Decayed State'e ge�ilir.
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
