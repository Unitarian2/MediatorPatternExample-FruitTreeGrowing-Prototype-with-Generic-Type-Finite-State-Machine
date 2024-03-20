using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitDecayingState : FruitState
{
    public FruitDecayingState(FruitContext fruitData, FruitStateMachine.EFruitState estate) : base(fruitData, estate)
    {
        FruitContext data = fruitData;
    }
    public override void EnterState()
    {
        CurrentTimer = 0;
        CurrentRate = 1f / Context.Settings.DecayTime;//Rate hesaplamasý için lazým
        Debug.Log("Started Decaying");
    }

    public override void ExitState()
    {
        Debug.Log("Stopped Decaying After : " + CurrentTimer + " Seconds");
        CurrentTimer = 0;
    }

    public override FruitStateMachine.EFruitState GetNextState()
    {
        if (Context.Settings.DecayTime <= CurrentTimer)
        {
            //Burada çýkmaz sokak olan Decayed State return et.
            return FruitStateMachine.EFruitState.Decayed;
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

        float decayProgress = Mathf.Clamp01(CurrentTimer * CurrentRate); // Çürüme ilerlemesi

        float size = Mathf.Lerp(Context.Settings.endSize, Context.Settings.startSize * 1.25f, decayProgress);//Fruit çürüme süresi boyunca bir niktar küçülmeli
        Context.Transform.localScale = new Vector3(size, size, size);

        Color newColor = Color.Lerp(Context.Settings.RipenedColor, Context.Settings.DecayColor, decayProgress);//Materyal rengini de deðiþiyoruz.
        Context.ChangeColor(newColor);
    }

}
