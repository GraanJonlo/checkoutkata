using System.Collections.Generic;

namespace CheckoutKata
{
    internal class StubbedPriceLookup : ILookupPrices
    {
        private readonly IDictionary<string, int> _priceDetails;
        private readonly List<IKeepTotal> _listeners = new List<IKeepTotal>(); 

        public StubbedPriceLookup(IDictionary<string, int> priceDetails)
        {
            _priceDetails = priceDetails;
        }

        public void SkuScanned(string sku)
        {
            NotifyListeners(_priceDetails[sku]);
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
