using System;

public interface ICharacterComander
{
    public event Action<Target> ChoosedTarget;
}
