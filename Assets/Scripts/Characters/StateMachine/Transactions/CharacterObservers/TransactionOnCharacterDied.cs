namespace CharacterTransactions
{
    public class TransactionOnCharacterDied : BaseTransactionCharacterObserver
    {
        public TransactionOnCharacterDied(CharacterState targetState, IFightable character, TransactionType type) : base(targetState, character, type){}

        public override void Off()
        {
            if (_character != null)
                _character.Died -= OnTriggering;
        }

        public override void TryOn()
        {
            if (_character != null)
                _character.Died += OnTriggering;
        }
    }
}
