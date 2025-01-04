using System;
using System.IO;
using System.Collections.Generic;
using System.Data;


class Program
{

    private readonly static int WALL = 0;
    private readonly static int EMPTY = 1;
    private readonly static int END = 2;
    private readonly static int START = 3;

    private readonly static int INF = 9999999;

    private readonly static int UP = 10;
    private readonly static int DOWN = 11;
    private readonly static int LEFT = 12;
    private readonly static int RIGHT = 13;

    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new("./day-16.in");

            List<List<int>> map = [];
            List<List<int>> dist = [];

            int endRow = 0;
            int endCol = 0;
            int startRow = 0;
            int startCol = 0;
            string? line;
            int lineNo = 0;
            while ((line = sr.ReadLine()) != null)
            {
                List<int> row = [];
                List<int> distRow = [];

                for (int i = 0; i < line.Length; i++) {
                    char c = line[i];
                    if (c == '#') {
                        row.Add(WALL);
                    } else if (c == '.') {
                        row.Add(EMPTY);
                    } else if (c == 'E') {
                        row.Add(END);
                        endRow = lineNo;
                        endCol = i;
                    } else if (c == 'S') {
                        row.Add(START);
                        startRow = lineNo;
                        startCol = i;
                    }

                    distRow.Add(INF);
                }

                dist.Add(distRow);
                map.Add(row);
                lineNo++;
            }

            dist[startRow][startCol] = 0;
            var pq = new PriorityQueue<(int, int, int), int>();
            pq.Enqueue((startRow, startCol, RIGHT), 0);

            // Dijkstra's algorithm
            while (pq.TryDequeue(out (int row, int col, int direction) coordinate, out int distance)) {
                dist[coordinate.row][coordinate.col] = Math.Min(distance, dist[coordinate.row][coordinate.col]);

                // found end
                if (coordinate.row == endRow && coordinate.col == endCol) {
                    Console.WriteLine("Part 1: " + distance);
                    break;
                }

                if (coordinate.direction == RIGHT) {
                    // right
                    if (map[coordinate.row][coordinate.col + 1] != WALL && distance + 1 <= dist[coordinate.row][coordinate.col + 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col + 1, RIGHT), distance + 1);
                    }
                    // up
                    if (map[coordinate.row - 1][coordinate.col] != WALL && distance + 1001 <= dist[coordinate.row - 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row - 1, coordinate.col, UP), distance + 1001);
                    }
                    // down
                    if (map[coordinate.row + 1][coordinate.col] != WALL && distance + 1001 <= dist[coordinate.row + 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row + 1, coordinate.col, DOWN), distance + 1001);
                    }
                }

                else if (coordinate.direction == UP) {
                    // up
                    if (map[coordinate.row - 1][coordinate.col] != WALL && distance + 1 <= dist[coordinate.row - 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row - 1, coordinate.col, UP), distance + 1);
                    }
                    // left
                    if (map[coordinate.row][coordinate.col - 1] != WALL && distance + 1001 <= dist[coordinate.row][coordinate.col - 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col - 1, LEFT), distance + 1001);
                    }
                    // right
                    if (map[coordinate.row][coordinate.col + 1] != WALL && distance + 1001 <= dist[coordinate.row][coordinate.col + 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col + 1, RIGHT), distance + 1001);
                    }
                }

                else if (coordinate.direction == LEFT) {
                    // left
                    if (map[coordinate.row][coordinate.col - 1] != WALL && distance + 1 <= dist[coordinate.row][coordinate.col - 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col - 1, LEFT), distance + 1);
                    }
                    // up
                    if (map[coordinate.row - 1][coordinate.col] != WALL && distance + 1001 <= dist[coordinate.row - 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row - 1, coordinate.col, UP), distance + 1001);
                    }
                    // down
                    if (map[coordinate.row + 1][coordinate.col] != WALL && distance + 1001 <= dist[coordinate.row + 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row + 1, coordinate.col, DOWN), distance + 1001);
                    }
                }

                else if (coordinate.direction == DOWN) {
                    // down
                    if (map[coordinate.row + 1][coordinate.col] != WALL && distance + 1 <= dist[coordinate.row + 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row + 1, coordinate.col, DOWN), distance + 1);
                    }
                    // left
                    if (map[coordinate.row][coordinate.col - 1] != WALL && distance + 1001 <= dist[coordinate.row][coordinate.col - 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col - 1, LEFT), distance + 1001);
                    }
                    // right
                    if (map[coordinate.row][coordinate.col + 1] != WALL && distance + 1001 <= dist[coordinate.row][coordinate.col + 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col + 1, RIGHT), distance + 1001);
                    }
                }
            }

            // Part 2
            var tiles = DFS(endRow, endCol, startRow, startCol, dist);
            Console.WriteLine("Part 2: " + tiles.Count);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    // Part 2
    // Reverse DFS
    // Returns a set containing all the tiles that are part of at least one best path
    private static HashSet<(int, int)> DFS(int row, int col, int startRow, int startCol, List<List<int>> dist) {
        HashSet<(int, int)> s = [];
        s.Add((row, col));
        // Reached start position
        if (row == startRow && col == startCol) {
            return s;
        }

        int curDistance = dist[row][col];

        // For each direction:
        //  Check if the neighbour has a change of 1
        //      In this case, do a DFS on it since it would have been a predecessor
        //  If the distance is 1001, then check the position right after as well
        //      This is because there could have been a turn in the middle
        int down = dist[row + 1][col];
        if (down == curDistance - 1 || down == curDistance - 1001) {
            s.UnionWith(DFS(row + 1, col, startRow, startCol, dist));
        }
        if (down == curDistance - 1001 && dist[row + 2][col] == curDistance - 2) {
            s.UnionWith(DFS(row + 2, col, startRow, startCol, dist));
        }

        int left = dist[row][col - 1];
        if (left == curDistance - 1 || left == curDistance - 1001) {
            s.UnionWith(DFS(row, col - 1, startRow, startCol, dist));
        }
        if (left == curDistance - 1001 && dist[row][col - 2] == curDistance - 2) {
            s.UnionWith(DFS(row, col - 2, startRow, startCol, dist));
        }

        int up = dist[row - 1][col];
        if (up == curDistance - 1 || up == curDistance - 1001) {
            s.UnionWith(DFS(row - 1, col, startRow, startCol, dist));
        }
        if (up == curDistance - 1001 && dist[row - 2][col] == curDistance - 2) {
            s.UnionWith(DFS(row - 2, col, startRow, startCol, dist));
        }

        int right = dist[row][col + 1];
        if (right == curDistance - 1 || right == curDistance - 1001) {
            s.UnionWith(DFS(row, col + 1, startRow, startCol, dist));
        }
        if (right == curDistance - 1001 && dist[row][col + 2] == curDistance - 2) {
            s.UnionWith(DFS(row, col + 2, startRow, startCol, dist));
        }

        return s;
    }
}
