import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;
import java.util.regex.*;

public class Day3 {

    public static void main(String args[]) {
        try {
            File in = new File("day-3/day-3.in");
            Scanner reader = new Scanner(in);

            int totalSum = 0;
            boolean enabled = true;
            while (reader.hasNextLine()) {
                String line = reader.nextLine();

                /* Part 1 */
                // Pattern p = Pattern.compile("(mul\\(\\d{1,3},\\d{1,3}\\))");
                // Matcher m = p.matcher(line);

                // while (m.find()) {
                //     String instruction = m.group(1);
                //     totalSum += parseInstruction(instruction);
                // }

                /* Part 2 */
                Pattern p = Pattern.compile("(mul\\(\\d{1,3},\\d{1,3}\\)|do\\(\\)|don\'t\\(\\))");
                Matcher m = p.matcher(line);

                while (m.find()) {
                    String instruction = m.group(1);
                    if (instruction.equals("do()")) {
                        enabled = true;
                    }
                    else if (instruction.equals("don't()")) {
                        enabled = false;
                    }
                    else if (enabled) {
                        totalSum += parseInstruction(instruction);
                    }
                }
            }

            System.out.println(totalSum);
            reader.close();
        } catch (FileNotFoundException e) {
            System.out.println("File not found");
            e.printStackTrace();
        }
    }


    private static int parseInstruction(String instruction) {
        String[] instrParts = instruction.split(",");
        int firstNumber = Integer.parseInt(instrParts[0].substring(4));
        int secondNumber = Integer.parseInt(instrParts[1].substring(0, instrParts[1].length() - 1));

        return firstNumber * secondNumber;
    }
}
