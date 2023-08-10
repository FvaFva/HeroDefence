using System;

namespace CharacterTransactions
{
    public class TransactionReacherObserver : ITransaction
    {
        private CharacterState _targetState;
        private IReachLogic _achiever;

        public TransactionReacherObserver(IReachLogic achiever, CharacterState targetState, TransactionType type)
        {
            _achiever = achiever;
            _targetState = targetState;
            Type = type;
        }

        public event Action<CharacterState, Target> NewStatusAvailable;

        public TransactionType Type { get; private set; }

        public void TryOn()
        {
            if (_achiever != null)
                _achiever!.Reached += OnReachTarget;
        }

        public void Off()
        {
            _achiever!.Reached -= OnReachTarget;
        }

        private void OnReachTarget(Target target)
        {
            _achiever!.Reached -= OnReachTarget;
            NewStatusAvailable?.Invoke(_targetState, target);
        }
    }
}