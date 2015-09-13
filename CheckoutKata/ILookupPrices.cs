namespace CheckoutKata
{
    public interface ILookupPrices
    {
        void PriceFor(string sku);
        void Register(IKeepTotal listener);
    }
}
