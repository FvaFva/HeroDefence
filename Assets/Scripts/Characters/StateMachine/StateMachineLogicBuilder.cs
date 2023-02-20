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

    public void Build(CharacterStateMachine machine, CharacterFightLogic attacker, CharacterMoveLogic mover, CharacterVisionLogic vision, IFightebel current)
    {        
        CreateLogics(attacker, mover, vision, current);
        machine.Init(_idle);
        Clear();
    }

    private void CreateIdleState()
    {
        _idle = new CharacterState(new CharacterIdleLogic());
        _activeState.Add(_idle);
    }    

    private void CreateLogics(CharacterFightLogic attacker, CharacterMoveLogic mover, CharacterVisionLogic vision, IFightebel current)
    {
        CreateIdleState();
        CreateTargetPointLogic(mover, current);
        CreateTargetAllyLogic(mover, vision , current);
        CreateTargetEnemyLogic(mover, vision, attacker, current);

        LoadToActiveStatrInputTransactions();
    }

    private void CreateTargetPointLogic(CharacterMoveLogic mover, IFightebel current)
    {
        CharacterState moveToPoint = new CharacterState(mover);
        _activeState.Add(moveToPoint);
        ICharacterStateTransaction stateAfterMove = new CharacterStateTransactionReach(mover, _idle);              
        moveToPoint.AddTransaction(stateAfterMove);
        _userInpuTransactions.Add(new CharacterStateTransactionComander(_toPoint, current, moveToPoint));
    }

    private void CreateTargetAllyLogic(CharacterMoveLogic mover, CharacterVisionLogic vision, IFightebel current)
    {
        CharacterState moveToAlly = new CharacterState(mover);
        CharacterState rotateToAlly = new CharacterState(vision);

        _activeState.Add(moveToAlly);
        _activeState.Add(rotateToAlly);

        ICharacterStateTransaction moveWhenLostAgleAlly = new CharacterStateTransactionComander(_toAlly, current, moveToAlly, vision);
        ICharacterStateTransaction moveWhenLostDistanceAlly = new CharacterStateTransactionComander(_toAlly, current, moveToAlly, mover);
        ICharacterStateTransaction rotateAfterMoveToAlly = new CharacterStateTransactionReach(mover, rotateToAlly);
        ICharacterStateTransaction stateAfterRotate = new CharacterStateTransactionReach(vision, _idle);

        _idle.AddTransaction(moveWhenLostDistanceAlly);
        _idle.AddTransaction(moveWhenLostAgleAlly);

        rotateToAlly.AddTransaction(stateAfterRotate);
        rotateToAlly.AddTransaction(moveWhenLostDistanceAlly);

        moveToAlly.AddTransaction(rotateAfterMoveToAlly);

        _userInpuTransactions.Add(new CharacterStateTransactionComander(_toAlly, current, moveToAlly));
    }

    private void CreateTargetEnemyLogic(CharacterMoveLogic mover, CharacterVisionLogic vision, CharacterFightLogic attacker, IFightebel current)
    {
        CharacterState moveToEnemy = new CharacterState(mover);
        CharacterState rotateToEnemy = new CharacterState(vision);
        CharacterState fight = new CharacterState(attacker);

        _activeState.Add(moveToEnemy);
        _activeState.Add(rotateToEnemy);
        _activeState.Add(fight);

        ICharacterStateTransaction moveWhenLostDistanceEnemy = new CharacterStateTransactionComander(_toEnemy, current, moveToEnemy, mover);
        ICharacterStateTransaction rotateWhenLostAgleEnemy = new CharacterStateTransactionComander(_toEnemy, current, rotateToEnemy, vision);
        ICharacterStateTransaction stateAfterFight = new CharacterStateTransactionReach(attacker, _idle);
        ICharacterStateTransaction rotateAfterMoveToEnemy = new CharacterStateTransactionReach(mover, rotateToEnemy);
        ICharacterStateTransaction fightAfterRotate = new CharacterStateTransactionReach(vision, fight);

        moveToEnemy.AddTransaction(rotateAfterMoveToEnemy);
        rotateToEnemy.AddTransaction(fightAfterRotate);

        fight.AddTransaction(stateAfterFight);
        fight.AddTransaction(moveWhenLostDistanceEnemy);
        fight.AddTransaction(rotateWhenLostAgleEnemy);

        _userInpuTransactions.Add(new CharacterStateTransactionComander(_toEnemy, current, moveToEnemy));
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
