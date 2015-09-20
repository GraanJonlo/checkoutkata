using System.Collections.Generic;

namespace CheckoutKata
{
    internal class PriceLookup : IListenForSkus
    {
        private readonly string _sku;
        private readonly int _price;
        private readonly List<IKeepTotal> _listeners = new List<IKeepTotal>();

        public PriceLookup(string sku, int price)
        {
            _sku = sku;
            _price = price;
        }

        public void SkuScanned(string sku)
        {
            if (_sku.Equals(sku))
            {
                NotifyListeners(_price);
            }
        }

        private void NotifyListeners(int price)
        {
            foreach (IKeepTotal listener in _listeners)
            {
                listener.Credit(price);
            }
        }

        public void Register(IKeepTotal listener)
        {
            _listeners.Add(listener);
        }
    }
}
