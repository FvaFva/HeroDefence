using System;

namespace CharacterTransactions
{
    public abstract class BaseTransactionCharacterObserver : ITransaction
    {
        private IFightable _character;
        private CharacterState _targetState;

        public BaseTransactionCharacterObserver(CharacterState targetState, IFightable character, TransactionType type)
        {
            _targetState = targetState;
            _character = character;
            Type = type;
        }

        public event Action<CharacterState, Target> NewStatusAvailable;

        public TransactionType Type { get; private set; }

        protected IFightable Character => _character;

        protected CharacterState TargetState => _targetState;

        public abstract void Off();

        public abstract void TryOn();

        protected virtual void OnTriggering()
        {
            NewStatusAvailable?.Invoke(TargetState, default(Target));
        }
    }
}
