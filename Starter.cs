using System;
using System.IO;
using System.Collections.Generic;


class Program
{

    static void Main(string[] args)
    {
        try
        {
            using StreamReader sr = new("./day-x.in");

            string? line;
            while ((line = sr.ReadLine()) != null)
            {
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
    }

}
