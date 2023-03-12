using System.Collections.Generic;
using UnityEngine;
using CharacterTransactions;

public class StateMachineLogicBuilder
{
    private RuleForPoint _toPoint = ScriptableObject.CreateInstance<RuleForPoint>();
    private RuleForEnemy _toEnemy = ScriptableObject.CreateInstance<RuleForEnemy>();
    private RuleForAlly _toAlly = ScriptableObject.CreateInstance<RuleForAlly>();
    private RuleForFightable _toFightable = ScriptableObject.CreateInstance<RuleForFightable>();
    private RuleForEmpty _toEmpty = ScriptableObject.CreateInstance<RuleForEmpty>();

    private List<TransactionChooserObserver> _userInpuTransactions = new List<TransactionChooserObserver>();
    private List<ITransaction> _uncontrollabaleTransactions = new List<ITransaction>();
    private List<CharacterState> _activeState = new List<CharacterState>();
    private List<CharacterState> _uncontrollabaleState = new List<CharacterState>();
    private CharacterState _idle;
    private TransactionChooserObserver _stopWhenTargetDie;

    public void Build(CharacterStateMachine machine, AllLogics logics, IFightable current)
    {
        CreateBasicLogics(logics.TargetObserve);
        CreateActiveLogics(logics.Fight, logics.Move, current);
        CreateUncontrollabaleLogic(logics.Dieing, current);
        LoadToActiveStatrAllTransactions();
        machine.Init(_idle, logics.TargetObserve, _userInpuTransactions);
        Clear();
    }

    private void CreateBasicLogics(CharacterTargetObserveLogic targetObserve)
    {
        _idle = new CharacterState(new CharacterIdleLogic(), "Idle");
        _stopWhenTargetDie = new TransactionChooserObserver(_toEmpty, null, _idle, TransactionType.TargetDieObserver, targetObserve);
        _activeState.Add(_idle);
    }    

    private void CreateActiveLogics(CharacterFightLogic attacker, CharacterMoveLogic mover, IFightable current)
    {
        CreateTargetPointLogic(mover, current);
        CreateTargetAllyLogic(mover, current);
        CreateTargetEnemyLogic(mover, attacker, current);
    }

    private void CreateUncontrollabaleLogic(CharacterDieingLogic dier, IFightable current)
    {
        CreatDethLogic(dier, current);
    }

    private void CreateTargetPointLogic(CharacterMoveLogic mover, IFightable current)
    {
        CharacterState move = new CharacterState(mover, "Move to point");
        _activeState.Add(move);
        ITransaction stateAfterMove = new TransactionReacherObserver(mover, _idle, TransactionType.ReacherObserver);              
        move.AddTransaction(stateAfterMove);
        _userInpuTransactions.Add(new TransactionChooserObserver(_toPoint, current, move, TransactionType.ChooserObserver));
    }

    private void CreateTargetAllyLogic(CharacterMoveLogic mover, IFightable current)
    {
        CharacterState move = new CharacterState(mover, "Move to ally");

        _activeState.Add(move);

        ITransaction moveWhenLostTarget = new TransactionChooserObserver(_toAlly, current, move,TransactionType.StaticChooserObserver, mover);
        ITransaction stateAfterMove = new TransactionReacherObserver(mover, _idle, TransactionType.ReacherObserver);

        _idle.AddTransaction(moveWhenLostTarget);
        move.AddTransaction(stateAfterMove);
        move.AddTransaction(_stopWhenTargetDie);

        _userInpuTransactions.Add(new TransactionChooserObserver(_toAlly, current, move, TransactionType.ChooserObserver));
    }

    private void CreateTargetEnemyLogic(CharacterMoveLogic mover, CharacterFightLogic attacker, IFightable current)
    {
        CharacterState move = new CharacterState(mover, "Move to enemy");
        CharacterState attack = new CharacterState(attacker, "Attack enemy");

        _activeState.Add(move);
        _activeState.Add(attack);

        ITransaction moveWhenLostTarget = new TransactionChooserObserver(_toEnemy, current, move, TransactionType.StaticChooserObserver, mover);
        ITransaction attackAfterMove = new TransactionReacherObserver(mover, attack, TransactionType.ReacherObserver);
        ITransaction stateAfterAttack = new TransactionReacherObserver(attacker, _idle, TransactionType.ReacherObserver);

        move.AddTransaction(attackAfterMove);
        move.AddTransaction(_stopWhenTargetDie);
        attack.AddTransaction(stateAfterAttack);
        attack.AddTransaction(moveWhenLostTarget);
        attack.AddTransaction(_stopWhenTargetDie);

        _userInpuTransactions.Add(new TransactionChooserObserver(_toEnemy, current, move, TransactionType.ChooserObserver));
    }

    private void CreatDethLogic(CharacterDieingLogic dier, IFightable current)
    {
        CharacterState dieing = new CharacterState(dier, "Dieing");
        _uncontrollabaleState.Add(dieing);

        ITransaction offAfterDieing = new TransactionReacherObserver(dier, null, TransactionType.ReacherObserver);

        dieing.AddTransaction(offAfterDieing);

        _uncontrollabaleTransactions.Add(new TransactionOnCharacterDied(dieing, current, TransactionType.CharacterDieObserver));
    }

    private void LoadToActiveStatrAllTransactions()
    {
        foreach(CharacterState state in _activeState)
        {
            foreach (ITransaction transaction in _userInpuTransactions)
                state.AddTransaction(transaction);

            state.AddTransaction(_uncontrollabaleTransactions);
        }
    }

    private void Clear()
    {
        _userInpuTransactions.Clear();
        _activeState.Clear();
        _idle = null;
    }
}
