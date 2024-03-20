using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;
using Color = UnityEngine.Color;

public class FruitGrowingState : FruitState
{
    public FruitGrowingState(FruitContext fruitContext, FruitStateMachine.EFruitState estate) : base(fruitContext, estate)
    {
        FruitContext fruitCtx = fruitContext;
    }


    public override void EnterState()
    {
        Debug.Log("Started Growing");
        CurrentTimer = 0;
        CurrentRate = 1f / Context.Settings.GrowTime;//Rate hesaplamasý için lazým
        Context.Transform.localScale = new Vector3(Context.Settings.startSize, Context.Settings.startSize, Context.Settings.startSize);//Fruit baþlangýç size'a getiriliyor.
    }
    public override void ExitState()
    {   
        Debug.Log("Stopped Growing After : " + CurrentTimer + " Seconds");
        CurrentTimer = 0;
    }

    public override FruitStateMachine.EFruitState GetNextState()
    {
        if(Context.Settings.GrowTime <= CurrentTimer)
        {
            //Fruit büyüdü. Mediator'a haber ver
            if(Context.Rigidbody != null) Context.Rigidbody.useGravity = true;// Meyveyi aðaçtan kopardýk.
            return FruitStateMachine.EFruitState.Ripening;
        }

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
        CurrentTimer += Time.deltaTime;

        float growProgress = Mathf.Clamp01(CurrentTimer * CurrentRate); // Büyüme ilerlemesi
        float size = Mathf.Lerp(Context.Settings.startSize, Context.Settings.endSize, growProgress);//Fruit büyüme süresi boyunca büyümeli
        Context.Transform.localScale = new Vector3(size, size, size);

        Color newColor = Color.Lerp(Context.Settings.StartColor, Context.Settings.GrewColor, growProgress);//Materyal rengini de deðiþiyoruz.
        Context.ChangeColor(newColor);
    }

}
