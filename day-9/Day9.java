import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

public class Day9 {

    private static int FREE = -1;

    public static void main(String args[]) {
        try {
            File in = new File("day-9/day-9.in");
            Scanner reader = new Scanner(in);

            String data = reader.nextLine();
            List<Integer> disk = new ArrayList<>();

            boolean isFile = true;
            int fileNum = 0;
            for (char c : data.toCharArray()) {
                int curDigit = Character.getNumericValue(c);
                if (isFile) {
                    for (int i = 0; i < curDigit; i++) {
                        disk.add(fileNum);
                    }
                    fileNum++;
                } else {
                    for (int i = 0; i < curDigit; i++) {
                        disk.add(FREE);
                    }
                }
                isFile = !isFile;
            }

            int left = 0;
            int right = disk.size() - 1;
            // left starts at first FREE
            while (disk.get(left) != FREE) left++;
            // right starts at end of last file block
            while (disk.get(right) == FREE) right--;

            while (left < right) {
                /* Part 1 */
                // int temp = disk.get(left);
                // disk.set(left, disk.get(right));
                // disk.set(right, temp);

                /* Part 2 */
                int blockSize = getFileBlockSize(disk, right);
                int i = left;
                boolean foundSpace = false;
                while (i < right) {
                    int freeSpace = getFreeSpaceSize(disk, i);
                    if (freeSpace >= blockSize) {
                        for (int j = 0; j < blockSize; j++) {
                            int temp = disk.get(i + j);
                            disk.set(i + j, disk.get(right));
                            disk.set(right, temp);
                            right--;
                        }
                        foundSpace = true;
                        break;
                    }

                    i++;
                }
                // If we didn't find the space to move the file, skip it
                if (!foundSpace) {
                    right -= blockSize;
                }

                while (disk.get(left) != FREE) left++;
                while (disk.get(right) == FREE) right--;
            }

            // Calculate checksum
            long checksum = 0;
            for (int i = 0; i < disk.size(); i++) {
                if (disk.get(i) == FREE) continue;
                checksum += i * disk.get(i);
            }

            System.out.println(checksum);
            reader.close();
        } catch (FileNotFoundException e) {
            System.out.println("Could not open file");
            e.printStackTrace();
        }
    }

    private static int getFileBlockSize(List<Integer> disk, int right) {
        int fileID = disk.get(right);
        int position = right;
        int blockSize = 0;
        while (disk.get(position) == fileID) {
            blockSize++;
            position--;
        }

        return blockSize;
    }

    private static int getFreeSpaceSize(List<Integer> disk, int left) {
        int position = left;
        int spaceSize = 0;
        while (disk.get(position) == FREE) {
            spaceSize++;
            position++;
        }

        return spaceSize;
    }
}
