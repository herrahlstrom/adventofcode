using System;
using System.Linq;

namespace AdventOfCode;

internal class PuzzleAttribute : Attribute
{
    public PuzzleAttribute(int year, int day, string name)
    {
        Name = name;
        Year = year;
        Day = day;
    }

    public int Year { get; }

    public int Day { get; }

    public string Name { get; set; }

    public bool PrintToConsole { get; set; }
}
