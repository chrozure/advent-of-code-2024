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
            List<List<int>> dist = [];
            List<List<(int, int)>> pred = [];

            string? line;
            int startRow = 0, startCol = 0, endRow = 0, endCol = 0;
            int lineNo = 0;
            while ((line = sr.ReadLine()) != null)
            {
                List<int> mapRow = [];
                List<int> distRow = [];
                List<(int, int)> predRow = [];

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

                    distRow.Add(INF);
                    predRow.Add((-1, -1));
                }

                map.Add(mapRow);
                dist.Add(distRow);
                pred.Add(predRow);
                lineNo++;
            }


            // 9476 steps required in normal map
            int numStepsRequired = BFS(startRow, startCol, endRow, endCol, map, dist, pred);
            Console.WriteLine("Number of steps in path: " + numStepsRequired);

            int numValidCheats = 0;
            for ((int row, int col) backtrack = (endRow, endCol); backtrack != (-1, -1); backtrack = pred[backtrack.row][backtrack.col])
            {
                numValidCheats += FindCheats(backtrack.row, backtrack.col, 2, 100, dist);
            }
            Console.WriteLine("Part 1: " + numValidCheats);

            numValidCheats = 0;
            for ((int row, int col) backtrack = (endRow, endCol); backtrack != (-1, -1); backtrack = pred[backtrack.row][backtrack.col])
            {
                numValidCheats += FindCheats(backtrack.row, backtrack.col, 20, 100, dist);
            }
            Console.WriteLine("Part 2: " + numValidCheats);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    // Run a breadth first search from the start to the finish
    // Returns the length of the path from the start to finish
    // Sets the values in each co-ordinate in dist to the distnace until the finish
    // and the predecessor as the predecessor for each position
    private static int BFS(int startRow, int startCol, int endRow, int endCol,
                        List<List<int>> map, List<List<int>> dist, List<List<(int, int)>> pred) {
        List<List<bool>> visited = [];

        for (int i = 0; i < SIZE; i++) {
            List<bool> visitedRow = [];
            List<(int, int)> predRow = [];
            for (int j = 0; j < SIZE; j++) {
                visitedRow.Add(false);
                predRow.Add((-1, -1));
            }

            visited.Add(visitedRow);
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

        // backtrack from end
        int numSteps = 0;
        for ((int row, int col) backtrack = (endRow, endCol); backtrack != (-1, -1); backtrack = pred[backtrack.row][backtrack.col])
        {
            dist[backtrack.row][backtrack.col] = numSteps;
            numSteps++;
        }

        return numSteps - 1;
    }

    private static int FindCheats(int row, int col, int distance, int timeSave, List<List<int>> dist)
    {

        int numCheats = 0;

        for (int i = -distance; i <= distance; i++)
        {
            for (int j = -distance + Math.Abs(i); j <= distance - Math.Abs(i); j++)
            {
                int posRow = row + i;
                int posCol = col + j;

                if (ValidPosition(posRow, posCol) && dist[row][col] - dist[posRow][posCol] - Math.Abs(i) - Math.Abs(j) >= timeSave)
                {
                    numCheats++;
                }
            }
        }

        return numCheats;
    }

    private static bool ValidPosition(int row, int col) {
        return row >= 0 && col >= 0 && row < SIZE && col < SIZE;
    }
}
