using CharacterTransactions;
using UnityEngine;
using System.Collections.Generic;

public class StateMachineLogicBuilder
{
    private RuleForPoint _toPoint = ScriptableObject.CreateInstance<RuleForPoint>();
    private RuleForEnemy _toEnemy = ScriptableObject.CreateInstance<RuleForEnemy>();
    private RuleForAlly _toAlly = ScriptableObject.CreateInstance<RuleForAlly>();
    private RuleForEmpty _toEmpty = ScriptableObject.CreateInstance<RuleForEmpty>();
    private List<TransactionChooserObserver> _userInputTransactions = new List<TransactionChooserObserver>();
    private List<ITransaction> _uncontrollableTransactions = new List<ITransaction>();
    private List<CharacterState> _activeState = new List<CharacterState>();
    private List<CharacterState> _uncontrollableState = new List<CharacterState>();
    private CharacterState _idle;
    private TransactionChooserObserver _stopWhenTargetDie;

    public void Build(CharacterStateMachine machine, AllLogics logics, IFightable current)
    {
        CreateBasicLogics(logics.TargetObserve);
        CreateActiveLogics(logics.Fight, logics.Move, current);
        CreateUncontrollableLogic(logics.Dyeing, current);
        LoadToActiveStartAllTransactions();
        machine.Init(_idle, logics.TargetObserve, _userInputTransactions);
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

    private void CreateUncontrollableLogic(CharacterDyeingLogic dying, IFightable current)
    {
        CreateDyingLogic(dying, current);
    }

    private void CreateTargetPointLogic(CharacterMoveLogic mover, IFightable current)
    {
        CharacterState move = new CharacterState(mover, "Move to point");
        _activeState.Add(move);
        ITransaction stateAfterMove = new TransactionReacherObserver(mover, _idle, TransactionType.ReacherObserver);
        move.AddTransaction(stateAfterMove);
        _userInputTransactions.Add(new TransactionChooserObserver(_toPoint, current, move, TransactionType.ChooserObserver));
    }

    private void CreateTargetAllyLogic(CharacterMoveLogic mover, IFightable current)
    {
        CharacterState move = new CharacterState(mover, "Move to ally");

        _activeState.Add(move);

        ITransaction moveWhenLostTarget = new TransactionChooserObserver(_toAlly, current, move, TransactionType.StaticChooserObserver, mover);
        ITransaction stateAfterMove = new TransactionReacherObserver(mover, _idle, TransactionType.ReacherObserver);

        _idle.AddTransaction(moveWhenLostTarget);
        move.AddTransaction(stateAfterMove);
        move.AddTransaction(_stopWhenTargetDie);

        _userInputTransactions.Add(new TransactionChooserObserver(_toAlly, current, move, TransactionType.ChooserObserver));
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

        _userInputTransactions.Add(new TransactionChooserObserver(_toEnemy, current, move, TransactionType.ChooserObserver));
    }

    private void CreateDyingLogic(CharacterDyeingLogic dyingLogic, IFightable current)
    {
        CharacterState dyeing = new CharacterState(dyingLogic, "Dying");
        _uncontrollableState.Add(dyeing);

        ITransaction offAfterDying = new TransactionReacherObserver(dyingLogic, null, TransactionType.ReacherObserver);

        dyeing.AddTransaction(offAfterDying);

        _uncontrollableTransactions.Add(new TransactionOnCharacterDied(dyeing, current, TransactionType.CharacterDieObserver));
    }

    private void LoadToActiveStartAllTransactions()
    {
        foreach (CharacterState state in _activeState)
        {
            foreach (ITransaction transaction in _userInputTransactions)
                state.AddTransaction(transaction);

            state.AddTransaction(_uncontrollableTransactions);
        }
    }

    private void Clear()
    {
        _userInputTransactions.Clear();
        _activeState.Clear();
        _idle = null;
    }
}
