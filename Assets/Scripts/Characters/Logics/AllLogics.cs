public struct AllLogics
{
    public CharacterFightLogic Fight { get; private set; }
    public CharacterDieingLogic Dieing{ get; private set; }
    public CharacterTargetObserveLogic TargetObserve { get; private set; }
    public CharacterMoveLogic Move { get; private set; }

    public AllLogics(CharacterFightLogic fight, CharacterDieingLogic dieing, CharacterTargetObserveLogic targetObserve, CharacterMoveLogic move )
    {
        Fight = fight;
        Dieing = dieing;
        TargetObserve = targetObserve;
        Move = move;
    }
}
