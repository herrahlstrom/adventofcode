using System.Text;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2021, 4, "Giant Squid")]
internal class Year2021Day04 : IPuzzle
{
    private readonly ICollection<Board> _boards = new List<Board>();
    private readonly List<int> _numbers = new();

    public object FirstPart()
    {
        var queue = new Queue<int>(_numbers);

        while (queue.TryDequeue(out int n))
        {
            foreach (Board board in _boards)
            {
                board.Mark(n);
                if (board.HasBingo())
                {
                    int unmarkedSum = board.GetUnmarkedNumbers().Sum();
                    return unmarkedSum * n;
                }
            }
        }

        throw new NotImplementedException();
    }

    public void ReadInput()
    {
        string input = File.ReadAllText("Input/04.txt");

        using var reader = new StringReader(input ?? throw new InvalidOperationException("Input is not initialized"));

        string firstLine = reader.ReadLine() ?? throw new InvalidOperationException("Invalid input");
        _numbers.AddRange(GetNumbers(firstLine));

        reader.ReadLine();

        while (true)
        {
            if (reader.ReadLine() is { } line1)
            {
                Board b = new();
                b.AddLine(0, line1);
                b.AddLine(1, reader.ReadLine() ?? throw new InvalidOperationException());
                b.AddLine(2, reader.ReadLine() ?? throw new InvalidOperationException());
                b.AddLine(3, reader.ReadLine() ?? throw new InvalidOperationException());
                b.AddLine(4, reader.ReadLine() ?? throw new InvalidOperationException());
                _boards.Add(b);
            }
            else
            {
                break;
            }

            reader.ReadLine();
        }
    }

    public object SecondPart()
    {
        var queue = new Queue<int>(_numbers);
        var boards = new List<Board>(_boards);

        while (queue.TryDequeue(out int n) && boards.Count > 0)
        {
            foreach (Board board in _boards)
            {
                board.Mark(n);
            }

            if (boards.Count == 1 && boards[0].HasBingo())
            {
                int unmarkedSum = boards[0].GetUnmarkedNumbers().Sum();
                return unmarkedSum * n;
            }

            boards.RemoveAll(b => b.HasBingo());
        }

        throw new NotImplementedException();
    }

    private static IEnumerable<int> GetNumbers(string line)
    {
        StringBuilder n = new(2);
        foreach (char c in line)
        {
            if (c == ',')
            {
                yield return int.Parse(n.ToString());
                n.Length = 0;
            }
            else
            {
                n.Append(c);
            }
        }

        if (n.Length > 0)
        {
            yield return int.Parse(n.ToString());
        }
    }

    private class Board
    {
        private readonly HashSet<int> _marked = new();
        private readonly int[,] _numbers = new int[5, 5];

        public void AddLine(int n, string line)
        {
            int[] numbers = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
            for (int i = 0; i < numbers.Length; i++)
            {
                _numbers[n, i] = numbers[i];
            }
        }

        public IEnumerable<int> GetUnmarkedNumbers()
        {
            for (int n = 0; n < 5; n++)
            {
                for (int m = 0; m < 5; m++)
                {
                    if (_marked.Contains(_numbers[n, m]) == false)
                    {
                        yield return _numbers[n, m];
                    }
                }
            }
        }

        public bool HasBingo()
        {
            for (int n = 0; n < 5; n++)
            {
                int a = 0;
                int b = 0;

                for (int m = 0; m < 5; m++)
                {
                    a += _marked.Contains(_numbers[n, m]) ? 1 : 0;
                    b += _marked.Contains(_numbers[m, n]) ? 1 : 0;
                }

                if (a >= 5 || b >= 5)
                {
                    return true;
                }
            }

            return false;
        }

        public void Mark(int number)
        {
            _marked.Add(number);
        }
    }
}