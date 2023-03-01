public class RuleForPoint : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, IFightable current)
    {
        return target.IsFightebel == false;
    }
}
