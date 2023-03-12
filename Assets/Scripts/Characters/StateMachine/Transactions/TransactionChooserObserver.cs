using System;

namespace CharacterTransactions
{
    public class TransactionChooserObserver : ITransaction
    {
        private CharacterState _targetState;
        private ITargetChooser _comander;
        private IFightable _current;
        private TransactionRule _rule;
        private bool _staticComander;
        private bool _isObserving;

        public event Action<CharacterState, Target> NewStatusAvailable;
        public TransactionType Type { get; private set; }

        public TransactionChooserObserver(TransactionRule rule, IFightable current, CharacterState targetState, TransactionType type, ITargetChooser comander = null)
        {
            _rule = rule;
            _current = current;
            _targetState = targetState;
            _staticComander = comander != null;
            _comander = comander;
            Type = type;
        }

        public void SetComander(ITargetChooser comander)
        {
            if (_staticComander == false)
            {
                if(_isObserving)
                {
                    ResubscribeComander(comander);
                }
                else
                {
                    _comander = comander;
                }
            }
        }

        public void TryOn()
        {
            if (_comander != null)            
                _comander.ChoosedTarget += OnChooseNewTarget;                    

            _isObserving = true;
        }

        public void Off()
        {
            if (_comander != null)            
                _comander.ChoosedTarget -= OnChooseNewTarget;            

            _isObserving = false;
        }

        private void OnChooseNewTarget(Target target)
        {
            if (target.IsIFightebelMatches(_current) == false && _rule.CheckSuitableTarget(target, _current))
            {
                _comander!.ChoosedTarget -= OnChooseNewTarget;
                NewStatusAvailable?.Invoke(_targetState, target);
            }
        }

        private void ResubscribeComander(ITargetChooser comander)
        {
            if (_comander != null)
                _comander.ChoosedTarget -= OnChooseNewTarget;

            _comander = comander;

            if (_comander != null)
                _comander.ChoosedTarget += OnChooseNewTarget;
        }
    }
}