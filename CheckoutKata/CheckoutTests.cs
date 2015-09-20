﻿using System.Collections.Generic;
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
        public void Scanning_3_As_has_a_total_of_130()
        {
            Checkout checkout = new CheckoutBuilder().Build();

            checkout.Scan("A");
            checkout.Scan("A");
            checkout.Scan("A");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(130));
        }

        [Test]
        public void Scanning_2_Bs_has_a_total_of_45()
        {
            Checkout checkout = new CheckoutBuilder().Build();

            checkout.Scan("B");
            checkout.Scan("B");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(45));
        }
    }
}
