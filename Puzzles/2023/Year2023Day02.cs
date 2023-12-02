using System;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2023, 2, "Cube Conundrum")]
public class Year2023Day02 : IPuzzle
{
    public object FirstPart()
    {
        const int MaxRed = 12;
        const int MaxGreen = 13;
        const int MaxBlue = 14;

        return InputReader.ReadLines(this)
                          .Select(ReadGame)
                          .Where(game => game.Sets.All(x => x.Red <= MaxRed && x.Green <= MaxGreen && x.Blue <= MaxBlue))
                          .Sum(game => game.Id);
    }

    public object SecondPart()
    {
        long sum = 0;
        foreach (var game in InputReader.ReadLines(this).Select(ReadGame))
        {
            int red = game.Sets.Max(x => x.Red);
            int green = game.Sets.Max(x => x.Green);
            int blue = game.Sets.Max(x => x.Blue);
            sum += red * green * blue;
        }
        return sum;
    }

    private static void UpdateCube(ref GameSet gameSet, ReadOnlySpan<char> cubeSpan)
    {
        int p = cubeSpan.IndexOf(' ');

        var color = cubeSpan[(p + 1)..];
        int count = int.Parse(cubeSpan[..p]);

        gameSet = color switch
        {
            "red" => gameSet with { Red = count },
            "green" => gameSet with { Green = count },
            "blue" => gameSet with { Blue = count },
            _ => throw new NotSupportedException(),
        };
    }

    private Game ReadGame(string line)
    {
        ReadOnlySpan<char> span = line.AsSpan();
        int p = span.IndexOf(':');

        return new Game(
            Id: int.Parse(span[5..p]),
            Sets: ReadGameData(span[(p + 2)..]));
    }

    private List<GameSet> ReadGameData(ReadOnlySpan<char> span)
    {
        var gameSets = new List<GameSet>();

        int a = 0;
        int b = 0;
        while (++b < span.Length)
        {
            if (span[b] == ';')
            {
                gameSets.Add(ReadGameSet(span[a..b]));
                a = b + 2;
            }
        }

        gameSets.Add(ReadGameSet(span[a..]));
        return gameSets;
    }

    private GameSet ReadGameSet(ReadOnlySpan<char> span)
    {
        var gameSet = new GameSet(0, 0, 0);

        int a = 0;
        int b = 0;

        while (++b < span.Length)
        {
            if (span[b] == ',')
            {
                UpdateCube(ref gameSet, span[a..b]);
                a = b + 2;
            }
        }

        UpdateCube(ref gameSet, span[a..b]);

        return gameSet;
    }

    record Game(int Id, IList<GameSet> Sets);
    record GameSet(int Red, int Green, int Blue);
}
