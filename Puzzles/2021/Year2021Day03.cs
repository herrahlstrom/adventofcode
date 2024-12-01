using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 3, "Binary Diagnostic")]
internal class Year2021Day03 : IPuzzle
{
    public object FirstPart()
    {
        using var reader = InputReader.OpenStreamReader(this);

        string firstLine = reader.ReadLine() ?? throw new InvalidDataException();
        int[] setCounter = firstLine.Select(c => c == 1 ? 1 : 0).ToArray();
        int counter = 1;

        while (reader.ReadLine() is { } line)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '1')
                {
                    setCounter[i]++;
                }
            }

            counter++;
        }

        string gammaBits = string.Join("", setCounter.Select(n => n > counter / 2 ? "1" : "0"));
        int gamma = Convert.ToInt32(gammaBits, 2);

        string epsilonBits = string.Join("", setCounter.Select(n => n < counter / 2 ? "1" : "0"));
        int epsilon = Convert.ToInt32(epsilonBits, 2);

        return gamma * epsilon;
    }

    public object SecondPart()
    {
        IList<string> lines = InputReader.ReadAllLines(this);

        int oxygen = GetOxygen(new List<string>(lines));
        int co2 = GetCo2(new List<string>(lines));

        return oxygen * co2;

        static int GetOxygen(List<string> lines)
        {
            for (int i = 0; ; i++)
            {
                int zeros = 0;
                int ones = 0;
                foreach (string line in lines)
                {
                    zeros += line[i] == '0' ? 1 : 0;
                    ones += line[i] == '1' ? 1 : 0;
                }

                if (ones >= zeros)
                {
                    lines.RemoveAll(x => x[i] != '1');
                }
                else
                {
                    lines.RemoveAll(x => x[i] != '0');
                }

                if (lines.Count == 1)
                {
                    return Convert.ToInt32(lines[0], 2);
                }
            }

            throw new Exception("Can't determin oxygen");
        }

        static int GetCo2(List<string> lines)
        {
            for (int i = 0; ; i++)
            {
                int zeros = 0;
                int ones = 0;
                foreach (string line in lines)
                {
                    zeros += line[i] == '0' ? 1 : 0;
                    ones += line[i] == '1' ? 1 : 0;
                }

                if (ones >= zeros)
                {
                    lines.RemoveAll(x => x[i] == '1');
                }
                else
                {
                    lines.RemoveAll(x => x[i] == '0');
                }

                if (lines.Count == 1)
                {
                    return Convert.ToInt32(lines[0], 2);
                }
            }

            throw new Exception("Can't determin oxygen");
        }
    }
}