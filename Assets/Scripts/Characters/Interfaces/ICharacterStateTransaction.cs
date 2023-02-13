using System;

public interface ICharacterStateTransaction
{
    public event Action<CharacterState, Target> Activited;
    public void Activate(ICharacterComander comander);
    public void Off();
}
