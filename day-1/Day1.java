import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Scanner;
import java.util.HashMap;

class Day1 {

    public static void main(String args[]) {
        try {
            File myObj = new File("day-1/day-1.in");
            Scanner myReader = new Scanner(myObj);

            List<Integer> firstColumn = new ArrayList<Integer>();
            List<Integer> secondColumn = new ArrayList<Integer>();
            Map<Integer, Integer> secondColumnFreq = new HashMap<>();

            while (myReader.hasNextLine()) {
                String data = myReader.nextLine();

                String firstNumberStr = data.substring(0, 5);
                int firstNumber = Integer.parseInt(firstNumberStr);
                String secondNumberStr = data.substring(8);
                int secondNumber = Integer.parseInt(secondNumberStr);
                if (secondColumnFreq.containsKey(secondNumber)) {
                    secondColumnFreq.put(secondNumber, secondColumnFreq.get(secondNumber) + 1);
                } else {
                    secondColumnFreq.put(secondNumber, 1);
                }

                firstColumn.add(firstNumber);
                secondColumn.add(secondNumber);
            }

            firstColumn.sort(null);
            secondColumn.sort(null);

            int totalDistance = 0;
            for (int i = 0; i < firstColumn.size(); i++) {
                totalDistance += Math.abs(firstColumn.get(i) - secondColumn.get(i));
            }

            System.out.println(totalDistance);

            // Part 2
            int totalSimilarity = 0;
            for (int i = 0; i < firstColumn.size(); i++) {
                int number = firstColumn.get(i);
                if (secondColumnFreq.containsKey(number)) {
                    totalSimilarity += number * secondColumnFreq.get(number);
                }
            }

            System.out.println(totalSimilarity);

            myReader.close();

        } catch(FileNotFoundException e) {
            System.out.println("Could not find file.");
            e.printStackTrace();
        }
    }
}