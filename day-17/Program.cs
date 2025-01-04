using System;
using System.IO;
using System.Collections.Generic;


class Program
{

    static void Main(string[] args)
    {
        try
        {
            long registerA = 35200350;
            long registerB = 0;
            long registerC = 0;

            List<int> program = [2,4,1,2,7,5,4,7,1,3,5,5,0,3,3,0];
            List<int> output = RunProgram(registerA, registerB, registerC, program);

            Console.WriteLine("Part 1: " +string.Join(",", output.Select(n => n.ToString()).ToArray()));

            /* Part 2 */
            // Essentially reverse engineers the possible A values using a BFS
            // The length of the output is given by about log_8(registerA)
            // It is the number of times you can divide registerA by 8
            // So we start at the last digit and find all possible numbers that can
            // give the digit before, and then all the possible numbers that give the digit before that
            Queue<long> q = new([1]);
            for (int digit = 14; digit >= 0; digit--)
            {
                int qSize = q.Count;
                for (int i = 0; i < qSize; i++)
                {
                    long nextA = q.Dequeue();

                    for (registerA = nextA * 8; registerA < (nextA + 1) * 8; registerA++)
                    {
                        // Run through the program a single time
                        registerB = (registerA % 8) ^ 2;

                        int power = 1;
                        for (int j = 0; j < registerB; j++) power *= 2;
                        registerC = registerA / power;

                        registerB ^= registerC ^ 3;
                        int res = (int) (registerB % 8);
                        if (res == program[digit])
                        {
                            q.Enqueue(registerA);
                        }
                    }
                }
            }

            Console.WriteLine("Part 2: " + q.Peek());
            // 35184372088832 lower bound
            // 37221274271220 answer
            // 281474976710656 upper bound
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

    // Part 1
    private static List<int> RunProgram(long registerA, long registerB, long registerC, List<int> program) {
        List<int> output = [];
        for (int instructionPointer = 0; instructionPointer < program.Count; instructionPointer += 2) {
            int opcode = program[instructionPointer];
            int operand = program[instructionPointer + 1];

            // Console.WriteLine("A: " + registerA + ", B: " + registerB + ", C: " + registerC);
            switch(opcode)
            {
                case 0:
                    ADV(operand, ref registerA, registerB, registerC);
                    break;
                case 1:
                    BXL(operand, ref registerB);
                    break;
                case 2:
                    BST(operand, registerA, ref registerB, registerC);
                    break;
                case 3:
                    JNZ(operand, registerA, ref instructionPointer);
                    break;
                case 4:
                    BXC(ref registerB, registerC);
                    break;
                case 5:
                    OUT(operand, registerA, registerB, registerC, output);
                    break;
                case 6:
                    BDV(operand, registerA, ref registerB, registerC);
                    break;
                case 7:
                    CDV(operand, registerA, registerB, ref registerC);
                    break;
                default:
                    break;
            }
        }

        return output;
    }

    private static long Combo(int operand, long registerA, long registerB, long registerC) {
        switch (operand)
        {
            case 0: return 0;
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            case 4: return registerA;
            case 5: return registerB;
            case 6: return registerC;
            default:
                break;
        }

        return 0;
    }

    private static void ADV(int operand, ref long registerA, long registerB, long registerC) {
        long numerator = registerA;
        long comboOperand = Combo(operand, registerA, registerB, registerC);
        int denominator = 1;
        for (int i = 0; i < comboOperand; i++) {
            denominator *= 2;
        }

        registerA = numerator / denominator;
    }

    private static void BXL(int operand, ref long registerB) {
        registerB ^= operand;
    }

    private static void BST(int operand, long registerA, ref long registerB, long registerC) {
        long comboOperand = Combo(operand, registerA, registerB, registerC);

        registerB = comboOperand % 8;
    }

    private static void JNZ(int operand, long registerA, ref int instructionPointer) {
        if (registerA == 0) return;

        instructionPointer = operand - 2;
    }

    private static void BXC(ref long registerB, long registerC) {
        registerB ^= registerC;
    }

    private static void OUT(int operand, long registerA, long registerB, long registerC, List<int> output) {
        long comboOperand = Combo(operand, registerA, registerB, registerC);
        output.Add((int) (comboOperand % 8));
    }

    private static void BDV(int operand, long registerA, ref long registerB, long registerC) {
        long numerator = registerA;
        long comboOperand = Combo(operand, registerA, registerB, registerC);
        int denominator = 1;
        for (int i = 0; i < comboOperand; i++) {
            denominator *= 2;
        }

        registerB = numerator / denominator;
    }

    private static void CDV(int operand, long registerA, long registerB, ref long registerC) {
        long numerator = registerA;
        long comboOperand = Combo(operand, registerA, registerB, registerC);
        int denominator = 1;
        for (int i = 0; i < comboOperand; i++) {
            denominator *= 2;
        }

        registerC = numerator / denominator;
    }
}
