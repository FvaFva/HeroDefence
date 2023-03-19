using System;
using System.Collections;
public class CharacterIdleLogic : IReachLogic
{
    public event Action<Target> Reached;

    public IEnumerator ReachTarget()
    {
        yield return null;
    }

    public void SetTarget(Target target)
    {
        Reached?.Invoke(target);
    }
}
