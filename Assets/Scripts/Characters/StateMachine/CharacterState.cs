using System;
using System.Collections;
using System.Collections.Generic;

namespace CharacterTransactions
{
    public class CharacterState
    {
        private List<ITransaction> _transactions = new List<ITransaction>();
        private IReachLogic _reacher;

        public string Name { get; private set; }
        public event Action<CharacterState, Target> OnFindNextState;
        public IEnumerator ReachTarget => _reacher.ReachTarget();

        public CharacterState(IReachLogic reacher, string name)
        {
            _reacher = reacher;
            Name = name;
        }

        public void AddTransaction(ITransaction transaction)
        {
            if (transaction != null && _transactions.Contains(transaction) == false)
                _transactions.Add(transaction);
        }

        public void AddTransaction(List<ITransaction> transactions)
        {
            foreach (ITransaction transaction in transactions)
                AddTransaction(transaction);
        }

        public void Enter(Target target)
        {
            _reacher.SetTarget(target);

            foreach (ITransaction transaction in _transactions)
            {
                transaction.NewStatusAvailable += OnTransactionActivated;
                transaction.TryOn();
            }
        }

        public void Exit()
        {
            foreach (ITransaction transaction in _transactions)
            {
                transaction.Off();
                transaction.NewStatusAvailable -= OnTransactionActivated;
            }
        }

        private void OnTransactionActivated(CharacterState nextState, Target target)
        {
            OnFindNextState?.Invoke(nextState, target);
        }
    }
}