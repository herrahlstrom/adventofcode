using AdventOfCode.Helper;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles;

[Puzzle(2022, 11, "Monkey in the Middle")]
internal class Year2022Day11 : IPuzzle
{
    public object FirstPart()
    {
        using var reader = InputReader.OpenStreamReader(this);

        int rounds = 20;
        Monkey[] monkeys = ReadMonkeys(reader).ToArray();

        for (int round = 1; round <= rounds; round++)
        {
            foreach (Monkey monkey in monkeys)
            {
                foreach (int item in monkey.Items)
                {
                    monkey.Inspections++;

                    int worryLevel = (int)decimal.Floor(monkey.Operation(item) / 3m);

                    int targetMonkeyId = worryLevel % monkey.TestDivisionNumber == 0
                        ? monkey.ThrowToIfTrue
                        : monkey.ThrowToIfFalse;
                    monkeys[targetMonkeyId].Items.Add(worryLevel);
                }
                monkey.Items.Clear();
            }
        }

        return monkeys
            .OrderByDescending(x => x.Inspections)
            .Take(2).Aggregate(1L, (n, monkey) => n * monkey.Inspections);
    }

    public object SecondPart()
    {
        using var reader = InputReader.OpenStreamReader(this);

        int rounds = 10000;
        Monkey[] monkeys = ReadMonkeys(reader).ToArray();

        long mol = monkeys.Aggregate(1L, (n, monkey) => n * monkey.TestDivisionNumber);

        for (int round = 1; round <= rounds; round++)
        {
            foreach (Monkey monkey in monkeys)
            {
                foreach (int item in monkey.Items)
                {
                    monkey.Inspections++;
                    long worryLevel = monkey.Operation(item) % mol;

                    int targetMonkeyId = worryLevel % monkey.TestDivisionNumber == 0
                        ? monkey.ThrowToIfTrue
                        : monkey.ThrowToIfFalse;
                    monkeys[targetMonkeyId].Items.Add(worryLevel);
                }
                monkey.Items.Clear();
            }
        }

        return monkeys
            .OrderByDescending(x => x.Inspections)
            .Take(2).Aggregate(1L, (n, monkey) => n * monkey.Inspections);
    }

    bool ReadMonkey(StreamReader reader, [MaybeNullWhen(false)] out Monkey monkey)
    {
        Regex idRx = new(@"Monkey (?<id>\d+):$", RegexOptions.Compiled);
        Regex startItemsRx = new(@"Starting items: (?<items>(?:\d+)(?:,\s*\d+)*)$", RegexOptions.Compiled);
        Regex operationMultiplyByNumberRx = new(@"Operation: new = old \* (?<value>\d+)$", RegexOptions.Compiled);
        Regex operationAddByNumberRx = new(@"Operation: new = old \+ (?<value>\d+)$", RegexOptions.Compiled);
        Regex operationMultiplyBySelfRx = new(@"Operation: new = old \* old$", RegexOptions.Compiled);
        Regex testRx = new(@"Test: divisible by (?<number>\d+)$", RegexOptions.Compiled);
        Regex ifTrueActionRx = new(@"If true: throw to monkey (?<id>\d+)$", RegexOptions.Compiled);
        Regex ifFalseActionRx = new(@"If false: throw to monkey (?<id>\d+)$", RegexOptions.Compiled);

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine()!;
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var idMatch = idRx.Match(line);
            Debug.Assert(idMatch.Success);

            var startItemsMatch = startItemsRx.Match(reader.ReadLine()!);
            Debug.Assert(startItemsMatch.Success);

            string operationLine = reader.ReadLine()!;

            var testMatch = testRx.Match(reader.ReadLine()!);
            Debug.Assert(testMatch.Success);
            int testPredicateNumber = int.Parse(testMatch.Groups["number"].Value);
            
            var ifTrueActionMatch = ifTrueActionRx.Match(reader.ReadLine()!);
            Debug.Assert(ifTrueActionMatch.Success);

            var ifFalseActionMatch = ifFalseActionRx.Match(reader.ReadLine()!);
            Debug.Assert(ifFalseActionMatch.Success);

            monkey = new Monkey
            {
                Id = int.Parse(idMatch.Groups["id"].Value),
                Items = startItemsMatch.Groups["items"].Value.Split(',').Select(long.Parse).ToList(),
                Operation = GetOperations(operationLine),
                TestDivisionNumber = testPredicateNumber,
                ThrowToIfTrue = int.Parse(ifTrueActionMatch.Groups["id"].Value),
                ThrowToIfFalse = int.Parse(ifFalseActionMatch.Groups["id"].Value)
            };
            return true;
        }

        monkey = default;
        return false;

        Func<long, long> GetOperations(string line)
        {
            if (operationMultiplyByNumberRx.Match(line) is { Success: true } operationMultiplyByNumberMatch)
            {
                int value = int.Parse(operationMultiplyByNumberMatch.Groups["value"].Value);
                return (n) => n * value;
            }

            if (operationMultiplyBySelfRx.Match(line) is { Success: true } operationMultiplyBySelfMatch)
            {
                return (n) => n * n;
            }

            if (operationAddByNumberRx.Match(line) is { Success: true } operationAddByNumberMatch)
            {
                int value = int.Parse(operationAddByNumberMatch.Groups["value"].Value);
                return (n) => n + value;
            }

            throw new NotSupportedException();
        }
    }

    private IEnumerable<Monkey> ReadMonkeys(StreamReader reader)
    {
        while (ReadMonkey(reader, out var monkey))
        {
            yield return monkey;
        }
    }
}

class Monkey
{
    public required int Id { get; set; }

    public long Inspections { get; set; } = 0;
    public required List<long> Items { get; set; }
    public required Func<long, long> Operation { get; set; }
    public required long TestDivisionNumber { get; set; }
    public required int ThrowToIfFalse { get; set; }
    public required int ThrowToIfTrue { get; set; }
}

