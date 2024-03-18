using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TreeState : BaseState<TreeStateMachine.ETreeState>
{
    protected TreeContext Context;

    protected TreeState(TreeContext context, TreeStateMachine.ETreeState key) : base(key)
    {
        Context = context;
    }
}
