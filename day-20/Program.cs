using System;
using System.IO;
using System.Collections.Generic;

class Program
{

    private readonly static int WALL = 0;
    private readonly static int TRACK = 1;

    private readonly static int SIZE = 141;

    private readonly static int INF = 99999999;

    static void Main()
    {
        try
        {
            using StreamReader sr = new("./day-20.in");

            List<List<int>> map = [];

            string? line;
            int startRow = 0, startCol = 0, endRow = 0, endCol = 0;
            int lineNo = 0;
            while ((line = sr.ReadLine()) != null)
            {
                List<int> mapRow = [];
                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];
                    if (c == '#') mapRow.Add(WALL);
                    else
                    {
                        mapRow.Add(TRACK);
                        if (c == 'S')
                        {
                            startRow = lineNo;
                            startCol = j;
                        }
                        else if (c == 'E')
                        {
                            endRow = lineNo;
                            endCol = j;
                        }
                    }
                }

                map.Add(mapRow);
                lineNo++;
            }

            // PrintMap(map);

            // 9476 steps required in normal map
            int numStepsRequired = BFS(startRow, startCol, endRow, endCol, map);
            Console.WriteLine("Number of steps in normal map: " +numStepsRequired);

            // For this part we will turn each wall into a track individually
            // and then run a BFS and see if the time taken is shorter
            int numValidCheats = 0;
            for (int i = 0; i < SIZE; i++) {
                for (int j = 0; j < SIZE; j++) {
                    if (map[i][j] != WALL) continue;

                    map[i][j] = TRACK;
                    int numSteps = BFS(startRow, startCol, endRow, endCol, map);

                    if (numSteps <= numStepsRequired - 100) {
                        numValidCheats++;
                    }
                    map[i][j] = WALL;
                }
            }

            Console.WriteLine("Part 1: " + numValidCheats);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    private static int BFS(int startRow, int startCol, int endRow, int endCol, List<List<int>> map) {
        List<List<bool>> visited = [];
        List<List<(int, int)>> pred = [];

        for (int i = 0; i < SIZE; i++) {
            List<bool> visitedRow = [];
            List<(int, int)> predRow = [];
            for (int j = 0; j < SIZE; j++) {
                visitedRow.Add(false);
                predRow.Add((-1, -1));
            }

            visited.Add(visitedRow);
            pred.Add(predRow);
        }

        Queue<(int, int)> q = new([(startRow, startCol)]);
        visited[startRow][startCol] = true;
        while (q.Count > 0) {
            (int row, int col) = q.Dequeue();
            if (row == endRow && col == endCol)
            {
                break;
            }

            (int, int)[] directions = [(0, 1), (1, 0), (-1, 0), (0, -1)];
            foreach ((int rowDir, int colDir) in directions) {
                int newRow = row + rowDir;
                int newCol = col + colDir;

                if (ValidPosition(newRow, newCol) && map[newRow][newCol] != WALL && !visited[newRow][newCol]) {
                    visited[newRow][newCol] = true;
                    pred[newRow][newCol] = (row, col);
                    q.Enqueue((newRow, newCol));
                }
            }
        }

        int numSteps = 0;
        for ((int, int) backtrack = (endRow, endCol); backtrack != (startRow, startCol); backtrack = pred[backtrack.Item1][backtrack.Item2])
        {
            numSteps++;
        }

        return numSteps;
    }

    private static bool ValidPosition(int row, int col) {
        return row >= 0 && col >= 0 && row < SIZE && col < SIZE;
    }

    private static void PrintMap(List<List<int>> map) {
        for (int i = 0; i < map.Count; i++) {
            for (int j = 0; j < map[0].Count; j++) {
                if (map[i][j] == WALL) {
                    Console.Write("#");
                } else if (map[i][j] == TRACK) {
                    Console.Write(".");
                }
            }

            Console.WriteLine();
        }
    }
}
