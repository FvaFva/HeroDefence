using System.Collections.Generic;
using UnityEngine;

public class StateMachineLogicBuilder
{
    private RuleMoveToPoint _toPoint = ScriptableObject.CreateInstance<RuleMoveToPoint>();
    private RuleMoveToEnemy _toEnemy = ScriptableObject.CreateInstance<RuleMoveToEnemy>();
    private RuleMoveToAlly _toAlly = ScriptableObject.CreateInstance<RuleMoveToAlly>();

    private List<ITransaction> _userInpuTransactions = new List<ITransaction>();
    private List<CharacterState> _activeState = new List<CharacterState>();
    private CharacterState _idle;

    public void Build(CharacterStateMachine machine, CharacterFightLogic attacker, CharacterMoveLogic mover, IFightable current)
    {        
        CreateLogics(attacker, mover, current);
        machine.Init(_idle);
        Clear();
    }

    private void CreateIdleState()
    {
        _idle = new CharacterState(new CharacterIdleLogic(), "Idle");
        _activeState.Add(_idle);
    }    

    private void CreateLogics(CharacterFightLogic attacker, CharacterMoveLogic mover, IFightable current)
    {
        CreateIdleState();
        CreateTargetPointLogic(mover, current);
        CreateTargetAllyLogic(mover, current);
        CreateTargetEnemyLogic(mover, attacker, current);

        LoadToActiveStatrInputTransactions();
    }

    private void CreateTargetPointLogic(CharacterMoveLogic mover, IFightable current)
    {
        CharacterState move = new CharacterState(mover, "Move to point");
        _activeState.Add(move);
        ITransaction stateAfterMove = new TransactionReachObserver(mover, _idle);              
        move.AddTransaction(stateAfterMove);
        _userInpuTransactions.Add(new TransactionComandObserver(_toPoint, current, move));
    }

    private void CreateTargetAllyLogic(CharacterMoveLogic mover, IFightable current)
    {
        CharacterState move = new CharacterState(mover, "Move to ally");

        _activeState.Add(move);

        ITransaction moveWhenLostTarget = new TransactionComandObserver(_toAlly, current, move, mover);
        ITransaction stateAfterMove = new TransactionReachObserver(mover, _idle);

        _idle.AddTransaction(moveWhenLostTarget);
        move.AddTransaction(stateAfterMove);

        _userInpuTransactions.Add(new TransactionComandObserver(_toAlly, current, move));
    }

    private void CreateTargetEnemyLogic(CharacterMoveLogic mover, CharacterFightLogic attacker, IFightable current)
    {
        CharacterState move = new CharacterState(mover, "Move to enemy");
        CharacterState attack = new CharacterState(attacker, "Attack enemy");

        _activeState.Add(move);
        _activeState.Add(attack);

        ITransaction moveWhenLostTarget = new TransactionComandObserver(_toEnemy, current, move, mover);
        ITransaction attackAfterMove = new TransactionReachObserver(mover, attack);
        ITransaction stateAfterAttack = new TransactionReachObserver(attacker, _idle);

        move.AddTransaction(attackAfterMove);
        attack.AddTransaction(stateAfterAttack);
        attack.AddTransaction(moveWhenLostTarget);

        _userInpuTransactions.Add(new TransactionComandObserver(_toEnemy, current, move));
    }

    private void LoadToActiveStatrInputTransactions()
    {
        foreach(CharacterState state in _activeState)
        {
            state.AddTransaction(_userInpuTransactions);
        }
    }

    private void Clear()
    {
        _userInpuTransactions.Clear();
        _activeState.Clear();
        _idle = null;
    }
}
