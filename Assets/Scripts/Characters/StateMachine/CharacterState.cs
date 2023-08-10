using System;
using System.Collections;
using System.Collections.Generic;

namespace CharacterTransactions
{
    public class CharacterState
    {
        private List<ITransaction> _transactions = new List<ITransaction>();
        private IReachLogic _achiever;

        public CharacterState(IReachLogic achiever, string name)
        {
            _achiever = achiever;
            Name = name;
        }

        public event Action<CharacterState, Target> OnFindNextState;

        public string Name { get; private set; }

        public IEnumerator ReachTarget => _achiever.ReachTarget();

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
            _achiever.SetTarget(target);

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