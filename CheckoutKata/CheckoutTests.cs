﻿using System;
using System.Collections.Generic;
using System.Linq;
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

    public class Checkout : IListenForTotals
    {
        private readonly IList<IListenForSkus> _skuListeners; 
        private int _total;

        public Checkout(IList<IListenForSkus> skuListeners, IKeepTotal account)
        {
            _skuListeners = skuListeners;

            foreach (IListenForSkus listener in skuListeners)
            {
                listener.Register(account);
            }

            account.Register(this);
        }

        public int Total()
        {
            return _total;
        }

        public void Scan(string sku)
        {
            foreach (IListenForSkus listener in _skuListeners)
            {
                listener.SkuScanned(sku);
            }
        }

        public void NewTotal(int value)
        {
            _total = value;
        }
    }

    internal class DiscountTracker : IListenForSkus
    {
        private readonly List<IKeepTotal> _listeners = new List<IKeepTotal>();
        private readonly Dictionary<string, int> _skuCount = new Dictionary<string, int>();
        private readonly Dictionary<string, Tuple<int,int>> _discountDetails = new Dictionary<string, Tuple<int, int>>(); 

        public DiscountTracker()
        {
            _discountDetails.Add("A", new Tuple<int, int>(3, 20));
            _discountDetails.Add("B", new Tuple<int, int>(2, 15));
        }

        public void SkuScanned(string sku)
        {
            if (_discountDetails.ContainsKey(sku))
            {
                if (!_skuCount.ContainsKey(sku))
                {
                    _skuCount.Add(sku, 0);
                }
                _skuCount[sku] = _skuCount[sku] + 1;

                if (_skuCount[sku] == _discountDetails[sku].Item1)
                {
                    NotifyListeners(_discountDetails[sku].Item2);
                }
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
}
