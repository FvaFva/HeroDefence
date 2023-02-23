using System.Collections;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    private CharacterState _baseState;
    private CharacterState _currentState;
    private Coroutine _currentStateAction;
    private ICharacterComander _comander;

    public void Init(CharacterState baseStat)
    {
        _baseState = baseStat;

        if (_baseState == null)
            enabled = false;

        Transit(_baseState, new());
    }

    public void SetNewComander(ICharacterComander comander)
    {
        _comander = comander;
        _currentState.SetNewComander(_comander);
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
            _currentState.Enter(_comander, target);
            _currentStateAction = StartCoroutine(_currentState.ReachTarget);
        }
    }
}
