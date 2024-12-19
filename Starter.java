import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

public class Starter {

    public static void main(String args[]) {
        try {
            File in = new File("day-x/day-x.in");
            Scanner reader = new Scanner(in);

            reader.close();
        } catch (FileNotFoundException e) {
            System.out.println("Could not open file");
            e.printStackTrace();
        }
    }
}
