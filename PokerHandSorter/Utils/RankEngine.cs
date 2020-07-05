using PokerHandSorter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerHandSorter.Utils
{
    public class RankEngine
    {
        private const int ROYAL_FLUSH_RANK = 10;
        private const int STRAIGHT_FLUSH_RANK = 9;
        private const int FOUR_OF_A_KIND_RANK = 8;
        private const int FULL_HOUSE_RANK = 7;
        private const int FLUSH_RANK = 6;
        private const int STRAIGHT_RANK = 5;
        private const int THREE_OF_A_KIND_RANK = 4;
        private const int TWO_PAIRS_RANK = 3;
        private const int PAIR_RANK = 2;
        private const int HIGH_CARD_RANK = 1;

        public int CalculateRank(IEnumerable<Card> playerCards)
        {
            var rulesEngine = new RulesEngine(playerCards);

            if (rulesEngine.IsRoyalFlush())
                return ROYAL_FLUSH_RANK;

            if (rulesEngine.IsStraightFlush())
                return STRAIGHT_FLUSH_RANK;

            if (rulesEngine.IsFourOfAKind())
                return FOUR_OF_A_KIND_RANK;

            if (rulesEngine.IsFullHouse())
                return FULL_HOUSE_RANK;

            if (rulesEngine.IsFlush())
                return FLUSH_RANK;

            if (rulesEngine.IsStraight())
                return STRAIGHT_RANK;

            if (rulesEngine.IsThreeOfAKind())
                return THREE_OF_A_KIND_RANK;

            if (rulesEngine.IsTwoPairs())
                return TWO_PAIRS_RANK;

            if (rulesEngine.IsPair())
                return PAIR_RANK;

            return 0;
        }

        public int CalculateRankOnTie(IEnumerable<Card> playerOneCards, IEnumerable<Card> playerTwoCards, int rank)
        {
            var rulesEngineOne = new RulesEngine(playerOneCards);
            var rulesEngineTwo = new RulesEngine(playerTwoCards);
            switch (rank)
            {
                case 2:
                    return CalculatePairTie(rulesEngineOne, rulesEngineTwo);

                case 3:
                    return CalculateTwoPairsTie(rulesEngineOne, rulesEngineTwo);

                case 4:
                    return CalculateThreeOfAKindTie(rulesEngineOne, rulesEngineTwo);

                case 7:
                    return CalculateFullHouseTie(rulesEngineOne, rulesEngineTwo);

                case 8:
                    return CalculateFourOfAKindTie(rulesEngineOne, rulesEngineTwo);

                default:
                    return CalculateHighCardTie(rulesEngineOne, rulesEngineTwo);
            }
        }

        private int CalculateHighCardTie(RulesEngine rulesEngineOne, RulesEngine rulesEngineTwo)
        {
            var playerOneHighCardValue = rulesEngineOne.GetHighCardValue();
            var playerTwoHighCardValue = rulesEngineTwo.GetHighCardValue();

            if (playerOneHighCardValue > playerTwoHighCardValue)
            {
                return 1;
            }
            else if (playerOneHighCardValue == playerTwoHighCardValue)
            {
                var playerOneNextHigh1 = rulesEngineOne.GetHighCardValue(playerOneHighCardValue);
                var playerTwoNextHigh1 = rulesEngineTwo.GetHighCardValue(playerTwoHighCardValue);

                if (playerOneNextHigh1 > playerTwoNextHigh1)
                    return HIGH_CARD_RANK;

                else if (playerOneNextHigh1 == playerTwoNextHigh1)
                {
                    var playerOneNextHigh2 = rulesEngineOne.GetHighCardValue(playerOneHighCardValue, playerOneNextHigh1);
                    var playerTwoNextHigh2 = rulesEngineTwo.GetHighCardValue(playerTwoHighCardValue, playerOneNextHigh1);

                    if (playerOneNextHigh2 > playerTwoNextHigh2)
                        return HIGH_CARD_RANK;

                    else if (playerOneNextHigh2 == playerTwoNextHigh2)
                    {
                        var playerOneNextHigh3 = rulesEngineOne.GetHighCardValue(playerOneHighCardValue, playerOneNextHigh1, playerOneNextHigh2);
                        var playerTwoNextHigh3 = rulesEngineTwo.GetHighCardValue(playerTwoHighCardValue, playerOneNextHigh1, playerOneNextHigh2);

                        if (playerOneNextHigh3 > playerTwoNextHigh3)
                            return HIGH_CARD_RANK;

                        else if (playerOneNextHigh3 == playerTwoNextHigh3)
                        {
                            if (rulesEngineOne.GetHighCardValue(playerOneHighCardValue, playerOneNextHigh1, playerOneNextHigh2, playerOneNextHigh3)
                                > rulesEngineTwo.GetHighCardValue(playerTwoHighCardValue, playerOneNextHigh1, playerOneNextHigh2, playerOneNextHigh3))
                                return HIGH_CARD_RANK;
                            else
                                return 0;
                        }
                        else
                            return 0;
                    }
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        private int CalculateFourOfAKindTie(RulesEngine rulesEngineOne, RulesEngine rulesEngineTwo)
        {
            var playerOneFourOfAKindOtherValue = rulesEngineOne.FourOfAKindOtherValue();
            var playerTwoFourOfAKindOtherValue = rulesEngineTwo.FourOfAKindOtherValue();

            if (playerOneFourOfAKindOtherValue > playerTwoFourOfAKindOtherValue)
            {
                return HIGH_CARD_RANK;
            }
            else
            {
                return 0;
            }
        }

        private int CalculateFullHouseTie(RulesEngine rulesEngineOne, RulesEngine rulesEngineTwo)
        {
            var playerOneFullHouseHighValue = rulesEngineOne.FullHouseHighValue();
            var playerTwoFullHouseHighValue = rulesEngineTwo.FullHouseHighValue();

            if (playerOneFullHouseHighValue > playerTwoFullHouseHighValue)
            {
                return HIGH_CARD_RANK;
            }
            else if (playerOneFullHouseHighValue == playerTwoFullHouseHighValue)
            {
                if (rulesEngineOne.GetHighCardValue(playerOneFullHouseHighValue)
                        > rulesEngineTwo.GetHighCardValue(playerTwoFullHouseHighValue))
                    return HIGH_CARD_RANK;
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        private int CalculateThreeOfAKindTie(RulesEngine rulesEngineOne, RulesEngine rulesEngineTwo)
        {
            var playerOneThreeOfAKindNextHighValue = rulesEngineOne.ThreeOfAKindNextHighValue();
            var playerTwoThreeOfAKindNextHighValue = rulesEngineTwo.ThreeOfAKindNextHighValue();

            if (playerOneThreeOfAKindNextHighValue > playerTwoThreeOfAKindNextHighValue)
            {
                return HIGH_CARD_RANK;
            }
            else if (playerOneThreeOfAKindNextHighValue == playerTwoThreeOfAKindNextHighValue)
            {
                if (rulesEngineOne.GetHighCardValue(playerOneThreeOfAKindNextHighValue)
                        > rulesEngineTwo.GetHighCardValue(playerTwoThreeOfAKindNextHighValue))
                    return HIGH_CARD_RANK;
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        private int CalculateTwoPairsTie(RulesEngine rulesEngineOne, RulesEngine rulesEngineTwo)
        {
            var playerOneTwoPairsOtherValue = rulesEngineOne.TwoPairsOtherValue();
            var playerTwoTwoPairsOtherValue = rulesEngineTwo.TwoPairsOtherValue();

            if (playerOneTwoPairsOtherValue > playerTwoTwoPairsOtherValue)
            {
                return HIGH_CARD_RANK;
            }
            else
            {
                return 0;
            }
        }

        private int CalculatePairTie(RulesEngine rulesEngineOne, RulesEngine rulesEngineTwo)
        {
            var playerOnePairValue = rulesEngineOne.PairHighValue();
            var playerTwoPairValue = rulesEngineTwo.PairHighValue();

            if (playerOnePairValue > playerTwoPairValue)
            {
                return HIGH_CARD_RANK;
            }
            else if (playerOnePairValue == playerTwoPairValue)
            {
                var playerOneNextHigh1 = rulesEngineOne.GetHighCardValue(playerOnePairValue);
                var playerTwoNextHigh1 = rulesEngineTwo.GetHighCardValue(playerTwoPairValue);

                if (playerOneNextHigh1 > playerTwoNextHigh1)
                    return HIGH_CARD_RANK;

                else if (playerOneNextHigh1 == playerTwoNextHigh1)
                {
                    var playerOneNextHigh2 = rulesEngineOne.GetHighCardValue(playerOnePairValue, playerOneNextHigh1);
                    var playerTwoNextHigh2 = rulesEngineTwo.GetHighCardValue(playerOnePairValue, playerOneNextHigh1);

                    if (playerOneNextHigh2 > playerTwoNextHigh2)
                        return HIGH_CARD_RANK;

                    else if (playerOneNextHigh2 == playerTwoNextHigh2)
                    {
                        var playerOneNextHigh3 = rulesEngineOne.GetHighCardValue(playerOnePairValue, playerOneNextHigh1, playerOneNextHigh2);
                        var playerTwoNextHigh3 = rulesEngineTwo.GetHighCardValue(playerOnePairValue, playerOneNextHigh1, playerOneNextHigh2);

                        if (playerOneNextHigh3 > playerTwoNextHigh3)
                            return HIGH_CARD_RANK;
                        else
                            return 0;
                    }
                    else
                        return 0;
                }

                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }
    }
}
