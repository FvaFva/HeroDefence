public class RuleMoveToAlly : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, IFightebel _current)
    {
        return target.TryGetFightebel(out IFightebel fightebel) && fightebel.IsFriendly(_current);
    }
}