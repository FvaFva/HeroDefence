using System.Collections;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    private CharacterState _baseState;
    private CharacterState _currentState;
    private Coroutine _currentStateAction;
    private Coroutine _oldStateAction;
    private ICharacterComander _comander;

    private void Update()
    {
        if(_currentStateAction != _oldStateAction)
        {
            Debug.Log($"Changed form {_oldStateAction} to {_currentStateAction}");
            _oldStateAction = _currentStateAction;
        }
    }

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

            Debug.Log($"Exit {_currentState.Name}");
            _currentState.OnFindNextState -= Transit;
            _currentState.Exit();
        }

        _currentState = nextState;

        if (_currentState != null)
        {
            Debug.Log($"Enter {_currentState.Name}");
            _currentState.OnFindNextState += Transit;
            _currentState.Enter(_comander, target);
            _currentStateAction = StartCoroutine(_currentState.ReachTarget);
        }
    }
}
