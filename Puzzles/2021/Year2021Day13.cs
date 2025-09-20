using System.Reflection;
using System.Text.RegularExpressions;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2021;

[Puzzle(2021, 13, "Transparent Origami")]
internal class Year2021Day13 : IPuzzle
{
    private readonly List<FoldInstruction> _foldInstructions = new();
    private readonly List<Point> _points = new();
    private Point _end;

    public object FirstPart()
    {
        ReadInput();

        bool[,] paper = new bool[_end.X + 1, _end.Y + 1];

        int width = paper.GetLength(0);
        int height = paper.GetLength(1);

        foreach (Point point in _points)
        {
            paper[point.X, point.Y] = true;
        }

        FoldInstruction instruction = _foldInstructions.First();

        Fold(instruction, paper, ref width, ref height);

        return Count(paper, height, width);
    }

    public void ReadInput()
    {
        using var reader = InputReader.OpenStreamReader(this);

        _end = new Point(0, 0);
        while (!reader.EndOfStream && reader.ReadLine() is { Length: > 0 } line)
        {
            string[] arr = line.Split(',');
            int x = int.Parse(arr[0]);
            int y = int.Parse(arr[1]);
            _points.Add(new Point(x, y));

            if (x > _end.X || y > _end.Y)
            {
                _end = new Point(Math.Max(x, _end.X), Math.Max(y, _end.Y));
            }
        }

        var foldRx = new Regex(@"^fold along (x|y)=(\d+)$");
        while (!reader.EndOfStream && reader.ReadLine() is { } line)
        {
            Match foldRxMatch = foldRx.Match(line);
            _foldInstructions.Add(new FoldInstruction
            {
                Direction = foldRxMatch.Groups[1].Value,
                Position = int.Parse(foldRxMatch.Groups[2].Value)
            });
        }
    }

    public object SecondPart()
    {
        bool[,] paper = new bool[_end.X + 1, _end.Y + 1];

        int width = paper.GetLength(0);
        int height = paper.GetLength(1);

        foreach (Point point in _points)
        {
            paper[point.X, point.Y] = true;
        }

        foreach (FoldInstruction instruction in _foldInstructions)
        {
            Fold(instruction, paper, ref width, ref height);
        }

        
        PuzzleAttribute puzzle = GetType().GetCustomAttribute<PuzzleAttribute>()!;
        Console.WriteLine("");
        Console.WriteLine("- Output from {0} {1}: {2} -", puzzle.Year, puzzle.Day, puzzle.Name);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Console.Write(paper[x, y] ? "¤" : " ");
            }

            Console.WriteLine("");
        }
        
        // Catched from console output
        return "LKREBPRK";
    }

    private static long Count(bool[,] paper, int height, int width)
    {
        long count = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (paper[x, y])
                {
                    count++;
                }
            }
        }

        return count;
    }

    private void Fold(FoldInstruction instruction, bool[,] paper, ref int width, ref int height)
    {
        switch (instruction.Direction)
        {
            case "y":
                FoldHorizontally(paper, instruction.Position);
                break;

            case "x":
                FoldVertically(paper, instruction.Position);
                break;

            default:
                throw new InvalidOperationException();
        }

        (width, height) = instruction.Direction switch
        {
            "y" => (width, instruction.Position),
            "x" => (instruction.Position, height),
            _ => throw new InvalidOperationException()
        };
    }

    private void FoldHorizontally(bool[,] paper, int y)
    {
        for (int dY = 1; dY <= y; dY++)
        {
            for (int x = 0; x <= _end.X; x++)
            {
                if (paper[x, y + dY])
                {
                    paper[x, y - dY] = true;
                    paper[x, y + dY] = false;
                }
            }
        }
    }

    private void FoldVertically(bool[,] paper, int x)
    {
        for (int dX = 1; dX <= x; dX++)
        {
            for (int y = 0; y <= _end.Y; y++)
            {
                if (paper[x + dX, y])
                {
                    paper[x - dX, y] = true;
                    paper[x + dX, y] = false;
                }
            }
        }
    }

    private class FoldInstruction
    {
        public string Direction { get; init; } = "";
        public int Position { get; init; }
    }
}