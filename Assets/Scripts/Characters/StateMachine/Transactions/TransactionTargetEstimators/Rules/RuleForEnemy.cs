public class RuleForEnemy : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, IFightable current)
    {        
        return target.TryGetFightebel(out IFightable fightebel) && fightebel.IsFriendly(current) == false;
    }
}
