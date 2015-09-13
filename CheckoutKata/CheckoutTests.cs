using System.Collections.Generic;
using NUnit.Framework;

namespace CheckoutKata
{
    [TestFixture]
    public class CheckoutTests
    {
        [Test]
        public void Sanity_test()
        {
            Assert.That(true, Is.True);
        }

        [Test]
        public void Scanning_an_A_gives_a_total_of_50()
        {
            Checkout checkout =
                new CheckoutBuilder().WithPriceDetails(new Dictionary<string, int>(1) {{"A", 50}}).Build();

            checkout.Scan("A");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(50));
        }

        [Test]
        public void Scanning_a_B_gives_a_total_of_30()
        {
            Checkout checkout =
                new CheckoutBuilder().WithPriceDetails(new Dictionary<string, int>(1) { { "B", 30 } }).Build();

            checkout.Scan("B");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(30));
        }
    }

    public class CheckoutBuilder
    {
        private ILookupPrices _priceLookup = new InMemoryPriceLookup(new Dictionary<string, int>(4)
        {
            {"A", 50},
            {"B", 30},
            {"C", 20},
            {"D", 15}
        });

        public CheckoutBuilder WithPriceDetails(IDictionary<string, int> priceDetails)
        {
            _priceLookup = new InMemoryPriceLookup(priceDetails);
            return this;
        }

        public Checkout Build()
        {
            return new Checkout(_priceLookup);
        }
    }

    public class Checkout
    {
        private int _total;
        private readonly ILookupPrices _priceLookup;

        public Checkout(ILookupPrices priceLookup)
        {
            _priceLookup = priceLookup;
        }

        public int Total()
        {
            return _total;
        }

        public void Scan(string sku)
        {
            _total = _priceLookup.PriceFor(sku);
        }
    }

    public class InMemoryPriceLookup : ILookupPrices
    {
        private readonly IDictionary<string, int> _priceDetails;

        public InMemoryPriceLookup(IDictionary<string, int> priceDetails)
        {
            _priceDetails = priceDetails;
        }

        public int PriceFor(string sku)
        {
            return _priceDetails[sku];
        }
    }

    public interface ILookupPrices
    {
        int PriceFor(string sku);
    }
}
