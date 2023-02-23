public class RuleMoveToEnemy : TransactionRule
{
    public override bool CheckSuitableTarget(Target target, IFightable _current)
    {        
        return target.TryGetFightebel(out IFightable fightebel) && fightebel.IsFriendly(_current) == false;
    }
}
