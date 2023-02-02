using System;

public interface IPercBag
{
    public event Action<Perc> ShowedPerc;
    public event Action<Perc> RemovedPerc;

    public void AddPerc(Perc perc);
    public void RemovePerc(Perc perc);
}
