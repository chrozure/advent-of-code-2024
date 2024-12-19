using System;
using System.IO;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Pass the file path and file name to the StreamReader constructor
            using StreamReader sr = new StreamReader("./day-1.in");

            List<int> firstColumn = new List<int>();
            List<int> secondColumn = new List<int>();
            Dictionary<int, int> secondColumnFreq = new Dictionary<int, int>();

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                string firstNumStr = line.Substring(0, 5);
                string secondNumStr = line.Substring(8);
                int firstNum = Int32.Parse(firstNumStr);
                int secondNum = Int32.Parse(secondNumStr);

                if (secondColumnFreq.ContainsKey(secondNum))
                {
                    secondColumnFreq[secondNum] = secondColumnFreq[secondNum] + 1;
                }
                else
                {
                    secondColumnFreq[secondNum] = 1;
                }

                firstColumn.Add(firstNum);
                secondColumn.Add(secondNum);
            }

            firstColumn.Sort();
            secondColumn.Sort();

            // Part 1
            int totalDistance = 0;
            for (int i = 0; i < firstColumn.Count; i++) {
                totalDistance += Math.Abs(firstColumn[i] - secondColumn[i]);
            }
            // 2031679
            Console.WriteLine(totalDistance);

            // Part 2
            int totalSimilarity = 0;
            for (int i = 0; i < firstColumn.Count; i++) {
                int number = firstColumn[i];
                if (secondColumnFreq.ContainsKey(number)) {
                    totalSimilarity += number * secondColumnFreq[number];
                }
            }
            // 19678534
            Console.WriteLine(totalSimilarity);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }
}
