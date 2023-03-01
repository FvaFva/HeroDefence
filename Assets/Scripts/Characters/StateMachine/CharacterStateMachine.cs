using UnityEngine;
using CharacterTransactions;
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
        Transit(_baseState, new Target());
    }

    public void SetNewComander(ITargetChooser comander)
    {       
        foreach(TransactionChooserObserver transaction in _dependentCommanderTransactions)
            transaction.SetComander(comander);        
    }

    private void OnDisable()
    {
        Transit(null, new());
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
            _targetObserver.SetTarget(new Target());
        }
    }
}
