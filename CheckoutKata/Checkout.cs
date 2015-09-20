using System.Collections.Generic;

namespace CheckoutKata
{
    public class Checkout : IListenForTotals
    {
        private readonly IList<IListenForSkus> _skuListeners; 
        private int _total;

        public Checkout(IList<IListenForSkus> skuListeners, IKeepTotal account)
        {
            _skuListeners = skuListeners;

            foreach (IListenForSkus listener in skuListeners)
            {
                listener.Register(account);
            }

            account.Register(this);
        }

        public int Total()
        {
            return _total;
        }

        public void Scan(string sku)
        {
            foreach (IListenForSkus listener in _skuListeners)
            {
                listener.SkuScanned(sku);
            }
        }

        public void NewTotal(int value)
        {
            _total = value;
        }
    }
}
