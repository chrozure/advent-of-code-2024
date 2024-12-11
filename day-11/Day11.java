import java.io.File;
import java.io.FileNotFoundException;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

public class Day11 {

    /* Part 1 */
    // private static int NUM_BLINKS = 25;

    /* Part 2 */
    private static int NUM_BLINKS = 75;

    public static void main(String args[]) {
        try {
            File in = new File("day-11/day-11.in");
            Scanner reader = new Scanner(in);
            String data = reader.nextLine();
            String[] stonesStr = data.split(" ");

            // Stone number -> frequency mapping
            Map<Long, Long> stones = new HashMap<>();
            for (String stone : stonesStr) {
                stones.put(Long.parseLong(stone), 1l);
            }

            for (int blink = 0; blink < NUM_BLINKS; blink++) {
                Map<Long, Long> newStones = new HashMap<>();
                for (Map.Entry<Long, Long> entry : stones.entrySet()) {
                    Long stone = entry.getKey();
                    Long freq = entry.getValue();

                    // Stone has number 0
                    if (stone == 0) {
                        newStones.put(1l, freq + newStones.getOrDefault(1l, 0l));
                    }
                    // Stone has number with even number of digits
                    else if (String.valueOf(stone).length() % 2 == 0) {
                        String stoneStr = String.valueOf(stone);
                        int middleOfStone = stoneStr.length() / 2;
                        long firstHalf = Long.parseLong(stoneStr.substring(0, middleOfStone));
                        long secondHalf = Long.parseLong(stoneStr.substring(middleOfStone));

                        newStones.put(firstHalf, freq + newStones.getOrDefault(firstHalf, 0l));
                        newStones.put(secondHalf, freq + newStones.getOrDefault(secondHalf, 0l));
                    }
                    // None of the other rules apply
                    else {
                        long newVal = stone * 2024;
                        newStones.put(newVal, freq + newStones.getOrDefault(newVal, 0l));
                    }
                }

                stones = newStones;
            }

            Long totalStones = 0l;
            for (Map.Entry<Long, Long> entry : stones.entrySet()) {
                totalStones += entry.getValue();
            }
            System.out.println(totalStones);

            reader.close();
        } catch (FileNotFoundException e) {
            System.out.println("Could not open file");
            e.printStackTrace();
        }
    }
}
