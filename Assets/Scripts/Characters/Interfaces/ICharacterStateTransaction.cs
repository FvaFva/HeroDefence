using System;

public interface ICharacterStateTransaction
{
    public event Action<CharacterState, Target> Activited;
    public void TryOn(ICharacterComander comander);
    public void Off();
}
