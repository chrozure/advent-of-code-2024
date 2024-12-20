using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;


class Program
{

    public static int SIZE = 140;

    private static readonly int UP = 0;
    // private static readonly int RIGHT = 1;
    private static readonly int DOWN = 2;
    // private static readonly int LEFT = 3;


    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new StreamReader("./day-12.in");
            List<string> farm = [];

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                farm.Add(line);
            }

            List<List<bool>> visited = [];
            for (int i = 0; i < SIZE; i++)
            {
                List<bool> row = [];
                for (int j = 0; j < SIZE; j++)
                {
                    row.Add(false);
                }
                visited.Add(row);
            }

            int totalFencing = 0;
            int discountedFencing = 0;
            Dictionary<(int row, int col), List<int>> fences = [];
            for (int i = 0; i < farm.Count; i++)
            {
                string row = farm[i];
                for (int j = 0; j < row.Length; j++)
                {
                    if (!visited[i][j]) {
                        (int area, int perimeter) = DFS(visited, i, j, farm, fences);
                        // Part 1
                        totalFencing += area * perimeter;

                        // Part 2
                        int numSides = FindNumSides(fences);
                        discountedFencing += area * numSides;

                        fences.Clear();
                    }
                }
            }

            Console.WriteLine(totalFencing);
            Console.WriteLine(discountedFencing);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    // Computes dfs from given source
    // Returns a tuple with the area and perimeter
    static private (int area, int perimeter) DFS(List<List<bool>> visited, int row, int col,
                                                 List<string> farm,
                                                 Dictionary<(int, int), List<int>> fences) {
        if (visited[row][col]) {
            return (0, 0);
        }

        visited[row][col] = true;
        char curPlant = farm[row][col];
        (int area, int perimeter) t = (1, 4);
        List<(int, int)> directions = new List<(int, int)> {(-1, 0), (0, 1), (1, 0), (0, -1)};

        for (int i = 0; i < directions.Count; i++) {
            (int, int) direction = directions[i];
            int newRow = row + direction.Item1;
            int newCol = col + direction.Item2;

            // If is valid, recursively DFS
            if (newRow >= 0 && newRow < SIZE && newCol >= 0 && newCol < SIZE && farm[newRow][newCol] == curPlant)
            {
                (int, int) result = DFS(visited, newRow, newCol, farm, fences);
                t.area += result.Item1;
                t.perimeter += result.Item2 - 1;
            }
            // Otherwise, place a fence in that position
            else
            {
                if (!fences.ContainsKey((row, col))) {
                    fences.Add((row, col), []);
                }
                fences[(row, col)].Add(i);
            }
        }

        return t;
    }

    // Given a dictionary with the co-ordinates of plants and the fences they have,
    // determine the number of sides that the area has
    static private int FindNumSides(Dictionary<(int row, int col), List<int>> fences) {
        int numSides = 0;
        while (fences.Count > 0) {
            List<(int, int)> keyPositions = new List<(int, int)>(fences.Keys);
            (int row, int col) interiorPoint = keyPositions[0];
            List<int> fencePositions = fences[interiorPoint];
            int curFencePos = fencePositions[0];
            numSides++;

            int curRow = interiorPoint.row;
            int curCol = interiorPoint.col;

            // Search horitontally for all parts on the same side
            if (curFencePos == UP || curFencePos == DOWN)
            {
                while (curCol >= 0 && fences.ContainsKey((curRow, curCol)) && fences[(curRow, curCol)].Contains(curFencePos)) {
                    fences[(curRow, curCol)].Remove(curFencePos);
                    if (fences[(curRow, curCol)].Count == 0) fences.Remove((curRow, curCol));
                    curCol--;
                }

                curCol = interiorPoint.col + 1;
                while (curCol < SIZE && fences.ContainsKey((curRow, curCol)) && fences[(curRow, curCol)].Contains(curFencePos)) {
                    fences[(curRow, curCol)].Remove(curFencePos);
                    if (fences[(curRow, curCol)].Count == 0) fences.Remove((curRow, curCol));
                    curCol++;
                }
            }

            // Search vertically
            else
            {
                while (curRow >= 0 && fences.ContainsKey((curRow, curCol)) && fences[(curRow, curCol)].Contains(curFencePos)) {
                    fences[(curRow, curCol)].Remove(curFencePos);
                    if (fences[(curRow, curCol)].Count == 0) fences.Remove((curRow, curCol));
                    curRow--;
                }

                curRow = interiorPoint.row + 1;
                while (curRow < SIZE && fences.ContainsKey((curRow, curCol)) && fences[(curRow, curCol)].Contains(curFencePos)) {
                    fences[(curRow, curCol)].Remove(curFencePos);
                    if (fences[(curRow, curCol)].Count == 0) fences.Remove((curRow, curCol));
                    curRow++;
                }

            }
        }

        return numSides;
    }
}
