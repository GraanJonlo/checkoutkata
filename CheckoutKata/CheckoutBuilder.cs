using System.Collections.Generic;

namespace CheckoutKata
{
    public class CheckoutBuilder
    {
        private readonly IList<IListenForSkus> _skuListeners = new List<IListenForSkus>();

        private readonly IKeepTotal _total = new InMemoryTotal();

        public CheckoutBuilder With(IListenForSkus listener)
        {
            _skuListeners.Add(listener);
            return this;
        }

        public Checkout Build()
        {
            return new Checkout(_skuListeners, _total);
        }
    }
}
