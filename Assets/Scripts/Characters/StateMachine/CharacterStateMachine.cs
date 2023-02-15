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
            enabled = true;

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

    private void RestarStateAction()
    {
        if (_currentStateAction != null)
            StopCoroutine(_currentStateAction);

        if (_currentState == null)
            return;
        else
            _currentStateAction = StartCoroutine(StateAction());
    }

    private IEnumerator StateAction()
    {
        return _currentState.ReachTarget;
    }

    private void Transit(CharacterState nextState, Target target)
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
            _currentState.Enter(_comander, target);
        }

        RestarStateAction();
    }
}
