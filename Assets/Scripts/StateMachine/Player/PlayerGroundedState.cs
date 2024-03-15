using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{


    public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory playerStateFactory) : base(context, playerStateFactory)
    { 
        isRootState = true;
        
    }

    public void HandleGravity()
    {
        ctx.CurrentMovementY = ctx.AppliedMovementY = ctx.Gravity;
    }

    public override void CheckSwitchStates()
    {
        //Debug.Log(ctx.CharacterController.isGrounded);
        //Z�plama tu�una bas�lm��sa, State de�i�iyoruz.
        if (ctx.IsJumpPressed && !ctx.RequireNewJumpPress)
        {
            SwitchState(factory.Jump());
        }
        //Z�plama tu�una bas�lmam��sa ve player yerde de�ilse, fall state ge�iyoruz.
        else if (!ctx.CharacterController.isGrounded)
        {
            
            SwitchState(factory.Fall());
        }
    }

    public override void EnterState()
    {
        //Sub State'i EnterState'de �a��r�yoruz ��nk� StateFactory state'leri bir dictionary'de cache'liyor. Bu state'e her girdi�imiz'de Substate'lerden birini se�memiz gerekli.
        InitializeSubState();
        HandleGravity();
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        if(!ctx.IsMovementPressed && ctx.IsRunPressed)
        {
            SetSubState(factory.Idle());
        }
        else if (ctx.IsMovementPressed && !ctx.IsRunPressed)
        {
            SetSubState(factory.Walk());
        }
        else
        {
            SetSubState(factory.Run());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
