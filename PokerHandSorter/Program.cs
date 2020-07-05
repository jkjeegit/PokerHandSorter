using PokerHandSorter.Models;
using PokerHandSorter.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PokerHandSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = string.Empty;
            if (args.Length > 0)
            {
                filePath = args[0];

                if (File.Exists(filePath))
                {
                    Execute(filePath);
                }
                else
                    Console.WriteLine("Path not found please try again.");
            }
            else
                Console.WriteLine("Please provide input file path and try again.");
        }

        static void Execute(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            var rankEngine = new RankEngine();

            List<Card> playerOneCards;
            List<Card> playerTwoCards;

            int playerOneHandCount = 0;
            int playerTwoHandCount = 0;

            for (int index = 0; index < lines.Count(); index++)
            {
                var playerCards = Common.ConvertInputToPlayerCards(lines[index]);

                playerCards = playerCards.Where(card => card != null);

                if (playerCards.Count() != 10)
                {
                    continue;
                }

                playerOneCards = playerCards.Take(5).ToList();
                playerTwoCards = playerCards.Skip(5).Take(5).ToList();

                var playerOneRank = rankEngine.CalculateRank(playerOneCards);
                var playerTwoRank = rankEngine.CalculateRank(playerTwoCards);

                if (playerOneRank == playerTwoRank)
                {
                    playerOneRank += rankEngine.CalculateRankOnTie(playerOneCards, playerTwoCards, playerOneRank);
                    playerTwoRank += rankEngine.CalculateRankOnTie(playerTwoCards, playerOneCards, playerTwoRank);
                }

                if (playerOneRank > playerTwoRank)
                {
                    playerOneHandCount++;
                }
                else
                {
                    playerTwoHandCount++;
                }
            }

            Console.WriteLine($"Player 1: {playerOneHandCount}");
            Console.WriteLine($"Player 2: {playerTwoHandCount}");
        }
    }
}
