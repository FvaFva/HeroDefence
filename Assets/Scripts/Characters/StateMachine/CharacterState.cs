using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState : MonoBehaviour
{
    [SerializeField] private List<CharacterStateTransaction> _transactions = new List<CharacterStateTransaction>();
    private Coroutine actionWorc;

    public event Action<CharacterState> OnFindNextState;

    public void Enter(IFightebel target)
    {
        foreach(CharacterStateTransaction transaction in _transactions)
        {
            transaction.Init(target);
            transaction.Activited += OnTansactionActivated;
        }

        RestartAction(target);
    }

    public void SetNewTarget(IFightebel target)
    {
        foreach (CharacterStateTransaction transaction in _transactions)
            transaction.SetNewTarget(target);

        RestartAction(target);
    }

    public void Exit()
    {
        foreach (CharacterStateTransaction transaction in _transactions)
        {
            transaction.Off();
            transaction.Activited -= OnTansactionActivated;
        }

        RestartAction(null);
    }

    private void RestartAction(IFightebel target)
    {
        if (actionWorc != null)
            StopCoroutine(actionWorc);

        if(target != null)
            actionWorc = StartCoroutine(Action(target));
    }

    private void OnTansactionActivated (CharacterState nextState)
    {
        OnFindNextState?.Invoke(nextState);
    }

    protected abstract IEnumerator Action(IFightebel target);
}
