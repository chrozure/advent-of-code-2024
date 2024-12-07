import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class Day7 {

    public static void main(String args[]) {
        try {
            File in = new File("day-7/day-7.in");
            Scanner reader = new Scanner(in);

            long totalCalibration = 0;

            while (reader.hasNextLine()) {
                String data = reader.nextLine();
                String[] splitData = data.split(":");

                long desiredTotal = Long.parseLong(splitData[0]);

                String[] numbersStr = splitData[1].split(" ");
                List<Long> numbers = new ArrayList<>();
                for (int i = 1; i < numbersStr.length; i++) {
                    numbers.add(Long.parseLong(numbersStr[i]));
                }

                if (compute(desiredTotal, 0, numbers)) {
                    totalCalibration += desiredTotal;
                }
            }

            System.out.println(totalCalibration);
            reader.close();
        } catch (FileNotFoundException e) {
            System.out.println("Could not open file");
            e.printStackTrace();
        }
    }

    private static boolean compute(long desiredTotal, long totalSoFar, List<Long> numbersLeft) {
        if (numbersLeft.isEmpty()) {
            return desiredTotal == totalSoFar;
        }

        return (
            compute(desiredTotal, totalSoFar + numbersLeft.get(0), getRestOfNumbers(numbersLeft)) ||
            compute(desiredTotal, totalSoFar * numbersLeft.get(0), getRestOfNumbers(numbersLeft)) ||
            /* Part 2 */
            compute(desiredTotal, Long.parseLong(totalSoFar + "" + numbersLeft.get(0)), getRestOfNumbers(numbersLeft))
        );
    }

    private static List<Long> getRestOfNumbers(List<Long> numbers) {
        List<Long> restOfNumbers = new ArrayList<>();
        for (int i = 1; i < numbers.size(); i++) {
            restOfNumbers.add(numbers.get(i));
        }

        return restOfNumbers;
    }
}
