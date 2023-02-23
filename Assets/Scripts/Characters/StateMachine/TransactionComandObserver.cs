using System;

public class TransactionComandObserver : ITransaction
{
    private CharacterState _targetState;
    private ICharacterComander _comander;
    private IFightable _current;
    private TransactionRule _rule;
    private bool _staticComander;

    public event Action<CharacterState, Target> Activited;

    public TransactionComandObserver(TransactionRule rule, IFightable current, CharacterState targetState, ICharacterComander comander = null)
    {
        _rule = rule;
        _current = current;
        _targetState = targetState;
        _staticComander = comander != null;
        _comander = comander;
    }

    public void TryOn(ICharacterComander comander)
    {
        DisabledComander();
        
        if (_staticComander)
        {
            _comander.ChoosedTarget += OnChooseNewTarget;
        }
        else if(comander != null)
        {
            _comander = comander;
            _comander.ChoosedTarget += OnChooseNewTarget;
        }
    }

    public void Off()
    {
        if (_comander != null)
        {
            _comander.ChoosedTarget -= OnChooseNewTarget;

            if (_staticComander == false)
                _comander = null;
        }
    }

    private void DisabledComander()
    {
        if (_comander != null && _staticComander == false)
        {
            _comander.ChoosedTarget -= OnChooseNewTarget;
            _comander = null;
        }
    }

    private void OnChooseNewTarget(Target target)
    {
        if (target.IsIFightebelMatches(_current) == false && _rule.CheckSuitableTarget(target, _current))
        {
            _comander!.ChoosedTarget -= OnChooseNewTarget;            
            Activited?.Invoke(_targetState, target);
        }
    }
}
