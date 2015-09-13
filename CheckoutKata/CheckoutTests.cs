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
            ILookupPrices priceLookup = new StubbedPriceLookup(new Dictionary<string, int>(1) {{"testSku", 1234}});
            Checkout checkout = new CheckoutBuilder().With(priceLookup).Build();

            checkout.Scan("testSku");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(1234));
        }

        [Test]
        public void Scanning_two_item_gives_a_total_of_their_summed_prices()
        {
            ILookupPrices priceLookup =
                new StubbedPriceLookup(new Dictionary<string, int>(2) {{"ASku", 1234}, {"AnotherSku", 5678}});
            Checkout checkout = new CheckoutBuilder().With(priceLookup).Build();

            checkout.Scan("ASku");
            checkout.Scan("AnotherSku");

            var total = checkout.Total();
            Assert.That(total, Is.EqualTo(1234 + 5678));
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

        private readonly IKeepTotal _total = new InMemoryTotal();

        public CheckoutBuilder With(ILookupPrices priceLookup)
        {
            _priceLookup = priceLookup;
            return this;
        }

        public Checkout Build()
        {
            return new Checkout(_priceLookup, _total);
        }
    }

    public class Checkout : IListenForTotals
    {
        private readonly ILookupPrices _priceLookup;
        private readonly IKeepTotal _account;
        private int _total;

        public Checkout(ILookupPrices priceLookup, IKeepTotal account)
        {
            _priceLookup = priceLookup;
            _account = account;
            _account.Register(this);
        }

        public int Total()
        {
            return _total;
        }

        public void Scan(string sku)
        {
            _account.Credit(_priceLookup.PriceFor(sku));
        }

        public void NewTotal(int value)
        {
            _total = value;
        }
    }

    public interface IListenForTotals
    {
        void NewTotal(int value);
    }

    public class InMemoryTotal : IKeepTotal
    {
        private int _total;
        private readonly List<IListenForTotals> _listeners = new List<IListenForTotals>(); 

        public void Credit(int amount)
        {
            _total += amount;
            NotifyListeners();
        }

        private void NotifyListeners()
        {
            foreach (IListenForTotals listener in _listeners)
            {
                listener.NewTotal(_total);
            }
        }

        public void Register(IListenForTotals listener)
        {
            _listeners.Add(listener);
        }
    }

    public interface IKeepTotal
    {
        void Credit(int amount);
        void Register(IListenForTotals listener);
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
