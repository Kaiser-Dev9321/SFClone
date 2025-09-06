public interface IEntityStateMachine
{
    IState currentState { get; set; }
    IState previousState { get; set; }

    public void ChangeState(IState newState);
    public void ExecuteStateUpdate();
}

