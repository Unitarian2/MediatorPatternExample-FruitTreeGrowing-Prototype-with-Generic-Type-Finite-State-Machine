

public abstract class PlayerBaseState
{
    protected bool isRootState = false;
    protected PlayerStateMachine ctx;
    protected PlayerStateFactory factory;
    protected PlayerBaseState currentSubState;
    protected PlayerBaseState currentSuperState;
    public PlayerBaseState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
    {
        ctx = context;
        factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if(currentSubState != null)
        {
            currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(PlayerBaseState newState)
    {
        ExitState();//Mevcut State'den ��k�yoruz.

        newState.EnterState();//Yeni state'e giriyoruz.

        //RootState ise context'e m�dahele edebilir. SubState'ler kendi aras�nda switch ediliyor.
        if (isRootState)
        {
            ctx.CurrentState = newState;//Mevcut state bilgisini StateMachine'de g�ncelliyoruz.
        }
        //E�er SuperState'i varsa, bu substate'in bilgisini SuperState'ine ge�iyoruz b�ylece birbirlerinden haberdar oluyorlar.
        else if (currentSuperState != null)
        {
            currentSuperState.SetSubState(newState);
        }
        
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
