using System.Collections.Generic;
using System.Linq;

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

        private readonly List<IListenForSkus> _discountTrackers = new List<IListenForSkus>(2)
        {
            new DiscountTracker("A", 3, 20),
            new DiscountTracker("B", 2, 15)
        }; 

        private readonly IKeepTotal _total = new InMemoryTotal();

        public CheckoutBuilder WithPriceLookup(IListenForSkus priceLookup)
        {
            _priceLookup = priceLookup;
            return this;
        }

        public Checkout Build()
        {
            var skuListeners = new List<IListenForSkus> {_priceLookup}.Concat(_discountTrackers).ToList();
            return new Checkout(skuListeners, _total);
        }
    }
}
