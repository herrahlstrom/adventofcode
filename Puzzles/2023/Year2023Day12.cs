using AdventOfCode.Helper;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 12, "Hot Springs")]
internal class Year2023Day12 : IPuzzle
{
    [Answer(7260L)]
    public object FirstPart()
    {
        long sum = 0;
        foreach (var line in InputReader.ReadLines(this))
        {
            sum += CalculateLine(line, 1);
        }
        return sum;
    }

    [Answer(1909291258644L)]
    public object SecondPart()
    {
        long sum = 0;
        foreach (var line in InputReader.ReadLines(this))
        {
            sum += CalculateLine(line, 5);
        }
        return sum;
    }

    private static long CalculateLine(string line, int expanderCount)
    {
        var p = line.IndexOf(' ');
        string springs = line[..p];
        IList<int> groups = line[(p + 1)..].Split(',').Select(int.Parse).ToArray();

        if (expanderCount > 0)
        {
            springs = ExpandSprings(springs, expanderCount);
            groups = ExpandGroups(groups, expanderCount);
        }

        var solver = new Solver(springs.ToString(), groups);
        return solver.CountRowArrangements();
    }

    private static List<int> ExpandGroups(IList<int> groups, int count)
    {
        List<int> expandedGroups = new(groups);
        for (int i = 1; i < count; i++)
        {
            expandedGroups.AddRange(groups);
        }
        return expandedGroups;
    }

    private static string ExpandSprings(string springs, int count)
    {
        StringBuilder expandedSprings = new(springs);
        for (int i = 1; i < count; i++)
        {
            expandedSprings.AppendFormat(",{0}", springs);
        }
        return expandedSprings.ToString();
    }

    private class Solver(string springs, IList<int> groups)
    {
        private readonly Dictionary<long, long> _remainingArrangements = [];

        public long CountRowArrangements(int s = 0, int g = 0)
        {
            if (s >= springs.Length)
            {
                return g >= groups.Count ? 1 : 0;
            }
            if (!_remainingArrangements.ContainsKey(g * springs.Length + s))
            {
                long arrangements = 0;
                if (CanPlaceGroup(s, g))
                {
                    arrangements += CountRowArrangements(s + 1 + groups[g], g + 1);
                }
                if (springs[s] != '#' && CanFitRemainingGroups(s + 1, g))
                {
                    arrangements += CountRowArrangements(s + 1, g);
                }
                _remainingArrangements[g * springs.Length + s] = arrangements;
            }
            return _remainingArrangements[g * springs.Length + s];
        }

        bool CanFitRemainingGroups(int s, int g)
        {
            for (; g < groups.Count; g++)
            {
                s += groups[g] + 1;
            }
            return s <= springs.Length + 1;
        }

        bool CanPlaceGroup(int s, int g)
        {
            if (g >= groups.Count)
            {
                return false;
            }

            int max = s + groups[g];

            for (int i = s; i < max; i++)
            {
                if (i >= springs.Length || springs[i] == '.')
                    return false;
            }

            if (max >= springs.Length) { return true; }
            return springs[max] != '#';
        }
    }
}
