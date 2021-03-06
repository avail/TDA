using System;

using PerformanceProcessor;
using BeatmapInfo;
using HitObjectInterpreter;

//Acts as the bridge between the user input (in the form of command-line arguments) and TaikoCalc
public class PPCalc
{
    private TaikoCalc processor;
    private Beatmap map;

    public PPCalc(string osufile, string hundreds, string misses, string _maxcombo, string mods)
    {
        //Format the command line arguments into a string array
        //array order is {filepath, amount100, amountmiss, maxcombo, mods}
        //string[] rawargs = ProcessArguments(args);

        map = new Beatmap(osufile);

        int amount300, amount100, amountmiss, maxcombo;
        string modlist;
        int totalobjects = GetNoteCount();

        //Put the arguments into the correct-type variables
        if(hundreds == "")
            amount100 = 0;
        else
            amount100 = Convert.ToInt32(hundreds);

        if(misses == "")
            amountmiss = 0;
        else
            amountmiss = Convert.ToInt32(misses);

        if(_maxcombo == "")
            maxcombo = totalobjects - amountmiss;
        else
            maxcombo = Convert.ToInt32(_maxcombo);

        if(mods == "")
            modlist = "";
        else
            modlist = mods;

        //Infer the number of 300s from the amount of 100s and misses
        amount300 = totalobjects - amount100 - amountmiss;
        if(amount300 < 0)
            throw new Exception("Error, Invalid number of 100s or misses");

        //Get the mods used in the play
        Modifiers usedmods = ConstructMods(modlist);

        processor = new TaikoCalc(maxcombo, amount300, amount100, amountmiss, usedmods, map);
    }

    //Print out the beatmap name and its difficulty
    public void PrintStats()
    {
        Console.WriteLine(map.GetTag("Metadata", "Title") + ", " + map.GetTag("Metadata", "Version"));
        Console.WriteLine("Star Rating: " + processor.GetSR() + "*");
        Console.WriteLine("PP: " + processor.GetPP());
    }

    //Gets the number of circles (only important note) in the beatmap
    private int GetNoteCount()
    {
        HitObjectListParser hitobjects = new HitObjectListParser(map);

        int count = 0;
        for(int i = 0; i < hitobjects.GetSize(); i++)
        {
            if(hitobjects.GetHitObjectType(i) == HitObjectType.Circle)
                count++;
        }
        return count;
    }

    //Takes a list of mods and turns it into a Modifiers variable
    private Modifiers ConstructMods(string modlist)
    {
        if(modlist == "")
            return Modifiers.None;

        modlist = modlist.ToUpper();
        if(modlist.Length % 2 != 0)
            throw new Exception("Error, invalid mods argument");

        Modifiers usedmods = Modifiers.None;
        for(int i = 0; i < modlist.Length; i+=2)
        {
            string potentialmod = modlist.Substring(i, 2);
            if(potentialmod == "NF")
                usedmods = (Modifiers)((int)usedmods | (int)Modifiers.NoFail);
            else if(potentialmod == "EZ")
                usedmods = (Modifiers)((int)usedmods | (int)Modifiers.Easy);
            else if(potentialmod == "HD")
                usedmods = (Modifiers)((int)usedmods | (int)Modifiers.Hidden);
            else if(potentialmod == "HR")
                usedmods = (Modifiers)((int)usedmods | (int)Modifiers.HardRock);
            else if(potentialmod == "DT")
                usedmods = (Modifiers)((int)usedmods | (int)Modifiers.DoubleTime);
            else if(potentialmod == "HT")
                usedmods = (Modifiers)((int)usedmods | (int)Modifiers.HalfTime);
            else if(potentialmod == "NC")
                usedmods = (Modifiers)((int)usedmods | (int)Modifiers.Nightcore);
            else if(potentialmod == "FL")
                usedmods = (Modifiers)((int)usedmods | (int)Modifiers.Flashlight);
        }

        return usedmods;
    }

    //Extracts the relevant information from a list of arguments, without formatting
    //Returned list contains [amount100, amountmiss, maxcombo, mods]
    private string[] ProcessArguments(string[] args)
    {
        string filepath = args[0];
        string amount100 = "";
        string amountmiss = "";
        string mods = "";
        string maxcombo = "";

        //args does NOT contain the program itself as an argument
        for(int i = 1; i < args.Length; i++)
        {
            if(args[i] == "-a")
            {
                if(i + 1 >= args.Length)
                    throw new Exception("Error, no argument provided");
                else
                    amount100 = args[i+1];
            }
            else if(args[i] == "-m")
            {
                if(i + 1 >= args.Length)
                    throw new Exception("Error, no argument provided");
                else
                    amountmiss = args[i+1];
            }
            else if(args[i] == "-c")
            {
                if(i + 1 >= args.Length)
                    throw new Exception("Error, no argument provided");
                else
                    maxcombo = args[i+1];
            }
            else if(args[i] == "-M")
            {
                if(i + 1 >= args.Length)
                    throw new Exception("Error, no argument provided");
                else
                    mods = args[i+1];
            }
        }

        string[] rawargs = {filepath, amount100, amountmiss, maxcombo, mods};
        return rawargs;
    }
}
