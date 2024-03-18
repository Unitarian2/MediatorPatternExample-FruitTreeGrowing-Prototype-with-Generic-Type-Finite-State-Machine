using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FruitState : BaseState<FruitStateMachine.EFruitState>
{
    protected FruitContext Context;

    //Fruit State'ler zaman temelli state de�i�imi sa�layacaklar.
    //Current Timer state'in ka��nc� saniyesinde oldu�unu belirtir.
    //Current Rate State'in s�resinin y�zde ka��n� tamamlad���n� belirtir.
    protected float CurrentTimer;
    protected float CurrentRate;
    protected FruitState(FruitContext context, FruitStateMachine.EFruitState key) : base(key)
    {
        Context = context;
    }
}
