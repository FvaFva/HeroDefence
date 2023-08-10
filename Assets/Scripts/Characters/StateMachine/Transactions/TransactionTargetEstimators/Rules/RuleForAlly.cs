public class RuleForAlly : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, IFightable current)
    {
        return target.TryGetFightable(out IFightable fightable) && fightable.IsFriendly(current);
    }
}
