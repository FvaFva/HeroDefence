using System;

public interface ITargetChooser
{
    public event Action<Target> ChoseTarget;
}
