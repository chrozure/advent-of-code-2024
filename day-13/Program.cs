using System;
using System.IO;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new("./day-13.in");

            long totalTokens = 0;
            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                // Button A
                int buttonAX = Int32.Parse(line.Substring(12, 2));
                int buttonAY = Int32.Parse(line.Substring(18, 2));

                // Button B
                line = sr.ReadLine();
                int buttonBX = Int32.Parse(line.Substring(12, 2));
                int buttonBY = Int32.Parse(line.Substring(18, 2));

                // Prize
                line = sr.ReadLine();
                string[] lineSplit = line.Split(',');
                // Part 1
                // long prizeX = long.Parse(lineSplit[0].Split('=')[1]);
                // long prizeY = long.Parse(lineSplit[1].Split('=')[1]);

                // Part 2
                long prizeX = long.Parse(lineSplit[0].Split('=')[1]) + 10000000000000;
                long prizeY = long.Parse(lineSplit[1].Split('=')[1]) + 10000000000000;

                // Empty line
                line = sr.ReadLine();

                /* Brute force solution for part 1 */
                // int minTokens = 100000;
                // bool canGetPrize = false;
                // for (int i = 0; i < 100; i++) {
                //     for (int j = 0; j < 100; j++) {
                //         if (i * buttonAX + j * buttonBX == prizeX &&
                //             i * buttonAY + j * buttonBY == prizeY &&
                //             3 * i + j < minTokens) {
                //                 canGetPrize = true;
                //                 minTokens = 3 * i + j;
                //             }
                //     }
                // }

                // if (canGetPrize) {
                //     totalTokens += minTokens;
                // }

                // Calculate the solution using linear algebra techniques
                long det = buttonAX * buttonBY - buttonAY * buttonBX;

                // infinite or no solutions
                if (det == 0) continue;

                long inverseX = buttonBY * prizeX - buttonBX * prizeY;
                long inverseY = buttonAX * prizeY - buttonAY * prizeX;

                // Invertible with integer solution
                if (inverseX % det == 0 && inverseY % det == 0)
                {
                    long numXPushes = inverseX / det;
                    long numYPushes = inverseY / det;

                    totalTokens += numXPushes * 3 + numYPushes;
                }
            }

            Console.WriteLine(totalTokens);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }
}
