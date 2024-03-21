using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitRipeningState : FruitState
{
    public FruitRipeningState(FruitContext fruitContext, FruitStateMachine.EFruitState estate) : base(fruitContext, estate)
    {
        FruitContext fruitCtx = fruitContext;
    }

    public override void EnterState()
    {
        CurrentTimer = 0;
        Debug.Log("Started Ripening");
        Context.AssignedTree.Mediator.OnStartedRipening(Context.AssignedTree);//Mediator'a haber gidiyor.
    }

    public override void ExitState()
    {
        Debug.Log("Stopped Ripening After : " + CurrentTimer + " Seconds");
        CurrentTimer = 0;
    }

    public override FruitStateMachine.EFruitState GetNextState()
    {
        if (Context.Settings.RipenTime <= CurrentTimer)
        {
            return FruitStateMachine.EFruitState.Decaying;
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

        float growProgress = Mathf.Clamp01(CurrentTimer * CurrentRate); // Olgunlaþma ilerlemesi

        Color newColor = Color.Lerp(Context.Settings.GrewColor, Context.Settings.RipenedColor, growProgress);//Materyal rengini de deðiþiyoruz.
        Context.ChangeColor(newColor);
    }

}
