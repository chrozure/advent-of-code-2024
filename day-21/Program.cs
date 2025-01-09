using System;
using System.IO;
using System.Collections.Generic;


class Program
{

    private readonly static int NUMERIC = 0;
    private readonly static int DIRECTION = 1;

    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new("./day-21.in");

            List<List<char>> numericKeypad = [
                ['7', '8', '9'],
                ['4', '5', '6'],
                ['1', '2', '3'],
                [' ', '0', 'A']
            ];
            List<List<char>> directionalKeypad = [
                [' ', '^', 'A'],
                ['<', 'v', '>']
            ];

            int numericalRow = 3;
            int numericalCol = 2;
            int directionalRow = 0;
            int directionalCol = 2;

            int totalComplexity = 0;

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
                // First 3 characters are number, 4th character is A
                int numericPart = int.Parse(line.Substring(0, 3));

                // numericButton = '0', line = '029A'
                List<char> directionalMoves = [];
                foreach (char numericButton in line)
                {
                    List<char> numericalMoves = BFS(numericKeypad, NUMERIC, numericalRow, numericalCol, numericButton);
                    foreach (char directionButton in numericalMoves)
                    {
                        // First find the path to the desired number
                        List<char> directionalMovesNumber = BFS(directionalKeypad, DIRECTION, directionalRow, directionalCol, directionButton);
                        directionalMovesNumber.Add('A');
                        directionalMoves.AddRange(directionalMovesNumber);

                        // Use the directionalMap to go to the number
                        foreach (char directionalMove in directionalMovesNumber)
                        {
                            if (directionalMove == '>') directionalCol++;
                            if (directionalMove == '<') directionalCol--;
                            if (directionalMove == 'v') directionalRow++;
                            if (directionalMove == '^') directionalRow--;
                        }

                        if (directionButton == '>') numericalCol++;
                        if (directionButton == '<') numericalCol--;
                        if (directionButton == 'v') numericalRow++;
                        if (directionButton == '^') numericalRow--;
                    }

                    // Now go to the A button to press it
                    List<char> directionalMovesA = BFS(directionalKeypad, DIRECTION, directionalRow, directionalCol, 'A');
                    directionalMovesA.Add('A');
                    directionalMoves.AddRange(directionalMovesA);
                    foreach (char directionalMove in directionalMovesA)
                    {
                        if (directionalMove == '>') directionalCol++;
                        if (directionalMove == '<') directionalCol--;
                        if (directionalMove == 'v') directionalRow++;
                        if (directionalMove == '^') directionalRow--;
                    }
                }

                int sequenceLength = 0;
                directionalRow = 0;
                directionalCol = 2;
                List<char> thing = [];
                foreach (char c in directionalMoves)
                {
                    List<char> directionalMovesDirection = BFS(directionalKeypad, DIRECTION, directionalRow, directionalCol, c);
                    directionalMovesDirection.Add('A');
                    thing.AddRange(directionalMovesDirection);

                    foreach (char c2 in directionalMovesDirection)
                    {
                        if (c2 == '>') directionalCol++;
                        if (c2 == '<') directionalCol--;
                        if (c2 == 'v') directionalRow++;
                        if (c2 == '^') directionalRow--;
                    }
                }

                Console.WriteLine(thing.Count);
                foreach (char c2 in thing)
                {
                    Console.Write(c2);
                }
                Console.WriteLine();

                sequenceLength = thing.Count;
                totalComplexity += sequenceLength * numericPart;
            }

            // 104464 too low
            // 136780 correct
            // 137836 wrong
            // 138892 wrong
            // 141004 wrong
            // 144944 too high
            // 145648 too high
            Console.WriteLine("Part 1: " + totalComplexity);
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }


    private static List<char> BFS(List<List<char>> map, int type, int startRow, int startCol, char target)
    {
        if (map[startRow][startCol] == '3' && target == '7')
        {
            return ['<', '<', '^', '^'];
        }
        if (map[startRow][startCol]  == '7' && target == '6')
        {
            return ['v', '>', '>'];
        }
        if (map[startRow][startCol]  == '5' && target == 'A')
        {
            return ['v', 'v', '>'];
        }
        if (map[startRow][startCol]  == '8' && target == 'A')
        {
            return ['v', 'v', 'v', '>'];
        }
        if (map[startRow][startCol]  == 'A' && target == '5')
        {
            return ['<', '^', '^'];
        }


        List<char> moves = [];
        int numRows = map.Count;
        int numCols = map[0].Count;

        List<List<bool>> visited = [];
        List<List<(int, int)>> pred = [];
        for (int i = 0; i < numRows; i++) {
            visited.Add( Enumerable.Repeat(false, numCols).ToList() );
            pred.Add( Enumerable.Repeat((-1, -1), numCols).ToList() );
        }

        Queue<(int, int)> q = new([(startRow, startCol)]);
        (int endRow, int endCol) = (-1, -1);
        while (q.Count > 0)
        {
            (int row, int col) = q.Dequeue();
            if (map[row][col] == target)
            {
                (endRow, endCol) = (row, col);
                break;
            }

            (int, int)[] directions = [];
            if (type == NUMERIC)
            {
                // prioritise going up and right
                directions = [(-1, 0), (0, 1), (0, -1), (1, 0), ];
            }
            else if (type == DIRECTION)
            {
                // prioritise going down and right
                directions = [(1, 0), (0, 1), (-1, 0), (0, -1)];
            }
            foreach ((int rowDir, int colDir) in directions)
            {
                int newRow = row + rowDir;
                int newCol = col + colDir;

                if (ValidPosition(newRow, newCol, numRows, numCols) && map[newRow][newCol] != ' ' && !visited[newRow][newCol])
                {
                    visited[newRow][newCol] = true;
                    pred[newRow][newCol] = (row, col);
                    q.Enqueue((newRow, newCol));
                }
            }
        }

        while ((endRow, endCol) != (startRow, startCol))
        {
            (int predRow, int predCol) = pred[endRow][endCol];
            if (predRow == endRow - 1) moves.Add('v');
            if (predRow == endRow + 1) moves.Add('^');
            if (predCol == endCol - 1) moves.Add('>');
            if (predCol == endCol + 1) moves.Add('<');

            (endRow, endCol) = (predRow, predCol);
        }

        moves.Reverse();
        if (type == NUMERIC)
        {
            Console.WriteLine(map[startRow][startCol] + " -> " + target);
            moves.ForEach(Console.Write);
            Console.WriteLine();
        }
        return moves;
    }

    private static bool ValidPosition(int row, int col, int numRows, int numCols)
    {
        return row >= 0 && col >= 0 && row < numRows && col < numCols;
    }
}
