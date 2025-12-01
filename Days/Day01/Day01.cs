/*
 * ====================================================
 * ADVENT OF CODE 2025 - DAY 01
 * ====================================================
 * author: @turbits
 * repo: https://github.com/turbits/advent-of-code-2025
 * website: https://aoc25.turbits.sh
 * license: MIT
 * ====================================================
 * DISCLAIMER
 * dont expect any good code in my AOC25 repo
 * ====================================================
 *
 * happy holidays :^)
 * 
*/

namespace TurbitsAOC25.Days.Day01;

enum Direction
{
    NONE,
    LEFT,
    RIGHT
}

public class Day01
{
    record Movement(Direction Direction, int Steps);
    public static int Day => 1;
    public static string Name => "Secret Entrance";

    private int _dialLocation = 50; // starts at 50 for p1 and p2
    
    private int _part1ZeroCount;
    private bool _part1Solved;
    
    private int _part2ZeroCount;
    private bool _part2Solved;

    public void Run()
    {
        string lineBreak = new('=', 50);
        string inputFilePath = Path.Combine(AppContext.BaseDirectory, "Days", "Day01", "input.txt");
        List<Movement> movements = InitializeMovements(inputFilePath);

        Console.CursorVisible = false;
        
        Console.WriteLine(lineBreak);
        Console.WriteLine("++ DAY 01 ++");
        
        do {
            Console.WriteLine("Solving part 1...");
            SolvePart1(movements);
            Console.WriteLine($"Part 1 answer: {_part1ZeroCount}");
        } while (!_part1Solved);

        Console.WriteLine();
        
        do {
            Console.WriteLine("Solving part 2...");
            SolvePart2(movements);
            Console.WriteLine($"Part 2 answer: {_part2ZeroCount}");
        } while (!_part2Solved);
        
        Console.CursorVisible = true;
    }

    private static Direction ParseDirection(string movement)
    {
        char.TryParse(movement.Remove(1), out char dir);
        switch (dir)
        {
            case 'L':
                return Direction.LEFT;
            case 'R':
                return Direction.RIGHT;
            default:
                return Direction.NONE;
        }
    }

    private void SolvePart1(List<Movement> movements)
    {
        const int dialSize = 100;
        int currentLocation = _dialLocation;

        for (var i = 0; i < movements.Count; i++) {
            var dir = movements[i].Direction;
            int steps = movements[i].Steps;
            
            DrawProgressBar(i + 1, movements.Count);
            
            // turn dial
            switch (dir) {
                case Direction.RIGHT:
                    currentLocation = (currentLocation + steps) % dialSize;
                    break;
                case Direction.LEFT:
                    currentLocation = ((currentLocation - steps) % dialSize + dialSize) % dialSize;
                    break;
                case Direction.NONE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }

            if (currentLocation == 0) _part1ZeroCount++;
            _dialLocation = currentLocation;
        }
        
        _part1Solved = true;
    }
    
    private void SolvePart2(List<Movement> movements)
    {
        const int dialSize = 100;
        _dialLocation = 50; // reset dial
        int currentLocation = _dialLocation;
        
        for (var i = 0; i < movements.Count; i++) {
            var dir = movements[i].Direction;
            int steps = movements[i].Steps;
            
            DrawProgressBar(i + 1, movements.Count);
            
            CountZeroClicks(movements[i]);
            
            switch (dir) {
                case Direction.RIGHT:
                    currentLocation = (currentLocation + steps) % dialSize;
                    break;
                case Direction.LEFT:
                    currentLocation = ((currentLocation - steps) % dialSize + dialSize) % dialSize;
                    break;
                case Direction.NONE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
            _dialLocation = currentLocation;
        }
        
        _part2Solved = true;
    }

    private void CountZeroClicks(Movement movement) {
        int needleLoc = _dialLocation;
        var dir = movement.Direction;
        int steps = movement.Steps;
        var zeroCount = 0;
        var dialSize = 100;
        
        for (var currentSteps = 0; currentSteps < steps; currentSteps++) {
            switch (dir) {
                case Direction.RIGHT:
                    needleLoc = (needleLoc + 1) % dialSize;
                    break;
                case Direction.LEFT:
                    needleLoc = (needleLoc - 1 + dialSize) % dialSize;
                    break;
                case Direction.NONE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }

            if (needleLoc == 0) {
                zeroCount++;
            }
        }
        _part2ZeroCount += zeroCount;
    }

    private List<Movement> InitializeMovements(string inputFilePath)
    {
        List<Movement> movements = new();
        foreach (string line in File.ReadLines(inputFilePath))
        {
            Direction dir = ParseDirection(line);
            int.TryParse(line.Substring(1), out int steps);
            movements.Add(new Movement(dir, steps));
        }

        return movements;
    }

    private static void DrawProgressBar(int progress, int total) {
        const int barLength = 25;
        double percentage = (double)progress / total;
        var filledLength = (int)(percentage * barLength);
        var progressBar = "[";
        for (var i = 0; i < barLength; i++) {
            if (i < filledLength) {
                progressBar += "#";
            } else {
                progressBar += "-";
            }
        }

        progressBar += $"] {percentage:P0}";
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(progressBar.PadRight(Console.WindowWidth));
    }
}

