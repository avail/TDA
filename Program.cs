using System;
using System.Diagnostics;

using BeatmapInfo;
using PerformanceProcessor;

public class Program
{
    public static void Main(string[] args)
    {
        if(args.Length == 0)
        {
            Console.WriteLine("Taiko Difficulty Analyzer");
            Console.WriteLine("\nUsage: TDA \"map.osu\" ");
        }
        //Otherwise try to run the program, and catch and display any exceptions that arise
        else
        {
            try
            {
                Console.Write("Number of 100s scored: ");
                var hundreds = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Number of misses: ");
                var misses = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Max combo (default TotalNotes - misses): ");
                var maxcombo = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Mods (e.g. hdhrdt, any order): ");
                var mods = Console.ReadLine();
                Console.WriteLine();

                PPCalc calculator = new PPCalc(args[0], hundreds, misses, maxcombo, mods);
                calculator.PrintStats();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
