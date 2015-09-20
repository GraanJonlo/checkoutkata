using System;
using System.Collections.Generic;

namespace CheckoutKata
{
    internal class DiscountTrackerHolder : IListenForSkus
    {
        private readonly List<DiscountTracker> _discountTrackers = new List<DiscountTracker>(2); 

        public DiscountTrackerHolder()
        {
            _discountTrackers.Add(new DiscountTracker("A", 3, 20));
            _discountTrackers.Add(new DiscountTracker("B", 2, 15));

        }

        public void SkuScanned(string sku)
        {
            foreach (DiscountTracker discountTracker in _discountTrackers)
            {
                discountTracker.SkuScanned(sku);
            }
        }

        public void Register(IKeepTotal listener)
        {
            foreach (DiscountTracker discountTracker in _discountTrackers)
            {
                discountTracker.Register(listener);
            }
        }
    }
}
