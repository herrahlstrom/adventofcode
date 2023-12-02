using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 8, "Seven Segment Search")]
internal class Year2021Day08 : IPuzzle
{
    private readonly List<Entry> _entries = new();

    public object FirstPart()
    {
        ReadInput();

        int hits = 0;

        foreach (Entry entry in _entries)
        {
            foreach (string output in entry.Output)
            {
                if (output.Length is 2 or 3 or 4 or 7)
                {
                    hits++;
                }
            }
        }

        return hits;
    }

    public void ReadInput()
    {
        using var reader = InputReader.OpenStreamReader(this);

        while (!reader.EndOfStream && reader.ReadLine() is { Length: > 0 } line)
        {
            _entries.Add(new Entry
            {
                Patterns = line[..58].Split(' '),
                Output = line[61..].Split(' ')
            });
        }
    }

    public object SecondPart()
    {
        long sum = 0;
        foreach (Entry entry in _entries)
        {
            string[] numbers = GetPatterns(entry);

            for (int i = 0; i < entry.Output.Length; i++)
            {
                string segments = string.Join("", entry.Output[i].OrderBy(c => c));
                int factor = (int)Math.Pow(10, 3 - i);
                int value = Array.IndexOf(numbers, segments);

                sum += value * factor;
            }
        }

        return sum;

        static string[] GetPatterns(Entry entry)
        {
            var petterns = entry.Patterns.Select(x => string.Join("", x.OrderBy(c => c))).ToList();
            string[] numbers = new string[10];

            string one = petterns.Single(x => x.Length == 2);
            numbers[1] = one;
            petterns.Remove(one);

            string seven = petterns.Single(x => x.Length == 3);
            numbers[7] = seven;
            petterns.Remove(seven);

            string four = petterns.Single(x => x.Length == 4);
            numbers[4] = four;
            petterns.Remove(four);

            string eight = petterns.Single(x => x.Length == 7);
            numbers[8] = eight;
            petterns.Remove(eight);

            char a = seven.Single(x => !one.Contains(x));

            string three = petterns.Single(x => x.Length == 5 && seven.All(x.Contains));
            numbers[3] = three;
            petterns.Remove(three);

            string nine = petterns.Single(x => x.Length == 6 && three.All(x.Contains));
            numbers[9] = nine;
            petterns.Remove(nine);

            char b = nine.Single(x => !three.Contains(x));

            string five = petterns.Single(x => x.Length == 5 && x.Contains(b));
            numbers[5] = five;
            petterns.Remove(five);

            string two = petterns.Single(x => x.Length == 5);
            numbers[2] = two;
            petterns.Remove(two);

            string zero = petterns.Single(x => x.Length == 6 && seven.All(x.Contains));
            numbers[0] = zero;
            petterns.Remove(zero);

            string six = petterns.Single(x => x.Length == 6);
            numbers[6] = six;
            petterns.Remove(six);

            return numbers;
        }
    }

    public class Entry
    {
        public string[] Output { get; init; } = Array.Empty<string>();
        public string[] Patterns { get; init; } = Array.Empty<string>();
    }
}