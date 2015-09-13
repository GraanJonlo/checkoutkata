namespace CheckoutKata
{
    public interface IKeepTotal
    {
        void Credit(int amount);
        void Debit(int amount);
        void Register(IListenForTotals listener);
    }
}
