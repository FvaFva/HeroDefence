public struct AllLogics
{
    public AllLogics(CharacterFightLogic fight, CharacterDyeingLogic dyeing, CharacterTargetObserveLogic targetObserve, CharacterMoveLogic move)
    {
        Fight = fight;
        Dyeing = dyeing;
        TargetObserve = targetObserve;
        Move = move;
    }

    public CharacterFightLogic Fight { get; private set; }

    public CharacterDyeingLogic Dyeing { get; private set; }

    public CharacterTargetObserveLogic TargetObserve { get; private set; }

    public CharacterMoveLogic Move { get; private set; }
}
