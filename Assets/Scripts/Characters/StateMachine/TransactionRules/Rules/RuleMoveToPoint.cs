public class RuleMoveToPoint : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, Team team)
    {
        return target.IsFightebel == false;
    }
}
