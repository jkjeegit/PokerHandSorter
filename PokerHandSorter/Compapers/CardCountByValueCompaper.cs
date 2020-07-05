using PokerHandSorter.Models;
using System.Collections.Generic;

namespace PokerHandSorter.Compapers
{
    public class CardCountByValueCompaper : IEqualityComparer<CardCountByValue>
    {
        public bool Equals(CardCountByValue x, CardCountByValue y)
        {
            return x.Value == y.Value;
        }

        public int GetHashCode(CardCountByValue obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}
