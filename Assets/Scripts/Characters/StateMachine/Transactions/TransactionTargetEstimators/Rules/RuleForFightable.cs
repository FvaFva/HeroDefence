public class RuleForFightable : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, IFightable current)
    {
        return target.IsFightebel;
    }
}