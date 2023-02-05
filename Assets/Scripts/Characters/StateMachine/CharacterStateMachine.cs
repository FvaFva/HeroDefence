using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterStateMachine : MonoBehaviour
{
    [SerializeField] private CharacterState _baseState;
    private CharacterState _currentState;
    private Character _character;
    private IFightebel _target;

    private void Awake()
    {
        if (_baseState == null)
        {
            enabled = false;
        }

        TryGetComponent<Character>(out _character);
        Transit(_baseState);
    }

    public void SetNewTarget(IFightebel target)
    {
        _target = target;
        _currentState.SetNewTarget(target);
    }

    private void Transit(CharacterState nextState)
    {
        if (_currentState != null)
        {
            _currentState.OnFindNextState -= Transit;
            _currentState.Exit();
        }

        _currentState = nextState;

        if (_currentState != null)
        {
            _currentState.OnFindNextState += Transit;
            _currentState.Enter(_target);
        }
    }
}
