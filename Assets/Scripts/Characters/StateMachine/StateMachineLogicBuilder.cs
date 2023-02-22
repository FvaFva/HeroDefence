using System.Collections.Generic;
using UnityEngine;

public class StateMachineLogicBuilder
{
    private RuleMoveToPoint _toPoint = ScriptableObject.CreateInstance<RuleMoveToPoint>();
    private RuleMoveToEnemy _toEnemy = ScriptableObject.CreateInstance<RuleMoveToEnemy>();
    private RuleMoveToAlly _toAlly = ScriptableObject.CreateInstance<RuleMoveToAlly>();

    private List<ICharacterStateTransaction> _userInpuTransactions = new List<ICharacterStateTransaction>();
    private List<CharacterState> _activeState = new List<CharacterState>();
    private CharacterState _idle;

    public void Build(CharacterStateMachine machine, CharacterFightLogic attacker, CharacterMoveLogic mover, CharacterRotationLogic turner, IFightebel current)
    {        
        CreateLogics(attacker, mover, turner, current);
        machine.Init(_idle);
        Clear();
    }

    private void CreateIdleState()
    {
        _idle = new CharacterState(new CharacterIdleLogic(), "Idle");
        _activeState.Add(_idle);
    }    

    private void CreateLogics(CharacterFightLogic attacker, CharacterMoveLogic mover, CharacterRotationLogic turner, IFightebel current)
    {
        CreateIdleState();
        CreateTargetPointLogic(mover, current);
        CreateTargetAllyLogic(mover, turner , current);
        CreateTargetEnemyLogic(mover, turner, attacker, current);

        LoadToActiveStatrInputTransactions();
    }

    private void CreateTargetPointLogic(CharacterMoveLogic mover, IFightebel current)
    {
        CharacterState move = new CharacterState(mover, "Move to point");
        _activeState.Add(move);
        ICharacterStateTransaction stateAfterMove = new CharacterStateTransactionReach(mover, _idle);              
        move.AddTransaction(stateAfterMove);
        _userInpuTransactions.Add(new CharacterStateTransactionComander(_toPoint, current, move));
    }

    private void CreateTargetAllyLogic(CharacterMoveLogic mover, CharacterRotationLogic turner, IFightebel current)
    {
        CharacterState move = new CharacterState(mover, "Move to ally");
        CharacterState rotate = new CharacterState(turner, "Rotate to ally");

        _activeState.Add(move);
        _activeState.Add(rotate);

        ICharacterStateTransaction moveWhenLostAgle = new CharacterStateTransactionComander(_toAlly, current, move, turner);
        ICharacterStateTransaction moveWhenLostDistance = new CharacterStateTransactionComander(_toAlly, current, move, mover);
        ICharacterStateTransaction rotateAfterMove = new CharacterStateTransactionReach(mover, rotate);
        ICharacterStateTransaction stateAfterRotate = new CharacterStateTransactionReach(turner, _idle);

        _idle.AddTransaction(moveWhenLostDistance);
        _idle.AddTransaction(moveWhenLostAgle);

        rotate.AddTransaction(stateAfterRotate);
        rotate.AddTransaction(moveWhenLostDistance);

        move.AddTransaction(rotateAfterMove);

        _userInpuTransactions.Add(new CharacterStateTransactionComander(_toAlly, current, move));
    }

    private void CreateTargetEnemyLogic(CharacterMoveLogic mover, CharacterRotationLogic turner, CharacterFightLogic attacker, IFightebel current)
    {
        CharacterState move = new CharacterState(mover, "Move to enemy");
        CharacterState rotate = new CharacterState(turner, "Rotate to enemy");
        CharacterState attack = new CharacterState(attacker, "Attack enemy");

        _activeState.Add(move);
        _activeState.Add(rotate);
        _activeState.Add(attack);

        ICharacterStateTransaction moveWhenLostDistance = new CharacterStateTransactionComander(_toEnemy, current, move, mover);
        ICharacterStateTransaction moveWhenLostAgle = new CharacterStateTransactionComander(_toEnemy, current, move, turner);

        ICharacterStateTransaction rotateAfterMove = new CharacterStateTransactionReach(mover, rotate);
        ICharacterStateTransaction attackAfterRotate = new CharacterStateTransactionReach(turner, attack);
        ICharacterStateTransaction stateAfterAttack = new CharacterStateTransactionReach(attacker, _idle);

        move.AddTransaction(rotateAfterMove);
        rotate.AddTransaction(attackAfterRotate);
        attack.AddTransaction(stateAfterAttack);

        rotate.AddTransaction(moveWhenLostDistance);
        attack.AddTransaction(moveWhenLostDistance);
        attack.AddTransaction(moveWhenLostAgle);

        _userInpuTransactions.Add(new CharacterStateTransactionComander(_toEnemy, current, move));
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
