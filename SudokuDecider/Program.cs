using System;
using System.Collections.Generic;
using Decider.Csp.BaseTypes;
using Decider.Csp.Global;

namespace SudokuDecider;

using System.IO;
using Decider.Csp.Integer;

public static class Program
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(args[0]);
    var sudoku = new VariableInteger[9, 9];
    for (var i = 0; i < 9; i++)
    {
      var line = lines[i].Replace(" ", "").ToCharArray();
      for (var j = 0; j < 9; j++)
      {
        var ch = line[j];
        var name = $"s{i}{j}";
        sudoku[i, j] = ch == '.' ? new VariableInteger(name, 0, 9) : new VariableInteger(name, int.Parse(ch.ToString()), int.Parse(ch.ToString()));
      }
    }

    DumpBoard(sudoku);

    Console.WriteLine();
    Console.WriteLine("Attempting solution...");
    Console.WriteLine();

    if (Solve(sudoku))
    {
      DumpBoard(sudoku);
    }
    else
    {
      Console.WriteLine("Solution failed");
    }
  }

  private static void DumpBoard(VariableInteger[,] sudoku)
  {
    for (var i = 0; i < sudoku.GetLength(0); i++)
    {
      for (var j = 0; j < sudoku.GetLength(1); j++)
      {
        var element = sudoku[i, j];
        var val = element.IsBound ? element.Value.ToString() : ".";
        Console.Write($"{val} ");
      }

      Console.WriteLine();
    }
  }

  private static bool Solve(VariableInteger[,] board)
  {
    if (board == null || board.Length == 0)
    {
      return false;
    }

    return true;
  }

  private static List<IConstraint> GetConstraints(VariableInteger[,] sudoku)
  {
    var constraints = new List<IConstraint>();

    // 1. Each row must contain the numbers from 1 to 9, without repetitions
    // 9 --> AllDifferentInteger
    for (var row = 0; row < 9; row++)
    {
      var allDiff = new AllDifferentInteger(new { });
    }

    // 2. Each column must contain the numbers from 1 to 9, without repetitions
    // 9 --> AllDifferentInteger

    // 3. The digits can only occur once per 3x3 block (nonet)
    // 9 --> AllDifferentInteger

    // 4. The sum of every single row, column, and nonet must equal 45
    // 9 --> ConstraintInteger

    return constraints;
  }
}
