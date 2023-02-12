using System.Collections;
using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    [SerializeField] private CharacterState _baseState;

    private CharacterState _currentState;
    private Coroutine _currentStateAction;
    private CharacterMoveLogic _moveLogic;
    private CharacterFightLogic _fightLogic;
    private ICharacterComander _comander;

    public void Init(CharacterMoveLogic moveLogic, CharacterFightLogic fightLogic)
    {
        if (_baseState == null)
            enabled = true;
        
        _moveLogic = moveLogic;
        _fightLogic = fightLogic;
        Transit(_baseState, new());
    }

    public void SetNewComander(ICharacterComander comander)
    {
        _comander = comander;
        Transit(_baseState, new());
    }

    private void OnEnable()
    {
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

        if (_currentStateAction == null)
            return;
        else
            _currentStateAction = StartCoroutine(StateAction());
    }

    private IEnumerator StateAction()
    {
        return _currentState.Action();
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
            _currentState.Init(_moveLogic, _fightLogic);
            _currentState.OnFindNextState += Transit;
            _currentState.Enter(_comander, target);
        }

        RestarStateAction();
    }
}
