using System;

namespace CharacterTransactions
{
    public interface ITransaction
    {
        public event Action<CharacterState, Target> NewStatusAvailable;

        public void TryOn();

        public void Off();
    }
}
