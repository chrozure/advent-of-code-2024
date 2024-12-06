import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;


public class Day6 {

    private final static int UNVISITED = 0;
    private final static int VISITED = 1;
    private final static int WALL = 2;

    private final static int UP = 0;
    private final static int RIGHT = 1;
    private final static int DOWN = 2;
    private final static int LEFT = 3;

    /* Part 2 */
    private final static int OBSTRUCTED = -1;

    public static void main(String args[]) {
        try {
            File in = new File("day-6/day-6.in");
            Scanner reader = new Scanner(in);

            int[][] grid = new int[130][130];

            int row = 0;
            int startRow = 0;
            int startCol = 0;
            while (reader.hasNextLine()) {
                String line = reader.nextLine();

                for (int col = 0; col < line.length(); col++) {
                    if (line.charAt(col) == '#') {
                        grid[row][col] = WALL;
                    } else if (line.charAt(col) == '^') {
                        startRow = row;
                        startCol = col;
                    }
                }
                row++;
            }

            /* Part 1 */
            int numVisited = patrol(grid, startRow, startCol);
            System.out.println(numVisited);

            /* Part 2 */
            int numPossibleBlocks = 0;
            for (int i = 0; i < 130; i++) {
                for (int j = 0; j < 130; j++) {
                    // Don't try to obstruct if there is already a wall
                    // or it is the guard's starting position
                    if (grid[i][j] == WALL || (i == startRow && j == startCol)) continue;

                    grid[i][j] = WALL;
                    if (patrol(grid, startRow, startCol) == OBSTRUCTED) {
                        numPossibleBlocks++;
                    }
                    grid[i][j] = UNVISITED;
                }
            }

            System.out.println(numPossibleBlocks);
            reader.close();
        } catch (FileNotFoundException e) {
            System.out.println("Could not open file");
            e.printStackTrace();
        }
    }

    // Makes the guard do its full patrol
    // If it exits the map, returns the number of visited places
    // If it gets stuck in a loop, returns OBSTRUCTED
    private static int patrol(int[][] grid, int startRow, int startCol) {
        int direction = UP;
        int numVisited = 0;
        int visited[][] = new int[130][130];
        int curRow = startRow;
        int curCol = startCol;
        while (curRow >= 0 && curCol >= 0 && curRow < 130 && curCol < 130) {
            if (grid[curRow][curCol] == UNVISITED) {
                numVisited++;
            }
            grid[curRow][curCol] = VISITED;
            visited[curRow][curCol]++;

            /* Specifically for part 2 */
            if (visited[curRow][curCol] > 4) {
                return OBSTRUCTED;
            }

            if (direction == UP && (curRow == 0 || grid[curRow - 1][curCol] != WALL)) {
                curRow--;
            } else if (direction == RIGHT && (curCol == 129 || grid[curRow][curCol + 1] != WALL)) {
                curCol++;
            } else if (direction == DOWN && (curRow == 129 || grid[curRow + 1][curCol] != WALL)) {
                curRow++;
            } else if (direction == LEFT && (curCol == 0 || grid[curRow][curCol - 1] != WALL)) {
                curCol--;
            } else {
                direction = turnRight(direction);
            }
        }

        return numVisited;
    }

    private static int turnRight(int direction) {
        switch (direction) {
            case UP: return RIGHT;
            case RIGHT: return DOWN;
            case DOWN: return LEFT;
            default: return UP;
        }
    }
}
