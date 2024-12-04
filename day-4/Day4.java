import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;
import java.lang.StringBuilder;
import java.util.List;
import java.util.ArrayList;
import java.util.Arrays;

public class Day4 {

    public static void main(String args[]) {
        try {
            File in = new File("day-4/day-4.in");
            Scanner reader = new Scanner(in);

            List<String> lines = new ArrayList<String>();
            while (reader.hasNextLine()) {
                String line = reader.nextLine();
                lines.add(line);
            }

            int rows = lines.size();
            int cols = lines.get(0).length();

            int xmasCount = 0;
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    /* Part 1 */
                    // if (j < cols - 3) {
                    //     // Horizontal
                    //     String horizontalSubstring = lines.get(i).substring(j, j + 4);
                    //     if (horizontalSubstring.equals("XMAS") ||
                    //         horizontalSubstring.equals("SAMX")) {
                    //         xmasCount++;
                    //     }

                    //     // Diagonal right down
                    //     if (i < rows - 3) {
                    //         StringBuilder str = new StringBuilder();
                    //         str.append(lines.get(i).charAt(j));
                    //         str.append(lines.get(i + 1).charAt(j + 1));
                    //         str.append(lines.get(i + 2).charAt(j + 2));
                    //         str.append(lines.get(i + 3).charAt(j + 3));

                    //         String word = str.toString();
                    //         if (word.equals("XMAS") || word.equals("SAMX")) {
                    //             xmasCount++;
                    //         }
                    //     }

                    //     // Diagonal right up
                    //     if (i > 2) {
                    //         StringBuilder str = new StringBuilder();
                    //         str.append(lines.get(i).charAt(j));
                    //         str.append(lines.get(i - 1).charAt(j + 1));
                    //         str.append(lines.get(i - 2).charAt(j + 2));
                    //         str.append(lines.get(i - 3).charAt(j + 3));

                    //         String word = str.toString();
                    //         if (word.equals("XMAS") || word.equals("SAMX")) {
                    //             xmasCount++;
                    //         }
                    //     }
                    // }

                    // Vertical
                    // if (i < rows - 3) {
                    //     StringBuilder str = new StringBuilder();
                    //     str.append(lines.get(i).charAt(j));
                    //     str.append(lines.get(i + 1).charAt(j));
                    //     str.append(lines.get(i + 2).charAt(j));
                    //     str.append(lines.get(i + 3).charAt(j));

                    //     String word = str.toString();
                    //     if (word.equals("XMAS") || word.equals("SAMX")) {
                    //         xmasCount++;
                    //     }
                    // }

                    /* Part 2 */
                    // Positions of possible Ms and their corresponding As
                    List<List<Integer>> mPos = Arrays.asList(
                        Arrays.asList(0, 0),
                        Arrays.asList(0, 2),
                        Arrays.asList(2, 0),
                        Arrays.asList(2, 2)
                    );
                    List<List<Integer>> aPos = Arrays.asList(
                        Arrays.asList(2, 2),
                        Arrays.asList(2, 0),
                        Arrays.asList(0, 2),
                        Arrays.asList(0, 0)
                    );

                    if (i < rows - 2 && j < cols - 2) {
                        if (lines.get(i + 1).charAt(j + 1) != 'A') continue;

                        for (int first = 0; first < 4; first++) {
                            for (int second = first + 1; second < 4; second++) {
                                // this is so ugly :/
                                if (lines.get(i + mPos.get(first).get(0)).charAt(j + mPos.get(first).get(1)) == 'M' &&
                                    lines.get(i + mPos.get(second).get(0)).charAt(j + mPos.get(second).get(1)) == 'M' &&
                                    lines.get(i + aPos.get(first).get(0)).charAt(j + aPos.get(first).get(1)) == 'S' &&
                                    lines.get(i + aPos.get(second).get(0)).charAt(j + aPos.get(second).get(1)) == 'S'
                                ) {
                                    xmasCount++;
                                }
                            }
                        }
                    }

                }
            }

            System.out.println(xmasCount);
            reader.close();

        } catch (FileNotFoundException e) {
            System.out.println("Unable to find file");
            e.printStackTrace();
        }
    }
}
