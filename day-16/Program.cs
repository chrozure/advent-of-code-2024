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


            while (pq.TryDequeue(out (int row, int col, int direction) coordinate, out int distance)) {
                Console.WriteLine(coordinate.row + " " + coordinate.col + " " + distance);
                dist[coordinate.row][coordinate.col] = distance;

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
                    // left
                    if (map[coordinate.row][coordinate.col - 1] != WALL && distance + 2001 <= dist[coordinate.row][coordinate.col - 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col - 1, LEFT), distance + 2001);
                    }
                }

                else if (coordinate.direction == UP) {
                    if (map[coordinate.row][coordinate.col + 1] != WALL && distance + 1001 <= dist[coordinate.row][coordinate.col + 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col + 1, RIGHT), distance + 1001);
                    }
                    if (map[coordinate.row - 1][coordinate.col] != WALL && distance + 1 <= dist[coordinate.row - 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row - 1, coordinate.col, UP), distance + 1);
                    }
                    if (map[coordinate.row + 1][coordinate.col] != WALL && distance + 2001 <= dist[coordinate.row + 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row + 1, coordinate.col, DOWN), distance + 2001);
                    }
                    if (map[coordinate.row][coordinate.col - 1] != WALL && distance + 1001 <= dist[coordinate.row][coordinate.col - 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col - 1, LEFT), distance + 1001);
                    }
                }

                else if (coordinate.direction == LEFT) {
                    if (map[coordinate.row][coordinate.col + 1] != WALL && distance + 2001 <= dist[coordinate.row][coordinate.col + 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col + 1, RIGHT), distance + 2001);
                    }
                    if (map[coordinate.row - 1][coordinate.col] != WALL && distance + 1001 <= dist[coordinate.row - 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row - 1, coordinate.col, UP), distance + 1001);
                    }
                    if (map[coordinate.row + 1][coordinate.col] != WALL && distance + 1001 <= dist[coordinate.row + 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row + 1, coordinate.col, DOWN), distance + 1001);
                    }
                    if (map[coordinate.row][coordinate.col - 1] != WALL && distance + 1 <= dist[coordinate.row][coordinate.col - 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col - 1, LEFT), distance + 1);
                    }
                }

                else if (coordinate.direction == DOWN) {
                    if (map[coordinate.row][coordinate.col + 1] != WALL && distance + 1001 <= dist[coordinate.row][coordinate.col + 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col + 1, RIGHT), distance + 1001);
                    }
                    if (map[coordinate.row - 1][coordinate.col] != WALL && distance + 2001 <= dist[coordinate.row - 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row - 1, coordinate.col, UP), distance + 2001);
                    }
                    if (map[coordinate.row + 1][coordinate.col] != WALL && distance + 1 <= dist[coordinate.row + 1][coordinate.col]) {
                        pq.Enqueue((coordinate.row + 1, coordinate.col, DOWN), distance + 1);
                    }
                    if (map[coordinate.row][coordinate.col - 1] != WALL && distance + 1001 <= dist[coordinate.row][coordinate.col - 1]) {
                        pq.Enqueue((coordinate.row, coordinate.col - 1, LEFT), distance + 1001);
                    }
                }
            }

            // 521 too low
            // 5021 too high
            // var tiles = DFS(endRow, endCol, startRow, startCol, pred);
            // Console.WriteLine(tiles.Count);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    private static HashSet<(int, int)> DFS(int row, int col, int startRow, int startCol, Dictionary<(int, int), List<(int, int)>> pred) {
        HashSet<(int, int)> s = [];
        s.Add((row, col));
        if (row == startRow && col == startCol) {
            return s;
        }

        foreach ((int r, int c) in pred[(row, col)]) {
            s.Add((r, c));
            s.UnionWith(DFS(r, c, startRow, startCol, pred));
        }

        return s;
    }
}
