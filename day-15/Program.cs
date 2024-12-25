using System;
using System.IO;
using System.Collections.Generic;


class Program
{

    private readonly static int GRID_SIZE = 50;
    private readonly static int WALL = 0;
    private readonly static int BOX = 1;
    private readonly static int ROBOT = 2;
    private readonly static int EMPTY = 3;
    private readonly static int BOX_LEFT = 4;
    private readonly static int BOX_RIGHT = 5;

    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new("./day-15.in");

            // First read in map
            List<List<int>> map = [];
            string? line;
            int curRow = 0;
            int curCol = 0;
            for (int i = 0; i < GRID_SIZE; i++) {
                line = sr.ReadLine()!;
                List<int> row = [];
                for (int j = 0; j < line.Length; j++) {
                    char c = line[j];
                    if (c == '#') {
                        row.Add(WALL);
                        row.Add(WALL);
                    } else if (c == 'O') {
                        // row.Add(BOX);
                        row.Add(BOX_LEFT);
                        row.Add(BOX_RIGHT);
                    } else if (c == '@') {
                        curRow = i;
                        // curCol = j;
                        curCol = j * 2;
                        row.Add(ROBOT);
                        row.Add(EMPTY);
                    } else {
                        row.Add(EMPTY);
                        row.Add(EMPTY);
                    }
                }

                map.Add(row);
            }

            // Then execute all the robot moves
            sr.ReadLine();
            PrintMap(map);
            // line = sr.ReadLine();
            while ((line = sr.ReadLine()) != null) {
                foreach (char direction in line) {
                    if (direction == '<') {
                        Console.WriteLine("LEFT");

                        // Find next free space
                        int lastCol = curCol - 1;

                        // Part 1
                        while (map[curRow][lastCol] == BOX) {
                            lastCol--;
                        }
                        // Part 2
                        while (map[curRow][lastCol] == BOX_RIGHT) {
                            lastCol -= 2;
                        }

                        // Moving boxes into wall does nothing
                        if (map[curRow][lastCol] == WALL) {
                            continue;
                        }

                        // Move last box down if there were boxes
                        while (lastCol < curCol) {
                            map[curRow][lastCol] = map[curRow][lastCol + 1];
                            map[curRow][lastCol + 1] = EMPTY;
                            lastCol++;
                        }
                        curCol--;

                        // PrintMap(map);
                    }

                    else if (direction == '>') {
                        Console.WriteLine("RIGHT");

                        int lastCol = curCol + 1;
                        // while (map[curRow][lastCol] == BOX) {
                        //     lastCol++;
                        // }
                        while (map[curRow][lastCol] == BOX_LEFT) {
                            lastCol += 2;
                        }

                        if (map[curRow][lastCol] == WALL) {
                            continue;
                        }

                        while (lastCol > curCol) {
                            map[curRow][lastCol] = map[curRow][lastCol - 1];
                            map[curRow][lastCol - 1] = EMPTY;
                            lastCol--;
                        }
                        curCol++;
                    }

                    else if (direction == '^') {
                        Console.WriteLine("UP");

                        if (map[curRow - 1][curCol] == WALL) {
                            continue;
                        }

                        else if (map[curRow - 1][curCol] == EMPTY) {
                            map[curRow - 1][curCol] = ROBOT;
                            map[curRow][curCol] = EMPTY;
                            curRow--;
                        }

                        else if (map[curRow - 1][curCol] == BOX_RIGHT &&
                                CanMoveBoxes(map, curRow - 1, curCol - 1, -1)) {
                            MoveBoxes(map, curRow - 1, curCol - 1, -1);
                            map[curRow - 1][curCol] = ROBOT;
                            map[curRow][curCol] = EMPTY;
                            curRow--;
                        }

                        else if (map[curRow - 1][curCol] == BOX_LEFT &&
                                CanMoveBoxes(map, curRow - 1, curCol, -1)) {
                            MoveBoxes(map, curRow - 1, curCol, -1);
                            map[curRow - 1][curCol] = ROBOT;
                            map[curRow][curCol] = EMPTY;
                            curRow--;
                        }
                    }

                    else if (direction == 'v') {
                        Console.WriteLine("DOWN");

                        if (map[curRow + 1][curCol] == WALL) {
                            continue;
                        }

                        else if (map[curRow + 1][curCol] == EMPTY) {
                            map[curRow + 1][curCol] = ROBOT;
                            map[curRow][curCol] = EMPTY;
                            curRow++;
                        }

                        else if (map[curRow + 1][curCol] == BOX_RIGHT &&
                                CanMoveBoxes(map, curRow + 1, curCol - 1, 1)) {
                            MoveBoxes(map, curRow + 1, curCol - 1, 1);
                            map[curRow + 1][curCol] = ROBOT;
                            map[curRow][curCol] = EMPTY;
                            curRow++;
                        }

                        else if (map[curRow + 1][curCol] == BOX_LEFT &&
                                CanMoveBoxes(map, curRow + 1, curCol, 1)) {
                            MoveBoxes(map, curRow + 1, curCol, 1);
                            map[curRow + 1][curCol] = ROBOT;
                            map[curRow][curCol] = EMPTY;
                            curRow++;
                        }
                    }
                }
            }
            PrintMap(map);

