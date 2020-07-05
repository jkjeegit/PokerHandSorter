using PokerHandSorter.Compapers;
using PokerHandSorter.Models;
using System.Collections.Generic;
using System.Linq;

namespace PokerHandSorter.Utils
{
    public class RulesEngine
    {
        private IEnumerable<Card> _playerCards;

        public RulesEngine(IEnumerable<Card> playerCards)
        {
            _playerCards = playerCards;
        }

        public IEnumerable<CardCountByValue> GenerateCardTallyByValue()
        {
            var result = new Dictionary<int, int>();

            return _playerCards.Select(playerCard =>
            {
                return new CardCountByValue()
                {
                    Value = playerCard.Value,
                    Count = _playerCards.Count(card => card.Value == playerCard.Value)
                };
            }).Distinct(new CardCountByValueCompaper())
            .OrderByDescending(order => order.Count);
        }

        public IEnumerable<int> GetCardValues()
        {
            return _playerCards.Select(card => card.Value).OrderBy(c => c).ToList();
        }

        public IEnumerable<string> GetCardSuits()
        {
            return _playerCards.Select(card => card.Suit).ToList();
        }

        public bool IsRoyalFlush()
        {
            if (!IsFlush())
                return false;

            var values = GetCardValues().ToList();

            if (values[0] == 10 && values[1] == 11 && values[2] == 12 && values[3] == 13 && values[4] == 14)
                return true;

            return false;
        }

        public bool IsStraightFlush()
        {
            return IsFlush() && IsStraight();
        }

        public bool IsFourOfAKind()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.First().Count == 4)
                return true;

            return false;
        }

        public int FourOfAKindOtherValue()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.First().Count == 4)
                return tally.Last().Value;

            return 0;
        }

        public bool IsFullHouse()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.Count() == 2 && tally.First().Count == 3 && tally.Last().Count == 2)
                return true;

            return false;
        }

        public int FullHouseHighValue()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.Count() == 2 && tally.First().Count == 3 && tally.Last().Count == 2)
                return tally.First().Value;

            return 0;
        }

        public bool IsFlush()
        {
            var suits = GetCardSuits();

            if (suits.Distinct().Count() == 1)
                return true;

            return false;
        }

        public bool IsStraight()
        {
            var values = GetCardValues().ToList();

            if (values[1] == values[0] + 1
                    && values[2] == values[0] + 2
                    && values[3] == values[0] + 3
                    && values[4] == values[0] + 4)
                return true;

            return false;
        }

        public bool IsThreeOfAKind()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.First().Count == 3)
                return true;

            return false;
        }

        public int ThreeOfAKindNextHighValue()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.First().Count == 3)
                return tally.Skip(1).First().Value;

            return 0;
        }

        public bool IsTwoPairs()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.Count() == 3 && tally.First().Count == 2 && tally.Skip(1).First().Count == 2)
                return true;

            return false;
        }

        public int TwoPairsOtherValue()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.Count() == 3 && tally.First().Count == 2 && tally.Skip(1).First().Count == 2)
                return tally.Last().Value;

            return 0;
        }

        public bool IsPair()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.Count() == 4 && tally.First().Count == 2)
                return true;

            return false;
        }

        public int PairHighValue()
        {
            var tally = GenerateCardTallyByValue();

            if (tally.Count() == 4 && tally.First().Count == 2)
                return tally.First().Value;

            return 0;
        }

        public int GetHighCardValue(int otherThan1 = 0, int otherThan2 = 0, int otherThan3 = 0, int otherThan4 = 0)
        {
            if (otherThan1 != 0 && otherThan2 != 0 && otherThan3 != 0 && otherThan4 != 0)
            {
                return GetCardValues().Where(v => v != otherThan1 && v != otherThan2 && v != otherThan3 && v!= otherThan4).Last();
            }

            if (otherThan1 != 0 && otherThan2 != 0 && otherThan3 != 0)
            {
                return GetCardValues().Where(v => v != otherThan1 && v != otherThan2 && v != otherThan3).Last();
            }

            if (otherThan1 != 0 && otherThan2 != 0)
            {
                return GetCardValues().Where(v => v != otherThan1 && v != otherThan2).Last();
            }

            if(otherThan1 != 0)
            {
                return GetCardValues().Where(v => v != otherThan1).Last();
            }

            return GetCardValues().Last();
        }
    }
}
