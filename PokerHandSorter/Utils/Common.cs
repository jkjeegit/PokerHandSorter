using PokerHandSorter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PokerHandSorter.Utils
{
    public class Common
    {
        private static char[] _splitChar = (" ").ToCharArray();

        public static IEnumerable<Card> ConvertInputToPlayerCards(string lineText)
        {
            var playerCardTexts = SplitLineToPlayerCardInputs(lineText);

            var test = new Card()
            {
                Value = GetCardValue(playerCardTexts[0].Substring(0, 1)),
                Suit = playerCardTexts[0].Substring(1)
            };

            return playerCardTexts.Select(text =>
            {
                if(!Regex.Match(text, "[2-9TJQKA][DHSC]").Success)
                    return null;

                return new Card()
                {
                    Value = GetCardValue(text.Substring(0, 1)),
                    Suit = text.Substring(1)
                };
            });
        }

        private static string[] SplitLineToPlayerCardInputs(string lineText)
        {
            if (string.IsNullOrWhiteSpace(lineText))
                return null;

            if (lineText.Length != 29)
                return null;

            return lineText.Split(_splitChar);
        }
        private static int GetCardValue(string value)
        {
            switch (value)
            {
                case "T":
                    return 10;
                case "J":
                    return 11;
                case "Q":
                    return 12;
                case "K":
                    return 13;
                case "A":
                    return 14;
                default:
                    return Convert.ToInt16(value);
            }
        }
    }
}
