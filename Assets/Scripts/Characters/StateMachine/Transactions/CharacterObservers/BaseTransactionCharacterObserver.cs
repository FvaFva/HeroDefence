using System;

namespace CharacterTransactions
{
    public abstract class BaseTransactionCharacterObserver : ITransaction
    {
        protected IFightable _character;
        protected CharacterState _targetState;

        public event Action<CharacterState, Target> NewStatusAvailable;
        public TypesOfTransaction Type { get; private set; }

        public BaseTransactionCharacterObserver(CharacterState targetState, IFightable character, TypesOfTransaction type)
        {
            _targetState = targetState;
            _character = character;
            Type = type;
        }

        public abstract void Off();

        public abstract void TryOn();

        protected virtual void OnTriggering()
        {
            NewStatusAvailable?.Invoke(_targetState, new Target());
        }
    }
}
