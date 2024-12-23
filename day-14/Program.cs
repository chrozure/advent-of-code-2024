using System;
using System.IO;
using System.Collections.Generic;


class Program
{

    private static readonly int NUM_ROWS = 103;
    private static readonly int NUM_COLS = 101;

    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new("./day-14.in");

            List<Robot> robots = [];

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] lineSplit = line.Split(' ');
                string[] positions = lineSplit[0].Split('=')[1].Split(',');
                int xPos = Int32.Parse(positions[0]);
                int yPos = Int32.Parse(positions[1]);

                string[] velocities = lineSplit[1].Split('=')[1].Split(',');
                int xVel = Int32.Parse(velocities[0]);
                int yVel = Int32.Parse(velocities[1]);

                Robot newRobot = new(xPos, yPos, xVel, yVel);
                robots.Add(newRobot);
            }

            // Part 1
            for (int i = 1; i <= 100; i++) {
                foreach (Robot r in robots) {
                    r.Move(NUM_ROWS, NUM_COLS);
                }
            }

            // Xount number of robots in each quadrant
            int[] robotsInQuadrants = [0, 0, 0, 0];
            foreach (Robot r in robots) {
                if (r.XPos > NUM_COLS / 2) {
                    if (r.YPos < NUM_ROWS / 2) {
                        robotsInQuadrants[0]++;
                    } else if (r.YPos > NUM_ROWS / 2) {
                        robotsInQuadrants[3]++;
                    }
                } else if (r.XPos < NUM_COLS / 2) {
                    if (r.YPos < NUM_ROWS / 2) {
                        robotsInQuadrants[1]++;
                    } else if (r.YPos > NUM_ROWS / 2) {
                        robotsInQuadrants[2]++;
                    }
                }
            }

            int safetyFactor = 1;
            foreach (int numRobots in robotsInQuadrants) {
                safetyFactor *= numRobots;
            }

            Console.WriteLine(safetyFactor);

            // Part 2
            for (int i = 101; i <= 7687; i++) {
                foreach (Robot r in robots) {
                    r.Move(NUM_ROWS, NUM_COLS);
                }
                // PrintMap(robots);
                // Console.WriteLine("Iteration " + i);
                // Console.ReadLine();
            }
            PrintMap(robots);
            // Pattern appears at
            // 112 168 213 271 314 374 415 477 516 ...
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    private static void PrintMap(List<Robot> robots) {
        List<List<int>> map = [];
        for (int i = 0; i < NUM_ROWS; i++) {
            List<int> mapRow = [];
            for (int j = 0; j < NUM_COLS; j++) {
                mapRow.Add(0);
            }
            map.Add(mapRow);
        }

        foreach (Robot r in robots) {
            map[r.YPos][r.XPos]++;
        }

        foreach (List<int> row in map) {
            foreach (int col in row) {
                if (col == 0) Console.Write(" ");
                else Console.Write(col);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
