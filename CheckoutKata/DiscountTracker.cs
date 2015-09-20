using System.Collections.Generic;

namespace CheckoutKata
{
    internal class DiscountTracker : IListenForSkus
    {
        private readonly List<IKeepTotal> _listeners = new List<IKeepTotal>();
        private readonly string _sku;
        private readonly int _trigger;
        private readonly int _discount;

        private int _count;

        public DiscountTracker(string sku, int trigger, int discount)
        {
            _sku = sku;
            _trigger = trigger;
            _discount = discount;
            _count = 0;
        }

        public void SkuScanned(string sku)
        {
            if (sku.Equals(_sku))
            {
                _count++;
                if (_count == _trigger)
                {
                    NotifyListeners();
                }
            }
        }

        private void NotifyListeners()
        {
            foreach (IKeepTotal listener in _listeners)
            {
                listener.Debit(_discount);
            }
        }

        public void Register(IKeepTotal listener)
        {
            _listeners.Add(listener);
        }
    }
}
