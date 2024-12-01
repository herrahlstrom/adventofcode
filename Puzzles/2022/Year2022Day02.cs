using System.Diagnostics;
using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles._2022;

[Puzzle(2022, 2, "Rock Paper Scissors")]
internal class Year2022Day02 : IPuzzle
{
    private enum RoundResult { Loss = 0, Draw = 3, Win = 6 }
    private enum Shape { Rock = 1, Paper = 2, Scissors = 3 }

    public object FirstPart()
    {
        int points = 0;

        var strategyGuide = InputReader.ReadLines(this).Select(line => new
        {
            Opponent = line[0] switch
            {
                'A' => Shape.Rock,
                'B' => Shape.Paper,
                'C' => Shape.Scissors,
                _ => throw new UnreachableException()
            },
            You = line[2] switch
            {
                'X' => Shape.Rock,
                'Y' => Shape.Paper,
                'Z' => Shape.Scissors,
                _ => throw new UnreachableException()
            }
        });

        foreach (var round in strategyGuide)
        {
            var roundResult = (round.Opponent, round.You) switch
            {
                (Shape.Rock, Shape.Rock) => RoundResult.Draw,
                (Shape.Rock, Shape.Paper) => RoundResult.Win,
                (Shape.Rock, Shape.Scissors) => RoundResult.Loss,
                (Shape.Paper, Shape.Rock) => RoundResult.Loss,
                (Shape.Paper, Shape.Paper) => RoundResult.Draw,
                (Shape.Paper, Shape.Scissors) => RoundResult.Win,
                (Shape.Scissors, Shape.Rock) => RoundResult.Win,
                (Shape.Scissors, Shape.Paper) => RoundResult.Loss,
                (Shape.Scissors, Shape.Scissors) => RoundResult.Draw,
                _ => throw new UnreachableException()
            };

            points += (int)roundResult + (int)round.You;
        }
        return points;
    }

    public object SecondPart()
    {
        int points = 0;

        var strategyGuide = InputReader.ReadLines(this).Select(line => new
        {
            Opponent = line[0] switch
            {
                'A' => Shape.Rock,
                'B' => Shape.Paper,
                'C' => Shape.Scissors,
                _ => throw new UnreachableException()
            },
            ExpectedResult = line[2] switch
            {
                'X' => RoundResult.Loss,
                'Y' => RoundResult.Draw,
                'Z' => RoundResult.Win,
                _ => throw new UnreachableException()
            }
        });

        foreach (var round in strategyGuide)
        {
            var you = (round.Opponent, round.ExpectedResult) switch
            {
                (Shape.Rock, RoundResult.Loss) => Shape.Scissors,
                (Shape.Rock, RoundResult.Draw) => Shape.Rock,
                (Shape.Rock, RoundResult.Win) => Shape.Paper,
                (Shape.Paper, RoundResult.Loss) => Shape.Rock,
                (Shape.Paper, RoundResult.Draw) => Shape.Paper,
                (Shape.Paper, RoundResult.Win) => Shape.Scissors,
                (Shape.Scissors, RoundResult.Loss) => Shape.Paper,
                (Shape.Scissors, RoundResult.Draw) => Shape.Scissors,
                (Shape.Scissors, RoundResult.Win) => Shape.Rock,
                _ => throw new UnreachableException()
            };

            points += (int)round.ExpectedResult + (int)you;
        }
        return points;
    }
}
