using System;

public class CharacterStateTransactionReach : ICharacterStateTransaction
{
    private CharacterState _targetState;
    private IReachLogic _reacher;

    public event Action<CharacterState, Target> Activited;

    public CharacterStateTransactionReach(IReachLogic reacher, CharacterState targetState)
    {
        _reacher = reacher;
        _targetState = targetState;
    }

    public void Activate(ICharacterComander comander)
    {   
        _reacher!.Reached += OnReachTarget;
    }

    public void Off()
    {                        
        _reacher!.Reached -= OnReachTarget;
    }    

    private void OnReachTarget()
    {
        _reacher!.Reached -= OnReachTarget;
        Activited?.Invoke(_targetState, new());
    }
}
