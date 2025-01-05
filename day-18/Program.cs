using System;
using System.IO;
using System.Collections.Generic;


class Program
{

    private readonly static int SIZE = 71;
    private readonly static int NUM_INITIAL_BYTES = 1024;

    private readonly static int UNCORRUPTED = 0;
    private readonly static int CORRUPTED = 1;

    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new("./day-18.in");
            List<List<int>> memorySpace = [];
            List<List<(int, int)>> pred = [];
            List<List<bool>> visited = [];

            for (int i = 0; i < SIZE; i++) {
                List<int> memoryRow = [];
                List<(int, int)> predRow = [];
                List<bool> visitedRow = [];
                for (int j = 0; j < SIZE; j++) {
                    memoryRow.Add(UNCORRUPTED);
                    predRow.Add((-1, -1));
                    visitedRow.Add(false);
                }
                memorySpace.Add(memoryRow);
                pred.Add(predRow);
                visited.Add(visitedRow);
            }

            for (int i = 0; i < NUM_INITIAL_BYTES; i++) {
                string line = sr.ReadLine()!;
                string[] lineSplit = line.Split(',');

                int x = int.Parse(lineSplit[0]);
                int y = int.Parse(lineSplit[1]);

                memorySpace[y][x] = CORRUPTED;
            }

            /* Part 1 */
            // Do a BFS from the top left corner
            Queue<(int, int)> q = new([(0, 0)]);
            while (q.Count > 0) {
                (int row, int col) = q.Dequeue();
                if (row == SIZE - 1 && col == SIZE - 1) {
                    break;
                }

                (int, int)[] directions = [(1, 0), (0, 1), (-1, 0), (0, -1)];
                foreach ((int rowDir, int colDir) in directions) {
                    int newRow = row + rowDir;
                    int newCol = col + colDir;

                    if (newRow >= 0 && newCol >= 0 && newRow < SIZE && newCol < SIZE && memorySpace[newRow][newCol] != CORRUPTED && !visited[newRow][newCol]) {
                        visited[newRow][newCol] = true;
                        q.Enqueue((newRow, newCol));
                        pred[newRow][newCol] = (row, col);
                    }
                }
            }

            // Backtrack from end to find the length of the shortest path
            (int, int) backTrack = (SIZE - 1, SIZE - 1);
            int numSteps = 0;
            while (!backTrack.Equals((0, 0))) {
                backTrack = pred[backTrack.Item1][backTrack.Item2];
                numSteps++;
            }

            Console.WriteLine("Part 1: " + numSteps);


            /* Part 2 */
            // Repeatedly add in bytes and do a DFS
            string? newLine;
            while ((newLine = sr.ReadLine()) != null) {
                string[] lineSplit = newLine.Split(',');

                int x = int.Parse(lineSplit[0]);
                int y = int.Parse(lineSplit[1]);

                memorySpace[y][x] = CORRUPTED;

                // Reset visited matrix
                for (int i = 0; i < SIZE; i++) {
                    for (int j = 0; j < SIZE; j++) {
                        visited[i][j] = false;
                    }
                }

                if (!DFS(memorySpace, 0, 0, visited)) {
                    Console.WriteLine("Part 2: " + x + "," + y);
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    // Part 2
    // Conducts a depth first search from the top left to the bottom right
    // Returns true if it is able to make it to the bottom right, false otherwise
    private static bool DFS(List<List<int>> memory, int row, int col, List<List<bool>> visited) {
        // Made it to bottom right
        if (row == SIZE - 1 && col == SIZE - 1) {
            return true;
        }
        visited[row][col] = true;

        (int, int)[] directions = [(1, 0), (0, 1), (-1, 0), (0, -1)];
        foreach ((int rowDir, int colDir) in directions) {
            int newRow = row + rowDir;
            int newCol = col + colDir;

            if (newRow >= 0 && newCol >= 0 && newRow < SIZE && newCol < SIZE && memory[newRow][newCol] != CORRUPTED && !visited[newRow][newCol]) {
                bool reachable = DFS(memory, newRow, newCol, visited);
                if (reachable) return true;
            }
        }

        return false;
    }

}
