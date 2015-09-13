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
            Checkout checkout = new CheckoutBuilder().Build();

            checkout.Scan("A");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(50));
        }

        [Test]
        public void Scanning_a_B_gives_a_total_of_30()
        {
            Checkout checkout = new CheckoutBuilder().Build();

            checkout.Scan("B");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(30));
        }
    }

    public class CheckoutBuilder
    {
        private IDictionary<string, int> _priceDetails = new Dictionary<string, int>(2) {{"A", 50}, {"B", 30}};

        public CheckoutBuilder WithPriceDetails(IDictionary<string, int> priceDetails)
        {
            _priceDetails = priceDetails;
            return this;
        }

        public Checkout Build()
        {
            return new Checkout(_priceDetails);
        }
    }

    public class Checkout
    {
        private int _total;
        private readonly IDictionary<string, int> _priceDetails;

        public Checkout(IDictionary<string, int> priceDetails)
        {
            _priceDetails = priceDetails;
        }

        public int Total()
        {
            return _total;
        }

        public void Scan(string sku)
        {
            _total = _priceDetails[sku];
        }
    }
}
