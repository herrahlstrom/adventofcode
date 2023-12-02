using AdventOfCode.Helper;
using System.Diagnostics;

namespace AdventOfCode.Puzzles;

[Puzzle(2022, 3, "Rucksack Reorganization")]
internal class Year2022Day03 : IPuzzle
{
    public static int GetPrio(char c)
    {
        int ascii = (int)c;
        return ascii switch
        {
            >= 97 and <= 122 => ascii - 96,
            >= 65 and <= 90 => ascii - 38,
            _ => throw new UnreachableException(),
        };
    }

    public object FirstPart()
    {
        int result = 0;
        foreach (var racksack in InputReader.ReadLines(this))
        {
            int compartmentSize = racksack.Length / 2;

            HashSet<char> commonCharacters = new();
            for (int i = 0; i < compartmentSize; i++)
            {
                if (racksack.IndexOf(racksack[i], compartmentSize) > 0 &&
                    commonCharacters.Add(racksack[i]))
                {
                    result += GetPrio(racksack[i]);
                }
            }
        }
        return result;
    }

    public object SecondPart()
    {
        int result = 0;

        foreach (var group in InputReader.ReadLines(this).Chunk(3))
        {
            HashSet<char> commonCharacters = new();

            foreach (char c in group[0])
            {
                if (group[1].Contains(c) &&
                    group[2].Contains(c) &&
                    commonCharacters.Add(c))
                {
                    result += GetPrio(c);
                }
            }
        }
        return result;
    }
}