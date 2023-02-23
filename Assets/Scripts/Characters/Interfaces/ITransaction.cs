using System;

public interface ITransaction
{
    public event Action<CharacterState, Target> Activited;
    public void TryOn(ICharacterComander comander);
    public void Off();
}
