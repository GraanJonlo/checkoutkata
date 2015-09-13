namespace CheckoutKata
{
    public interface IListenForSkus
    {
        void SkuScanned(string sku);
        void Register(IKeepTotal listener);
    }
}
