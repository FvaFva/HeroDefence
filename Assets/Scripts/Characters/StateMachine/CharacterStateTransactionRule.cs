using System;

public class CharacterStateTransactionRule : ICharacterStateTransaction
{
    private CharacterState _targetState;
    private ICharacterComander _comander;
    private Team _team;
    private TransactionRule _rule;

    public event Action<CharacterState, Target> Activited;

    public CharacterStateTransactionRule(TransactionRule rule, Team team, CharacterState targetState)
    {
        _rule = rule;
        _team = team;
        _targetState = targetState;
    }

    public void TryOn(ICharacterComander comander)
    {
        Off();
        
        if (comander != null)
        {
            _comander = comander;
            _comander!.ChoosedTarget += OnChooseNewTarget;
        }
    }

    public void Off()
    {
        if (_comander != null)
        {
            _comander.ChoosedTarget -= OnChooseNewTarget;
            _comander = null;
        }
    }
    
    protected void OnChooseNewTarget(Target target)
    {
        if (_rule.CheckSuitableTarget(target, _team))
        {            
            _comander!.ChoosedTarget -= OnChooseNewTarget;            
            Activited?.Invoke(_targetState, target);
        }
    }
}
