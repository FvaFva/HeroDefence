public class RuleMoveToFightable : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, Team team)
    {
        return target.IsFightebel;
    }
}
