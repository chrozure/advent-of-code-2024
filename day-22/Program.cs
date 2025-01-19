using System;
using System.IO;
using System.Collections.Generic;


class Program
{

    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new("./day-22.in");

            string? line;
            // long totalSecrets = 0;
            Dictionary<(long, long, long, long), long> sequenceToProfit = [];
            while ((line = sr.ReadLine()) != null)
            {
                long secretNumber = long.Parse(line);
                long profit = secretNumber % 10;
                List<long> profitSequence = [];
                for (int i = 0; i < 3; i++)
                {
                    secretNumber = NextSecretNumber(secretNumber);
                    profitSequence.Add(secretNumber % 10 - profit);
                    profit = secretNumber % 10;
                }

                HashSet<(long, long, long, long)> seenSequences = [];

                for (int i = 0; i < 1997; i++)
                {
                    secretNumber = NextSecretNumber(secretNumber);
                    profitSequence.Add(secretNumber % 10 - profit);
                    profit = secretNumber % 10;

                    (long, long, long, long) currentSequence = (profitSequence[0], profitSequence[1], profitSequence[2], profitSequence[3]);

                    // Need to make sure it is the first occurrence of
                    // this sequence for the current monkey
                    if (!seenSequences.Contains(currentSequence))
                    {
                        seenSequences.Add(currentSequence);
                        if (!sequenceToProfit.ContainsKey(currentSequence))
                        {
                            sequenceToProfit[currentSequence] = 0;
                        }
                        sequenceToProfit[currentSequence] += profit;
                    }

                    profitSequence.RemoveAt(0);
                }

                /* Part 1 */
                // for (int i = 0; i < 2000; i++)
                // {
                //     secretNumber = Prune(Mix(secretNumber, secretNumber * 64));
                //     secretNumber = Prune(Mix(secretNumber, secretNumber / 32));
                //     secretNumber = Prune(Mix(secretNumber, secretNumber * 2048));
                // }
                // totalSecrets += secretNumber;

            }

            Console.WriteLine(Enumerable.Max(sequenceToProfit.Values.ToList()));
            // Console.WriteLine(totalSecrets);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    private static long NextSecretNumber(long secretNumber)
    {
        secretNumber = Prune(Mix(secretNumber, secretNumber * 64));
        secretNumber = Prune(Mix(secretNumber, secretNumber / 32));
        secretNumber = Prune(Mix(secretNumber, secretNumber * 2048));

        return secretNumber;
    }

    private static long Mix(long secretNumber, long otherNumber)
    {
        return secretNumber ^ otherNumber;
    }

    private static long Prune(long secretNumber)
    {
        return secretNumber % 16777216;
    }
}
