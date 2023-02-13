using System;
using System.Collections;
public class CharacterIdleLogic : IReachLogic
{
    public event Action Reached;
    public event Action Failed;

    public IEnumerator ReachTarget()
    {
        yield return null;
    }

    public void SetTarget(Target target)
    {
    }
}
