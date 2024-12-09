import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Scanner;
import java.util.Set;

import javafx.util.Pair;

public class Day8 {

    private static int SIZE = 50;

    public static void main(String args[]) {
        try {
            File in = new File("src/day-8.in");
            Scanner reader = new Scanner(in);

            Set<Pair<Integer, Integer>> antinodes = new HashSet<>();
            Map<Character, List<Pair<Integer, Integer>>> antennas = new HashMap<>();
            int row = 0;
            while (reader.hasNextLine()) {
                String line = reader.nextLine();
                for (int col = 0, n = line.length(); col < n; col++) {
                    char c = line.charAt(col);
                    if (c != '.') {
                        Pair<Integer, Integer> position = new Pair<>(row, col);
                        // Map doesn't contain c, create new list
                        if (!antennas.containsKey(c)) {
                            List<Pair<Integer, Integer>> newList = new ArrayList<>();
                            newList.add(position);
                            antennas.put(c, newList);
                        }
                        // Map contains c, find all possible antinodes
                        else {
                            List<Pair<Integer, Integer>> otherAntennas = antennas.get(c);
                            for (Pair<Integer, Integer> otherAntenna : otherAntennas) {
                                int otherAntennaRow = otherAntenna.getKey();
                                int otherAntennaCol = otherAntenna.getValue();
                                int dRow = otherAntennaRow - row;
                                int dCol = otherAntennaCol - col;

                                /* Part 1 */
                                // int newRow1 = otherAntennaRow + dRow;
                                // int newCol1 = otherAntennaCol + dCol;
                                // int newRow2 = row - dRow;
                                // int newCol2 = col - dCol;

                                // if (newRow1 >= 0 && newRow1 < SIZE && newCol1 >= 0 && newCol1 < SIZE) {
                                //     antinodes.add(new Pair<Integer, Integer>(newRow1, newCol1));
                                // }
                                // if (newRow2 >= 0 && newRow2 < SIZE && newCol2 >= 0 && newCol2 < SIZE) {
                                //     antinodes.add(new Pair<Integer, Integer>(newRow2, newCol2));
                                // }

                                /* Part 2 */
                                int newRow = otherAntennaRow;
                                int newCol = otherAntennaCol;
                                while (newRow >= 0 && newRow < SIZE && newCol >= 0 && newCol < SIZE) {
                                    antinodes.add(new Pair<Integer, Integer>(newRow, newCol));
                                    newRow += dRow;
                                    newCol += dCol;
                                }

                                newRow = row;
                                newCol = col;
                                while (newRow >= 0 && newRow < SIZE && newCol >= 0 && newCol < SIZE) {
                                    antinodes.add(new Pair<Integer, Integer>(newRow, newCol));
                                    newRow -= dRow;
                                    newCol -= dCol;
                                }
                            }

                            otherAntennas.add(position);
                            antennas.put(c, otherAntennas);
                        }
                    }
                }
                row++;
            }
            System.out.println(antennas);
            System.out.println(antinodes.size());
            reader.close();
        } catch (FileNotFoundException e) {
            System.out.println("Could not read file");
            e.printStackTrace();
        }
    }
}
