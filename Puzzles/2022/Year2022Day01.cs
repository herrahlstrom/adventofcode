using AdventOfCode.Helper;

namespace AdventOfCode.Puzzles;

[Puzzle(2022, 1, "Calorie Counting")]
public class Year2022Day01 : IPuzzle
{
    public object FirstPart()
    {
        int maxCalories = 0;

        int calories = 0;
        foreach(var line in InputReader.ReadLines(this))
        {
            if(line == "")
            {
                maxCalories = calories > maxCalories ? calories : maxCalories;
                calories = 0;
                continue;
            }
            calories += int.Parse(line);
        }
        maxCalories = calories > maxCalories ? calories : maxCalories;

        return maxCalories;
    }

    public object SecondPart()
    {
        List<int> allCalories = new();

        int calories = 0;
        foreach(var line in InputReader.ReadLines(this))
        {
            if(line == "")
            {
                allCalories.Add(calories);
                calories = 0;
                continue;
            }
            calories += int.Parse(line);
        }
        if(calories > 0)
        {
            allCalories.Add(calories);
        }

        var sumCaloriesTop3Elfs = allCalories
            .OrderByDescending(x => x)
            .Take(3)
            .Sum();

        return sumCaloriesTop3Elfs;
    }
}
