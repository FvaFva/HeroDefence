public class RuleForEmpty : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, IFightable current)
    {
        return target.IstEmpty == false;
    }
}
