using System;

namespace CharacterTransactions
{
    public class TransactionChooserObserver : ITransaction
    {
        private CharacterState _targetState;
        private ITargetChooser _commander;
        private IFightable _current;
        private TransactionRule _rule;
        private bool _staticCommander;
        private bool _isObserving;

        public TransactionChooserObserver(TransactionRule rule, IFightable current, CharacterState targetState, TransactionType type, ITargetChooser commander = null)
        {
            _rule = rule;
            _current = current;
            _targetState = targetState;
            _staticCommander = commander != null;
            _commander = commander;
            Type = type;
        }

        public event Action<CharacterState, Target> NewStatusAvailable;

        public TransactionType Type { get; private set; }

        public void SetCommander(ITargetChooser commander)
        {
            if (_staticCommander == false)
            {
                if (_isObserving)
                {
                    ResubscribeCommander(commander);
                }
                else
                {
                    _commander = commander;
                }
            }
        }

        public void TryOn()
        {
            if (_commander != null)
                _commander.ChoseTarget += OnChooseNewTarget;

            _isObserving = true;
        }

        public void Off()
        {
            if (_commander != null)
                _commander.ChoseTarget -= OnChooseNewTarget;

            _isObserving = false;
        }

        private void OnChooseNewTarget(Target target)
        {
            if (target.IsIFightableMatches(_current) == false && _rule.CheckSuitableTarget(target, _current))
            {
                _commander!.ChoseTarget -= OnChooseNewTarget;
                NewStatusAvailable?.Invoke(_targetState, target);
            }
        }

        private void ResubscribeCommander(ITargetChooser commander)
        {
            if (_commander != null)
                _commander.ChoseTarget -= OnChooseNewTarget;

            _commander = commander;

            if (_commander != null)
                _commander.ChoseTarget += OnChooseNewTarget;
        }
    }
}