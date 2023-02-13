using System.Collections.Generic;
using UnityEngine;

public static class StateMachineLogicBuilder
{
    private static RuleMoveToPoint _toPoint = ScriptableObject.CreateInstance<RuleMoveToPoint>();
    private static RuleMoveToFightable _toFightable = ScriptableObject.CreateInstance<RuleMoveToFightable>();

    private static List<ICharacterStateTransaction> _controlebel = new List<ICharacterStateTransaction>();
    private static Dictionary<IReachLogic, ICharacterStateTransaction> _rechers = new Dictionary<IReachLogic, ICharacterStateTransaction>();
    private static CharacterState _idle;

    public static void Build(CharacterStateMachine machine, CharacterFightLogic attacker, CharacterMoveLogic mover, CharacterVisionLogic vision,Team team)
    {
        CreateIdleState();
        CreateReachersTransction(attacker, mover);
        CreateControlebelTransactions(attacker, mover, vision,team);
        _idle.AddTransaction(_controlebel);
        machine.Init(_idle);
        Clear();
    }

    private static void  CreateIdleState()
    {
        _idle = new CharacterState(new CharacterIdleLogic());        
    }

    private static void CreateReachersTransction(CharacterFightLogic attacker, CharacterMoveLogic mover)
    {
        _rechers.Add(mover, new CharacterStateTransactionReach(mover, _idle));
        _rechers.Add(attacker, new CharacterStateTransactionReach(attacker, _idle));
    }

    private static void CreateVisionTransaction(IReachLogic vision, CharacterState moveToFighter)
    {
        _rechers.Add(vision, new CharacterStateTransactionReach(vision, moveToFighter));
    }

    private static void CreateControlebelTransactions(CharacterFightLogic attacker, CharacterMoveLogic mover, CharacterVisionLogic vision, Team team)
    {
        CharacterState moveToPoint = new CharacterState(mover);
        CharacterState moveToFighter = new CharacterState(mover);

        _controlebel.Add(new CharacterStateTransactionRule(_toFightable, team, moveToFighter));
        _controlebel.Add(new CharacterStateTransactionRule(_toPoint, team, moveToPoint));

        moveToPoint.AddTransaction(_rechers[mover]);
        moveToPoint.AddTransaction(_controlebel);
        CreateVisionTransaction(vision, moveToFighter);

        moveToFighter.AddTransaction(_rechers[mover]);
        moveToFighter.AddTransaction(_controlebel);
        moveToFighter.AddTransaction(_rechers[vision]);
    }

    private static void Clear()
    {
        _controlebel.Clear();
        _rechers.Clear();
        _idle = null;
    }
}
