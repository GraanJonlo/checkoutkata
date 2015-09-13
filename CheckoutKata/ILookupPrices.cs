namespace CheckoutKata
{
    public interface ILookupPrices
    {
        void SkuScanned(string sku);
        void Register(IKeepTotal listener);
    }
}
