using System;
using System.Collections;

public interface ITargetDistanceObserver
{
    public event Action<IFightable> LostTarget;

    public event Action<IFightable> FoundTarget;

    public IEnumerator ObserveTarget();
}