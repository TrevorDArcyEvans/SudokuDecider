namespace SudokuDecider;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Decider.Csp.BaseTypes;
using Decider.Csp.Global;
using Decider.Csp.Integer;

public static class Program
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(args[0]);
    var sudoku = GetBoard(lines);

    DumpBoard(sudoku);

    Console.WriteLine();
    Console.WriteLine("Attempting solution...");
    Console.WriteLine();

    var (success, state) = Solve(sudoku);
    if (success)
    {
      Console.WriteLine($"Runtime:\t{state.Runtime}");
      Console.WriteLine($"Backtracks:\t{state.Backtracks}");
      Console.WriteLine($"Solutions:\t{state.Solutions.Count}");
      Console.WriteLine($"Optimal sln:\t{state.OptimalSolution != null}");
      Console.WriteLine();

      DumpBoard(state.Solutions.First());

      if (state.Solutions.Count > 1)
      {
        Console.WriteLine();

        DumpBoard(state.Solutions.Last());
      }
    }
    else
    {
      Console.WriteLine("Solution failed");
    }
  }

  private static VariableInteger[,] GetBoard(string[] lines)
  {
    var sudoku = new VariableInteger[9, 9];
    for (var row = 0; row < 9; row++)
    {
      var line = lines[row].Replace(" ", "").ToCharArray();
      for (var col = 0; col < 9; col++)
      {
        var ch = line[col];
        var name = GetElementName(row, col);
        sudoku[row, col] = ch == '.' ? new VariableInteger(name, 1, 9) : new VariableInteger(name, int.Parse(ch.ToString()), int.Parse(ch.ToString()));
      }
    }

    return sudoku;
  }

  private static string GetElementName(int row, int col)
  {
    return $"s{row}{col}";
  }

  private static void DumpBoard(IDictionary<string, IVariable<int>> sudoku)
  {
    for (var row = 0; row < 9; row++)
    {
      for (var col = 0; col < 9; col++)
      {
        var name = GetElementName(row, col);
        var val = sudoku[name];
        Console.Write($"{val.InstantiatedValue} ");
      }

      Console.WriteLine();
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

  private static (bool, StateInteger) Solve(VariableInteger[,] board)
  {
    if (board == null || board.Length == 0)
    {
      return (false, null);
    }

    var variables = new List<VariableInteger>();
    for (var row = 0; row < board.GetLength(0); row++)
    {
      var rowInts = board.SliceRow(row);
      variables.AddRange(rowInts);
    }

    var constraints = GetConstraints(board);
    var state = new StateInteger(variables, constraints);
    var searchResult = state.SearchAllSolutions();

    return (searchResult == StateOperationResult.Solved, state);
  }

  private static List<IConstraint> GetConstraints(VariableInteger[,] sudoku)
  {
    var rowBaseIdxs = new[] {0, 3, 6};
    var colBaseIdxs = new[] {0, 3, 6};

    var constraints = new List<IConstraint>();

    // 1. Each row must contain the numbers from 1 to 9, without repetitions
    for (var row = 0; row < sudoku.GetLength(0); row++)
    {
      var rowInts = sudoku.SliceRow(row);
      var allDiff = new AllDifferentInteger(rowInts);
      constraints.Add(allDiff);
    }

    // 2. Each column must contain the numbers from 1 to 9, without repetitions
    for (var col = 0; col < sudoku.GetLength(1); col++)
    {
      var colInts = sudoku.SliceCol(col);
      var allDiff = new AllDifferentInteger(colInts);
      constraints.Add(allDiff);
    }

    // 3. The digits can only occur once per 3x3 block (nonet)
    foreach (var rowBaseIdx in rowBaseIdxs)
    {
      foreach (var colBaseIdx in colBaseIdxs)
      {
        var allDiff = new AllDifferentInteger
        (
          new[]
          {
            sudoku[rowBaseIdx + 0, colBaseIdx + 0], sudoku[rowBaseIdx + 0, colBaseIdx + 1], sudoku[rowBaseIdx + 0, colBaseIdx + 2],
            sudoku[rowBaseIdx + 1, colBaseIdx + 0], sudoku[rowBaseIdx + 1, colBaseIdx + 1], sudoku[rowBaseIdx + 1, colBaseIdx + 2],
            sudoku[rowBaseIdx + 2, colBaseIdx + 0], sudoku[rowBaseIdx + 2, colBaseIdx + 1], sudoku[rowBaseIdx + 2, colBaseIdx + 2]
          }
        );
        constraints.Add(allDiff);
      }
    }

    // 4. The sum of every single row, column, and nonet must equal 45
    for (var row = 0; row < sudoku.GetLength(0); row++)
    {
      var rowInts = sudoku.SliceRow(row).ToList();
      var sum = new ConstraintInteger(rowInts[0] + rowInts[1] + rowInts[2] + rowInts[3] + rowInts[4] + rowInts[5] + rowInts[6] + rowInts[7] + rowInts[8] == 45);
      constraints.Add(sum);
    }

    for (var col = 0; col < sudoku.GetLength(1); col++)
    {
      var colInts = sudoku.SliceCol(col).ToList();
      var sum = new ConstraintInteger(colInts[0] + colInts[1] + colInts[2] + colInts[3] + colInts[4] + colInts[5] + colInts[6] + colInts[7] + colInts[8] == 45);
      constraints.Add(sum);
    }

    // sum in nonet must equal 45
    foreach (var rowBaseIdx in rowBaseIdxs)
    {
      foreach (var colBaseIdx in colBaseIdxs)
      {
        var sum = new ConstraintInteger
        (
          sudoku[rowBaseIdx + 0, colBaseIdx + 0] + sudoku[rowBaseIdx + 0, colBaseIdx + 1] + sudoku[rowBaseIdx + 0, colBaseIdx + 2] +
          sudoku[rowBaseIdx + 1, colBaseIdx + 0] + sudoku[rowBaseIdx + 1, colBaseIdx + 1] + sudoku[rowBaseIdx + 1, colBaseIdx + 2] +
          sudoku[rowBaseIdx + 2, colBaseIdx + 0] + sudoku[rowBaseIdx + 2, colBaseIdx + 1] + sudoku[rowBaseIdx + 2, colBaseIdx + 2] ==
          45
        );
        constraints.Add(sum);
      }
    }

    return constraints;
  }
}
