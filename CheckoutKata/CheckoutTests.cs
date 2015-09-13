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
    }

    public class CheckoutBuilder
    {
        public Checkout Build()
        {
            return new Checkout();
        }
    }

    public class Checkout
    {
        public int Total()
        {
            return 0;
        }

        public void Scan(string sku)
        {
            // no op
        }
    }
}
