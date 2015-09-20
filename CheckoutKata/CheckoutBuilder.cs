using System.Collections.Generic;

namespace CheckoutKata
{
    public class CheckoutBuilder
    {
        private IListenForSkus _priceLookup = new PriceLookup(new Dictionary<string, int>(4)
        {
            {"A", 50},
            {"B", 30},
            {"C", 20},
            {"D", 15}
        });

        private readonly IListenForSkus _discountTracker = new DiscountTrackerHolder();

        private readonly IKeepTotal _total = new InMemoryTotal();

        public CheckoutBuilder WithPriceLookup(IListenForSkus priceLookup)
        {
            _priceLookup = priceLookup;
            return this;
        }

        public Checkout Build()
        {
            return new Checkout(new List<IListenForSkus>(2) {_priceLookup, _discountTracker}, _total);
        }
    }
}
