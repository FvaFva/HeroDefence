using System;
using System.Collections;

public interface IReachLogic
{
    public event Action<Target> Reached;

    public IEnumerator ReachTarget();

    public void SetTarget(Target target);
}
