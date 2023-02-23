public class RuleMoveToPoint : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, IFightable _current)
    {
        return target.IsFightebel == false;
    }
}
