using System.Collections.Generic;
using NUnit.Framework;

namespace CheckoutKata
{
    [TestFixture]
    public class CheckoutTests
    {
        [Test]
        public void Scanning_an_item_gives_a_total_of_that_items_price()
        {
            IListenForSkus priceLookup = new PriceLookup(new Dictionary<string, int>(1) {{"testSku", 1234}});
            Checkout checkout = new CheckoutBuilder().With(priceLookup).Build();

            checkout.Scan("testSku");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(1234));
        }

        [Test]
        public void Scanning_two_item_gives_a_total_of_their_summed_prices()
        {
            IListenForSkus priceLookup =
                new PriceLookup(new Dictionary<string, int>(2) {{"ASku", 1234}, {"AnotherSku", 5678}});
            Checkout checkout = new CheckoutBuilder().With(priceLookup).Build();

            checkout.Scan("ASku");
            checkout.Scan("AnotherSku");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(1234 + 5678));
        }

        [Test]
        public void Scanning_3_As_has_a_total_of_130()
        {
            Checkout checkout = new CheckoutBuilder().Build();

            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(130));
        }
    }

    public class CheckoutBuilder
    {
        private IListenForSkus _priceLookup = new PriceLookup(new Dictionary<string, int>(4)
        {
            {"A", 50},
            {"B", 30},
            {"C", 20},
            {"D", 15}
        });

        private readonly IListenForSkus _discountTracker = new DiscountTracker();

        private readonly IKeepTotal _total = new InMemoryTotal();

        public CheckoutBuilder With(IListenForSkus priceLookup)
        {
            _priceLookup = priceLookup;
            return this;
        }

        public Checkout Build()
        {
            return new Checkout(_priceLookup, _discountTracker, _total);
        }
    }

    public class Checkout : IListenForTotals
    {
        private readonly List<IListenForSkus> _skuListener = new List<IListenForSkus>(2); 
        private int _total;

        public Checkout(IListenForSkus priceLookup, IListenForSkus discountTracker, IKeepTotal account)
        {
            priceLookup.Register(account);
            _skuListener.Add(priceLookup);

            discountTracker.Register(account);
            _skuListener.Add(discountTracker);

            account.Register(this);
        }

        public int Total()
        {
            return _total;
        }

        public void Scan(string sku)
        {
            foreach (IListenForSkus listener in _skuListener)
            {
                listener.SkuScanned(sku);
            }
        }

        public void NewTotal(int value)
        {
            _total = value;
        }
    }

    public class DiscountTracker : IListenForSkus
    {
        private int _countA;
        private readonly List<IKeepTotal> _listeners = new List<IKeepTotal>(); 

        public void SkuScanned(string sku)
        {
            if (sku.Equals("A"))
            {
                _countA++;
            }

            if (_countA == 3)
            {
                NotifyListeners(20);
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

    public interface IListenForSkus
    {
        void SkuScanned(string sku);
        void Register(IKeepTotal listener);
    }
}
