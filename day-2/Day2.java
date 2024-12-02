import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;


public class Day2 {

    public static void main(String args[]) {
        try {
            File input = new File("day-2/day-2.in");
            Scanner reader = new Scanner(input);

            int numSafeReports = 0;
            while (reader.hasNextLine()) {
                String report = reader.nextLine();
                String[] levelsStr = report.split(" ");

                List<Integer> levels = new ArrayList<>();
                for (int i = 0; i < levelsStr.length; i++) {
                    levels.add(Integer.parseInt(levelsStr[i]));
                }

                // Part 1
                // if (isSafe(levels)) {
                //     numSafeReports++;
                // }

                // Part 2
                for (int skip = -1; skip < levels.size(); skip++) {
                    List<Integer> newList = new ArrayList<>();
                    for (int i = 0; i < levels.size(); i++) {
                        if (i == skip) continue;
                        newList.add(levels.get(i));
                    }

                    if (isSafe(newList)) {
                        numSafeReports++;
                        break;
                    }
                }
            }

            System.out.println(numSafeReports);
            reader.close();

        } catch (FileNotFoundException e) {
            System.out.println("Unable to find file");
            e.printStackTrace();
        }
    }

    private static boolean isSafe(List<Integer> levels) {
        boolean ascending = levels.get(1) > levels.get(0);

        boolean safe = true;
        for (int i = 0; i < levels.size() - 1; i++) {
            int diff = levels.get(i + 1) - levels.get(i);
            if (ascending && diff <= 0 || diff > 3) {
                safe = false;
            }
            else if (!ascending && diff >= 0 || diff < -3) {
                safe = false;
            }
        }

        return safe;
    }
}
