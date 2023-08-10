using CharacterTransactions;
using UnityEngine;
using System.Collections.Generic;

public class CharacterStateMachine : MonoBehaviour
{
    private CharacterState _baseState;
    private CharacterState _currentState;
    private Coroutine _currentStateAction;
    private List<TransactionChooserObserver> _dependentCommanderTransactions = new List<TransactionChooserObserver>();
    private CharacterTargetObserveLogic _targetObserver;

    public void Init(CharacterState baseStat, CharacterTargetObserveLogic targetObserver, List<TransactionChooserObserver> dependentCommanderTransactions)
    {
        _baseState = baseStat;

        if (_baseState == null)
            enabled = false;

        foreach (TransactionChooserObserver transaction in dependentCommanderTransactions)
            _dependentCommanderTransactions.Add(transaction);

        _targetObserver = targetObserver;
        Transit(_baseState, default(Target));
    }

    public void SetNewCommander(ITargetChooser commander)
    {
        foreach (TransactionChooserObserver transaction in _dependentCommanderTransactions)
            transaction.SetCommander(commander);
    }

    private void OnDisable()
    {
        Transit(null, default(Target));
    }

    private void Transit(CharacterState nextState, Target target)
    {
        if (_currentState != null)
        {
            if (_currentStateAction != null)
                StopCoroutine(_currentStateAction);

            _currentState.OnFindNextState -= Transit;
            _currentState.Exit();
        }

        _currentState = nextState;

        if (_currentState != null)
        {
            _currentState.OnFindNextState += Transit;
            _currentState.Enter(target);
            _targetObserver.SetTarget(target);
            _currentStateAction = StartCoroutine(_currentState.ReachTarget);
        }
        else
        {
            _targetObserver.SetTarget(default(Target));
        }
    }
}
