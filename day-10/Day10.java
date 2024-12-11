import java.io.File;
import java.io.FileNotFoundException;
import java.util.Arrays;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Scanner;
import java.util.Set;


public class Day10 {

    private static int SIZE = 55;

    public static void main(String args[]) {
        try {
            File in = new File("day-10/day-10.in");
            Scanner reader = new Scanner(in);

            List<List<Integer>> map = new ArrayList<>();
            while (reader.hasNextLine()) {
                String line = reader.nextLine();
                List<Integer> curRow = new ArrayList<>();

                for (char c : line.toCharArray()) {
                    curRow.add(c - '0');
                }
                map.add(curRow);
            }

            int totalScores = 0;
            int totalRating = 0;
            for (int i = 0; i < SIZE; i++) {
                for (int j = 0; j < SIZE; j++) {
                    if (map.get(i).get(j) == 0) {
                        /* Part 1 */
                        totalScores += dfsScores(map, i, j).size();
                        /* Part 2 */
                        totalRating += dfsRating(map, i, j);
                    }
                }
            }

            System.out.println("Total scores: " + totalScores);
            System.out.println("Total rating: " + totalRating);
            reader.close();
        } catch (FileNotFoundException e) {
            System.out.println("Could not open file");
            e.printStackTrace();
        }
    }

    /* Part 1 */
    // Starts a DFS from the current row and column
    // Returns a set containing all reachable 9s
    private static Set<List<Integer>> dfsScores(List<List<Integer>> map, int row, int col) {
        int curHeight = map.get(row).get(col);
        Set<List<Integer>> returnSet = new HashSet<>();
        // Reached top of trail
        if (curHeight == 9) {
            returnSet.add(new ArrayList<Integer>(Arrays.asList(row, col)));
            return returnSet;
        }

        int[][] directions = {{0, -1}, {1, 0}, {0, 1}, {-1, 0}};
        for (int[] direction : directions) {
            int newRow = row + direction[0];
            int newCol = col + direction[1];

            if (newRow >= 0 && newRow < SIZE && newCol >= 0 && newCol < SIZE &&
                map.get(newRow).get(newCol) == curHeight + 1) {
                Set<List<Integer>> peaks = dfsScores(map, newRow, newCol);
                returnSet.addAll(peaks);
            }
        }

        return returnSet;
    }

    /* Part 2 */
    // Starts a DFS from the current row and column
    // Returns the number of possible ways to reach a 9
    private static int dfsRating(List<List<Integer>> map, int row, int col) {
        int curHeight = map.get(row).get(col);
        // Reached top of trail
        if (curHeight == 9) return 1;

        int[][] directions = {{0, -1}, {1, 0}, {0, 1}, {-1, 0}};
        int ratings = 0;
        for (int[] direction : directions) {
            int newRow = row + direction[0];
            int newCol = col + direction[1];

            if (newRow >= 0 && newRow < SIZE && newCol >= 0 && newCol < SIZE &&
                map.get(newRow).get(newCol) == curHeight + 1) {
                ratings += dfsRating(map, newRow, newCol);
            }
        }

        return ratings;
    }
}
