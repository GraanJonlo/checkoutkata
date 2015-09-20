using System.Collections.Generic;

namespace CheckoutKata
{
    public class CheckoutBuilder
    {
        private readonly IList<IListenForSkus> _skuListeners = new List<IListenForSkus>();

        private readonly IKeepTotal _total = new InMemoryTotal();

        public CheckoutBuilder WithPriceLookup(IListenForSkus priceLookup)
        {
            _skuListeners.Add(priceLookup);
            return this;
        }

        public CheckoutBuilder WithDiscountTracker(IListenForSkus discountTracker)
        {
            _skuListeners.Add(discountTracker);
            return this;
        }

        public Checkout Build()
        {
            return new Checkout(_skuListeners, _total);
        }
    }
}
