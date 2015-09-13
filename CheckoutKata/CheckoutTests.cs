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
            ILookupPrices priceLookup = new StubbedPriceLookup(new Dictionary<string, int>(1) {{"testSku", 1234}});
            Checkout checkout = new CheckoutBuilder().With(priceLookup).Build();

            checkout.Scan("testSku");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(1234));
        }
    }

    public class CheckoutBuilder
    {
        private ILookupPrices _priceLookup = new StubbedPriceLookup(new Dictionary<string, int>(4)
        {
            {"A", 50},
            {"B", 30},
            {"C", 20},
            {"D", 15}
        });

        public CheckoutBuilder With(ILookupPrices priceLookup)
        {
            _priceLookup = priceLookup;
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

    public class StubbedPriceLookup : ILookupPrices
    {
        private readonly IDictionary<string, int> _priceDetails;

        public StubbedPriceLookup(IDictionary<string, int> priceDetails)
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
