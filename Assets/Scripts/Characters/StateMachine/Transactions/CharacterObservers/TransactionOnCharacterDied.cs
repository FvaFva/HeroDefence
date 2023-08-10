namespace CharacterTransactions
{
    public class TransactionOnCharacterDied : BaseTransactionCharacterObserver
    {
        public TransactionOnCharacterDied(CharacterState targetState, IFightable character, TransactionType type)
            : base(targetState, character, type)
        {
        }

        public override void Off()
        {
            if (Character != null)
                Character.Died -= OnTriggering;
        }

        public override void TryOn()
        {
            if (Character != null)
                Character.Died += OnTriggering;
        }
    }
}
