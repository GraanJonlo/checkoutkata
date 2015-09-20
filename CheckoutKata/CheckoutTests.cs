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
            Checkout checkout = new CheckoutBuilder().WithPriceLookup(priceLookup).Build();

            checkout.Scan("testSku");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(1234));
        }

        [Test]
        public void Scanning_two_item_gives_a_total_of_their_summed_prices()
        {
            IListenForSkus priceLookup =
                new PriceLookup(new Dictionary<string, int>(2) {{"ASku", 1234}, {"AnotherSku", 5678}});
            Checkout checkout = new CheckoutBuilder().WithPriceLookup(priceLookup).Build();

            checkout.Scan("ASku");
            checkout.Scan("AnotherSku");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(1234 + 5678));
        }

        [Test]
        public void Applies_applicable_discount()
        {
            IListenForSkus priceLookup = new PriceLookup(new Dictionary<string, int>(1) { { "Foo", 0 } });
            Checkout checkout =
                new CheckoutBuilder().WithPriceLookup(priceLookup)
                    .WithDiscountTracker(new DiscountTracker("Foo", 2, 200))
                    .Build();

            checkout.Scan("Foo");
            checkout.Scan("Foo");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(-200));
        }

        [Test]
        public void Applies_applicable_discount_multiple_times()
        {
            IListenForSkus priceLookup = new PriceLookup(new Dictionary<string, int>(1) { { "Foo", 0 } });
            Checkout checkout =
                new CheckoutBuilder().WithPriceLookup(priceLookup)
                    .WithDiscountTracker(new DiscountTracker("Foo", 2, 200))
                    .Build();

            checkout.Scan("Foo");
            checkout.Scan("Foo");
            checkout.Scan("Foo");
            checkout.Scan("Foo");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(-400));
        }
    }
}