            int sumCoordinates = 0;
            for (int i = 0; i < GRID_SIZE; i++) {
                // for (int j = 0; j < GRID_SIZE; j++) {
                for (int j = 0; j < GRID_SIZE * 2; j++) {
                    // if (map[i][j] == BOX) {
                    if (map[i][j] == BOX_LEFT) {
                        sumCoordinates += 100 * i + j;
                    }
                }
            }

            Console.WriteLine(sumCoordinates);
        }

        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    // Given the co-ordinates of the left side of a box and a direction,
    // check if the box can move in that direction
    // Recursively checks for all boxes on path
    private static bool CanMoveBoxes(List<List<int>> map, int boxRow, int boxCol, int direction) {
        if (map[boxRow + direction][boxCol] == EMPTY && map[boxRow + direction][boxCol + 1] == EMPTY) {
            return true;
        }

        else if (map[boxRow + direction][boxCol] == WALL || map[boxRow + direction][boxCol + 1] == WALL) {
            return false;
        }

        bool canMoveOthers = true;
        if (map[boxRow + direction][boxCol] == BOX_RIGHT) {
            canMoveOthers = canMoveOthers && CanMoveBoxes(map, boxRow + direction, boxCol - 1, direction);
        }
        if (canMoveOthers && map[boxRow + direction][boxCol] == BOX_LEFT) {
            canMoveOthers = canMoveOthers && CanMoveBoxes(map, boxRow + direction, boxCol, direction);
        }
        if (canMoveOthers && map[boxRow + direction][boxCol + 1] == BOX_LEFT) {
            canMoveOthers = canMoveOthers && CanMoveBoxes(map, boxRow + direction, boxCol + 1, direction);
        }

        return canMoveOthers;
    }

    // Move all boxes in a clump going up or down
    private static void MoveBoxes(List<List<int>> map, int boxRow, int boxCol, int direction) {
        if (map[boxRow + direction][boxCol] == EMPTY && map[boxRow + direction][boxCol + 1] == EMPTY) {
            map[boxRow + direction][boxCol] = BOX_LEFT;
            map[boxRow][boxCol] = EMPTY;
            map[boxRow + direction][boxCol + 1] = BOX_RIGHT;
            map[boxRow][boxCol + 1] = EMPTY;
            return;
        }

        // This should never happen
        else if (map[boxRow + direction][boxCol] == WALL || map[boxRow + direction][boxCol + 1] == WALL) {
            return;
        }

        if (map[boxRow + direction][boxCol] == BOX_RIGHT) {
            MoveBoxes(map, boxRow + direction, boxCol - 1, direction);
        }
        if (map[boxRow + direction][boxCol] == BOX_LEFT) {
            MoveBoxes(map, boxRow + direction, boxCol, direction);
        }
        if (map[boxRow + direction][boxCol + 1] == BOX_LEFT) {
            MoveBoxes(map, boxRow + direction, boxCol + 1, direction);
        }

        map[boxRow + direction][boxCol] = BOX_LEFT;
        map[boxRow][boxCol] = EMPTY;
        map[boxRow + direction][boxCol + 1] = BOX_RIGHT;
        map[boxRow][boxCol + 1] = EMPTY;
        return;
    }

    private static void PrintMap(List<List<int>> map) {
        foreach (List<int> row in map) {
            foreach (int n in row) {
                if (n == WALL) Console.Write('#');
                if (n == BOX) Console.Write('O');
                if (n == ROBOT) Console.Write('@');
                if (n == EMPTY) Console.Write('.');
                if (n == BOX_LEFT) Console.Write('[');
                if (n == BOX_RIGHT) Console.Write(']');
            }
            Console.WriteLine();
        }
    }
}
