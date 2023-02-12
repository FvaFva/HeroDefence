using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState : MonoBehaviour
{
    [SerializeField] private List<CharacterStateTransaction> _transactions = new List<CharacterStateTransaction>();

    protected IReachLogic _reacher;

    public event Action<CharacterState, Target> OnFindNextState;

    public void Init(IReachLogic reacher)
    {
        _reacher = reacher;
    }

    public void Enter(ICharacterComander comander, Target target)
    {
        foreach(CharacterStateTransaction transaction in _transactions)
        {
            transaction.Init(comander, target);
            transaction.Activited += OnTansactionActivated;
        }
    }

    public void Exit()
    {
        foreach (CharacterStateTransaction transaction in _transactions)
        {
            transaction.Off();
            transaction.Activited -= OnTansactionActivated;
        }
    }

    private void OnTansactionActivated (CharacterState nextState, Target target)
    {
        OnFindNextState?.Invoke(nextState, target);
    }

    public abstract IEnumerator Action();
}
