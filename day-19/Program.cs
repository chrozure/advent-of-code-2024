using System;
using System.IO;
using System.Collections.Generic;


class Program
{

    static void Main()
    {
        try
        {
            using StreamReader sr = new("./day-19.in");

            string? line = sr.ReadLine()!;
            string[] patterns = line.Split(", ");

            int numPossibleDesigns = 0;
            long numPossibleDesignWays = 0;
            line = sr.ReadLine()!;
            while ((line = sr.ReadLine()) != null)
            {
                int lineLength = line.Length;
                List<long> matches = new(Enumerable.Repeat(0l, lineLength));

                for (int i = 0; i < lineLength; i++)
                {
                    foreach (string pattern in patterns)
                    {
                        int patternLength = pattern.Length;

                        // Matching from the start
                        if (i == patternLength - 1 && CanUsePaterm(line, pattern, i))
                        {
                            matches[i]++;
                        }
                        // Matching from a previously matched position
                        if (i >= patternLength && matches[i - patternLength] > 0 && CanUsePaterm(line, pattern, i))
                        {
                            matches[i] += matches[i - patternLength];
                        }
                    }
                }

                numPossibleDesignWays += matches[lineLength - 1];
                if (matches[lineLength - 1] > 0)
                {
                    numPossibleDesigns++;
                }
            }

            Console.WriteLine("Part 1: " + numPossibleDesigns);
            Console.WriteLine("Part 2: " + numPossibleDesignWays);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    // Helper function to determine if a pattern exists in the line
    // where the pattern ends at position
    private static bool CanUsePaterm(string line, string pattern, int position) {
        int patternLength = pattern.Length;

        for (int i = 0; i < patternLength; i++) {
            if (line[position - i] != pattern[patternLength - i - 1]) {
                return false;
            }
        }

        return true;
    }
}
