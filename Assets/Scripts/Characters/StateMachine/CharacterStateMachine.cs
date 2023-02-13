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
        _baseState= baseStat;

        if (_baseState == null)
            enabled = true;
    }

    public void SetNewComander(ICharacterComander comander)
    {
        _comander = comander;

        if(comander != null)
            Transit(_baseState, new());
    }

    private void OnDisable()
    {
        Transit(null, new());
        RestarStateAction();
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
