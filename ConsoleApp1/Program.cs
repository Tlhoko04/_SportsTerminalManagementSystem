using System;
using System.Linq;

namespace ArenaManagementSuite
{
    class Program
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("      SPORTS & ARENA MANAGEMENT SYSTEM  ");
                Console.WriteLine("==================================================");
                Console.WriteLine("1.Mass Converter");
                Console.WriteLine("2.Point-of-Sale Register");
                Console.WriteLine("3.Secure Team Dispatcher: Reverse & Encode");
                Console.WriteLine("4.Darts Tournament Simulator (Jagged Arrays) ");
                Console.WriteLine("5. Exit System");
                Console.WriteLine("==================================================");
                Console.Write("Select an option (1-5): ");

                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine("\n");

                switch (choice)
                {
                    case '1':
                        RunUnitCostCalculator();
                        break;
                    case '2':
                        RunConcessionBilling();
                        break;
                    case '3':
                        RunTextTransformationMenu();
                        break;
                    case '4':
                        RunDartsTournament();
                        break;
                    case '5':
                        running = false;
                        Console.WriteLine("Shutting down Arena Suite. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Press any key to retry...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        #region Module 1: Concession Mass & Unit Price Converter (Q1)
        static void RunUnitCostCalculator()
        {
            Console.Clear();
            Console.WriteLine("--- Bulk Price Calculator ---");

            try
            {
                Console.Write("Enter item name: ");
                string itemName = Console.ReadLine();

                Console.Write("Enter mass of the item (kg): ");
                double massKg = double.Parse(Console.ReadLine());

                Console.Write("Enter item total price: ");
                decimal price = decimal.Parse(Console.ReadLine());

                if (massKg <= 0 || price < 0)
                {
                    throw new FormatException("Mass must be positive and price non-negative.");
                }

                double massPounds = massKg * 2.205;
                decimal costPerKg = price / (decimal)massKg;
                decimal costPerPound = price / (decimal)massPounds;

                Console.WriteLine();
                Console.WriteLine($"{itemName} costs {costPerKg:C}/kg.");
                Console.WriteLine($"That equates to {costPerPound:C}/pound.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid numeric input was entered. Operation aborted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            Pause();
        }
        #endregion

        #region Module 2: Point-of-Sale Dynamic Register (Q2)
        static void RunConcessionBilling()
        {
            Console.Clear();
            Console.WriteLine("--- Concession POS Register ---");

            string[] itemNames = new string[0];
            decimal[] itemPrices = new decimal[0];

            while (IsAnother())
            {
                Array.Resize(ref itemNames, itemNames.Length + 1);
                Array.Resize(ref itemPrices, itemPrices.Length + 1);

                Console.WriteLine();
                itemNames[itemNames.Length - 1] = GetItemName();
                itemPrices[itemPrices.Length - 1] = GetItemPrice($"Enter price for {itemNames[itemNames.Length - 1]}: ");
            }

            Console.Clear();
            DisplayBill(itemNames, itemPrices);
            Pause();
        }

        static bool IsAnother()
        {
            Console.Write("Add an item (Y/N)? ");
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine();
            return char.ToUpper(key) == 'Y';
        }

        static string GetItemName()
        {
            string name = string.Empty;
            do
            {
                Console.Write("Enter item name: ");
                name = Console.ReadLine()?.Trim() ?? string.Empty;
            } while (string.IsNullOrEmpty(name));

            return name;
        }

        static decimal GetItemPrice(string prompt)
        {
            decimal price;
            do
            {
                Console.Write(prompt);
            } while (!decimal.TryParse(Console.ReadLine(), out price) || price <= 0);

            return price;
        }

        static void DisplayBill(string[] items, decimal[] prices)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("              OFFICIAL RECEIPT          ");
            Console.WriteLine("========================================");

            for (int i = 0; i < items.Length; i++)
            {
                Console.WriteLine(items[i].PadRight(28) + " | " + prices[i].ToString("C").PadLeft(9));
            }

            Console.WriteLine("".PadRight(28, '-') + "-+-" + "".PadRight(9, '-'));
            Console.WriteLine("Total".PadRight(28) + " | " + prices.Sum().ToString("C").PadLeft(9));
            Console.WriteLine("========================================");
        }
        #endregion

        #region Module 3: Sentence Reversal & Alphabet Encoding (Q3)
        static void RunTextTransformationMenu()
        {
            Console.Clear();
            Console.WriteLine("--- Secure Team Dispatcher ---");
            Console.Write("Enter a sentence ending with a period (.): ");
            string sentence = Console.ReadLine()?.ToLower();

            if (string.IsNullOrEmpty(sentence) || !sentence.EndsWith("."))
            {
                Console.WriteLine("Sentence must not be empty and must terminate with a period.");
                Pause();
                return;
            }

            Console.WriteLine("\n1. Reverse all words in-place.");
            Console.WriteLine("2. Encode to numeric cipher.");
            Console.Write("\nChoose action (1 or 2): ");
            char choice = Console.ReadKey().KeyChar;
            Console.WriteLine("\n");

            switch (choice)
            {
                case '1':
                    ReverseWords(ref sentence);
                    Console.WriteLine("Reversed: " + sentence);
                    break;
                case '2':
                    string encoded = Encode(sentence);
                    Console.WriteLine("Encoded: " + encoded);
                    break;
                default:
                    Console.WriteLine("Invalid selection. Terminating module.");
                    break;
            }

            Pause();
        }

        // Custom string manipulation: strictly character traversal without .NET Split/Reverse
        static void ReverseWords(ref string s)
        {
            string result = "";
            int lastBoundary = -1;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ' || s[i] == '.')
                {
                    // Reverse letters of current token
                    for (int j = i - 1; j > lastBoundary; j--)
                    {
                        result += s[j];
                    }

                    if (s[i] == ' ')
                    {
                        result += ' ';
                    }

                    lastBoundary = i;
                }
            }

            s = result + '.';
        }

        // Custom cipher: a -> 1*, b -> 2*, c-z -> 3-26, punctuation preserved
        static string Encode(string s)
        {
            string encoded = "";

            for (int i = 0; i < s.Length; i++)
            {
                int charIdx = Alphabet.IndexOf(s[i]);
                if (charIdx >= 0)
                {
                    encoded += (charIdx + 1).ToString();
                    if (charIdx == 0 || charIdx == 1) // 'a' or 'b'
                    {
                        encoded += "*";
                    }
                }
                else
                {
                    encoded += s[i];
                }
            }

            return encoded;
        }
        #endregion

        #region Module 4: Darts Jagged Array Tournament Simulation (Q4)
        static void RunDartsTournament()
        {
            Console.Clear();
            Console.WriteLine("--- Darts Match Tournament Simulator ---");

            int players = PromptRange("Enter number of players (2-4): ", 2, 4);
            int rounds = PromptRange("Enter number of rounds (1-10): ", 1, 10);

            Console.WriteLine();

            int[][] scorecard = GenerateScoreCard(players, rounds);
            SimulateMatch(scorecard);
            int[] totals = GetTotals(scorecard);
            PrintScores(scorecard, totals);

            Pause();
        }

        static int PromptRange(string prompt, int min, int max)
        {
            int val;
            do
            {
                Console.Write(prompt);
            } while (!int.TryParse(Console.ReadLine(), out val) || val < min || val > max);

            return val;
        }

        static int[][] GenerateScoreCard(int numPlayers, int numRounds)
        {
            int[][] scorecard = new int[numPlayers][];
            for (int i = 0; i < numPlayers; i++)
            {
                scorecard[i] = new int[numRounds];
            }
            return scorecard;
        }

        static void SimulateMatch(int[][] scorecard)
        {
            Random rng = new Random();
            for (int i = 0; i < scorecard.Length; i++)
            {
                for (int j = 0; j < scorecard[i].Length; j++)
                {
                    scorecard[i][j] = rng.Next(1, 21); // Scores 1-20
                }
            }
        }

        static int[] GetTotals(int[][] scorecard)
        {
            int[] totals = new int[scorecard.Length];
            for (int i = 0; i < scorecard.Length; i++)
            {
                for (int j = 0; j < scorecard[i].Length; j++)
                {
                    totals[i] += scorecard[i][j];
                }
            }
            return totals;
        }

        static void PrintScores(int[][] scorecard, int[] totals)
        {
            Console.WriteLine("Rounds Matrix" + "".PadRight(scorecard[0].Length * 4 - 8) + "| Total");
            Console.WriteLine("".PadRight(scorecard[0].Length * 4 + 8, '-'));

            for (int i = 0; i < scorecard.Length; i++)
            {
                Console.Write($"P{i + 1}: ");
                foreach (int score in scorecard[i])
                {
                    Console.Write(score.ToString().PadLeft(2) + "  ");
                }

                Console.WriteLine("| " + totals[i].ToString().PadLeft(4));
            }
        }
        #endregion

        static void Pause()
        {
            Console.Write("\nPress any key to return to main menu... ");
            Console.ReadKey();
        }
    }
}