using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeActiveState : TreeState
{
    float timer;

    public TreeActiveState(TreeContext treeContext, TreeStateMachine.ETreeState estate) : base(treeContext, estate)
    {
        TreeContext ctx = treeContext;
    }

    public override void EnterState()
    {
        timer = 0f;
    }

    public override void ExitState()
    {
        timer = 0f;
    }

    public override TreeStateMachine.ETreeState GetNextState()
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
        timer += Time.deltaTime;
        if(timer >= Context.Settings.spawnTime)
        {
            //Spawn a Fruit
            Context.SpawnFruit();
            timer = 0f;
        }
    }
}
