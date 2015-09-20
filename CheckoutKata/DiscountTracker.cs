using System;
using System.Collections.Generic;

namespace CheckoutKata
{
    internal class DiscountTracker : IListenForSkus
    {
        private readonly List<IKeepTotal> _listeners = new List<IKeepTotal>();
        private readonly Dictionary<string, int> _skuCount = new Dictionary<string, int>();
        private readonly Dictionary<string, Tuple<int, int>> _discountDetails = new Dictionary<string, Tuple<int, int>>(); 

        public DiscountTracker()
        {
            _discountDetails.Add("A", new Tuple<int, int>(3, 20));
            _discountDetails.Add("B", new Tuple<int, int>(2, 15));
        }

        public void SkuScanned(string sku)
        {
            if (_discountDetails.ContainsKey(sku))
            {
                if (!_skuCount.ContainsKey(sku))
                {
                    _skuCount.Add(sku, 0);
                }
                _skuCount[sku] = _skuCount[sku] + 1;

                if (_skuCount[sku] == _discountDetails[sku].Item1)
                {
                    NotifyListeners(_discountDetails[sku].Item2);
                }
            }
        }

        private void NotifyListeners(int discount)
        {
            foreach (IKeepTotal listener in _listeners)
            {
                listener.Debit(discount);
            }
        }

        public void Register(IKeepTotal listener)
        {
            _listeners.Add(listener);
        }
    }
}
