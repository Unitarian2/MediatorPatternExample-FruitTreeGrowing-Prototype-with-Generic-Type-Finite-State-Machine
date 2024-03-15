using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    IEnumerator JumpResetTimer()
    {
        yield return new WaitForSeconds(0.5f);
        ctx.JumpCount = 0;
    }
    public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory playerStateFactory) : base(context, playerStateFactory)
    {
        isRootState = true;
        
    }

    public override void CheckSwitchStates()
    {
        if (ctx.CharacterController.isGrounded)
        {
            SwitchState(factory.Grounded());
        }
    }

    public override void EnterState()
    {
        //Sub State'i EnterState'de �a��r�yoruz ��nk� StateFactory state'leri bir dictionary'de cache'liyor. Bu state'e her girdi�imiz'de Substate'lerden birini se�memiz gerekli.
        InitializeSubState();
        HandleJump();
    }

    public override void ExitState()
    {
        ctx.Animator.SetBool(ctx.IsJumpingHash, false);
        if (ctx.IsJumpPressed)
        {
            ctx.RequireNewJumpPress = true;
        }
        

        //Jump durduktan bir s�re sonra jumpCount s�f�rlan�yor. B�ylece jump combosu resetlenmi� oluyor.
        ctx.JumpResetCoroutine = ctx.StartCoroutine(JumpResetTimer());
    }

    public override void InitializeSubState()
    {
        if (!ctx.IsMovementPressed && ctx.IsRunPressed)
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
        
        HandleGravity();
        CheckSwitchStates();
    }

    public void HandleJump()
    {
        //Jump combosu tamamlanmam��sa ResetTimer durduruyoruz ki jumpCount combonun ortas�nda s�f�rlanmas�n.
        if (ctx.JumpCount < 3 && ctx.JumpResetCoroutine != null)
        {
            ctx.StopCoroutine(ctx.JumpResetCoroutine);
        }
        //Dictionary d���na ��kmamak i�in ekledik
        else if (ctx.JumpCount >= 3)
        {
            ctx.JumpCount = 0;
            if (ctx.JumpResetCoroutine != null) ctx.StopCoroutine(ctx.JumpResetCoroutine);
        }



        //Animation ba�l�yor.
        ctx.Animator.SetBool(ctx.IsJumpingHash, true);

        ctx.IsJumping = true;
        ctx.JumpCount += 1;
        ctx.Animator.SetInteger(ctx.JumpCountHash, ctx.JumpCount);

        //HandleGravity'de Velocity Verlet(yar�m ad�ml�k) hesaplama yapt���m�z i�in, initialJumpVelocity'yi yar� yar�ya d���rd�k.
        ctx.CurrentMovementY = ctx.InitialJumpVelocities[ctx.JumpCount];
        ctx.AppliedMovementY = ctx.InitialJumpVelocities[ctx.JumpCount];

    }

    void HandleGravity()
    {
        //sadece havaya y�kselirken y 0'dan b�y�k olur.
        //Jump butonuna basmay� b�rak�rsak k���k jump(fallMultiplier 1f oldu�u i�in), bas�l� tutarsak ise full jump(fallMultiplier artt��� i�in)
        bool isFalling = ctx.CurrentMovementY <= 0f || !ctx.IsJumpPressed;

        //D���yorsak, public fallspeed de�eri kullan�yoruz. Yoksa 1.0f default de�er kullan�yoruz. D��me h�z�n� art�rmak i�in kullan�l�r.
        float fallMultiplier = ctx.FallSpeed;

        
        //Player d����te. Else ile kod tekrar� olsa da ilerde buraya d���� an�na �zel kodlar eklenebilir o nedenle bu �ekilde b�rakt�k.
        if (isFalling)
        {
            //Burada Velocity Verlet entegrasyonu kulland�k. Yar�m ad�ml�k h�z g�ncellemesi yapt�k b�ylece frame rate fark�ndan do�acak sapmalardan kurtulmak ama�lan�yor.
            float previousYVelocity = ctx.CurrentMovementY;
            ctx.CurrentMovementY = ctx.CurrentMovementY + (ctx.JumpGravities[ctx.JumpCount] * fallMultiplier * Time.deltaTime);

            //NextYVelocity Clamp'liyoruz, y�ksek mesafeden a�a�� d���yorsak fallMultiplier'�m�z bizi a��r� h�zland�rmas�n diye. -20f d��erken ula�aca��m�z max d��me h�z�
            ctx.AppliedMovementY = Mathf.Max((previousYVelocity + ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
        //Di�er durumlarda Gravity uyguluyoruz.
        else
        {
            //Burada Velocity Verlet entegrasyonu kulland�k. Yar�m ad�ml�k h�z g�ncellemesi yapt�k b�ylece frame rate fark�ndan do�acak sapmalardan kurtulmak ama�lan�yor.
            float previousYVelocity = ctx.CurrentMovementY;
            ctx.CurrentMovementY = ctx.CurrentMovementY + (ctx.JumpGravities[ctx.JumpCount] * Time.deltaTime);
            ctx.AppliedMovementY = (previousYVelocity + ctx.CurrentMovementY) * 0.5f;
        }
    }
}
