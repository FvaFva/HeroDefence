public class RuleMoveToPoint : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, IFightebel _current)
    {
        return target.IsFightebel == false;
    }
}
