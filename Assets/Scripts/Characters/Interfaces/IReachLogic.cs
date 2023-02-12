using System;
using System.Collections;

public interface IReachLogic
{
    public event Action Reached;
    public event Action Failed;

    public IEnumerator ReachTarget();

    public void SetTarget(Target target);
}
