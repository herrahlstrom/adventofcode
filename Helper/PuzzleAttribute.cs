using System;
using System.Linq;

namespace AdventOfCode.Helper;

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
}
