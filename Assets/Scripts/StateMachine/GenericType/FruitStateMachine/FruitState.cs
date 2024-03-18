using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FruitState : BaseState<FruitStateMachine.EFruitState>
{
    protected FruitContext Context;

    //Fruit State'ler zaman temelli state deðiþimi saðlayacaklar.
    //Current Timer state'in kaçýncý saniyesinde olduðunu belirtir.
    //Current Rate State'in süresinin yüzde kaçýný tamamladýðýný belirtir.
    protected float CurrentTimer;
    protected float CurrentRate;
    protected FruitState(FruitContext context, FruitStateMachine.EFruitState key) : base(key)
    {
        Context = context;
    }
}
