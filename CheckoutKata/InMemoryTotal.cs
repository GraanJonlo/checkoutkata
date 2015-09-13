using System.Collections.Generic;

namespace CheckoutKata
{
    public class InMemoryTotal : IKeepTotal
    {
        private int _total;
        private readonly List<IListenForTotals> _listeners = new List<IListenForTotals>(); 

        public void Credit(int amount)
        {
            AdjustTotal(amount);
        }

        public void Debit(int amount)
        {
            AdjustTotal(-amount);
        }

        private void AdjustTotal(int amount)
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
}
