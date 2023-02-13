using System;
using System.Collections;
using System.Collections.Generic;

public class CharacterState
{
    private List<ICharacterStateTransaction> _transactions = new List<ICharacterStateTransaction>();
    private IReachLogic _reacher;

    public event Action<CharacterState, Target> OnFindNextState;
    public IEnumerator ReachTarget => _reacher.ReachTarget();

    public CharacterState(IReachLogic reacher)
    {
        _reacher = reacher;
    }

    public void AddTransaction(ICharacterStateTransaction transaction)
    {
        if(transaction != null && _transactions.Contains(transaction) == false)
            _transactions.Add(transaction);
    }

    public void AddTransaction(List<ICharacterStateTransaction> transactions)
    {
        foreach (ICharacterStateTransaction transaction in transactions)
            AddTransaction(transaction);
    }

    public void Enter(ICharacterComander comander, Target target)
    {
        _reacher.SetTarget(target);

        foreach(ICharacterStateTransaction transaction in _transactions)
        {
            transaction.Activited += OnTransactionActivated;
            transaction.Activate(comander);
        }
    }

    public void Exit()
    {
        foreach (ICharacterStateTransaction transaction in _transactions)
        {
            transaction.Off();
            transaction.Activited -= OnTransactionActivated;
        }
    }

    private void OnTransactionActivated (CharacterState nextState, Target target)
    {
        OnFindNextState?.Invoke(nextState, target);
    }
}
