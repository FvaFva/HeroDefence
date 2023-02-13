using UnityEngine;

public abstract class TransactionRule : ScriptableObject
{
    public abstract bool CheckSuitableTarget(Target target, Team team);
}
