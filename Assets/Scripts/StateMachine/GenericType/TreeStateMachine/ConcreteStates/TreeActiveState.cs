using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeActiveState : TreeState
{
    public TreeActiveState(TreeContext treeContext, TreeStateMachine.ETreeState estate) : base(treeContext, estate)
    {
        TreeContext ctx = treeContext;
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
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
        
    }
}
