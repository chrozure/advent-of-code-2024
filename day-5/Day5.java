import java.io.File;
import java.io.FileNotFoundException;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Scanner;
import java.util.Set;

public class Day5 {

    public static void main(String args[]) {
        try {
            File in = new File("day-5/day-5.in");
            Scanner reader = new Scanner(in);

            // Read in all the rules and put them into a map
            Map<Integer, Set<Integer>> rules = new HashMap<>();
            while (reader.hasNextLine()) {
                String rule = reader.nextLine();
                if (rule.isEmpty()) break;

                String[] numbers = rule.split("\\|");

                int firstNum = Integer.parseInt(numbers[0]);
                int secondNum = Integer.parseInt(numbers[1]);
                Set<Integer> newRule;
                if (!rules.containsKey(firstNum)) {
                    newRule = new HashSet<>();
                } else {
                    newRule = rules.get(firstNum);
                }
                newRule.add(secondNum);
                rules.put(firstNum, newRule);
            }

            // Go through pages and check ordering
            int middlePageSum = 0;
            while (reader.hasNextLine()) {
                String update = reader.nextLine();
                boolean validUpdate = true;

                String[] pages = update.split(",");
                for (int i = 0; i < pages.length; i++) {
                    for (int j = i + 1; j < pages.length; j++) {
                        int firstNum = Integer.parseInt(pages[i]);
                        int secondNum = Integer.parseInt(pages[j]);

                        // If the second number should appear before the first,
                        // the update is not valid
                        if (rules.get(secondNum).contains(firstNum)) {
                            validUpdate = false;
                            // break;

                            /* Part 2 */
                            swapPages(pages, i, j);
                        }
                    }
                }

                /* Part 1 */
                // if (validUpdate) {
                //     middlePageSum += Integer.parseInt(pages[pages.length / 2]);
                // }

                /* Part 2 */
                if (!validUpdate) {
                    middlePageSum += Integer.parseInt(pages[pages.length / 2]);
                }
            }

            System.out.println(middlePageSum);
            reader.close();
        } catch (FileNotFoundException e) {
            System.out.println("Could not open file");
            e.printStackTrace();
        }
    }

    private static void swapPages(String[] pages, int i, int j) {
        String temp = pages[i];
        pages[i] = pages[j];
        pages[j] = temp;
    }
}
