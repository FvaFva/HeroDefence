using System;

namespace CharacterTransactions
{
    public class TransactionReacherObserver : ITransaction
    {
        private CharacterState _targetState;
        private IReachLogic _reacher;

        public TransactionType Type { get; private set; }
        public event Action<CharacterState, Target> NewStatusAvailable;

        public TransactionReacherObserver(IReachLogic reacher, CharacterState targetState, TransactionType type)
        {
            _reacher = reacher;
            _targetState = targetState;
            Type = type;
        }

        public void TryOn()
        {
            if (_reacher != null)
                _reacher!.Reached += OnReachTarget;
        }

        public void Off()
        {
            _reacher!.Reached -= OnReachTarget;
        }

        private void OnReachTarget(Target target)
        {
            _reacher!.Reached -= OnReachTarget;
            NewStatusAvailable?.Invoke(_targetState, target);
        }
    }
}