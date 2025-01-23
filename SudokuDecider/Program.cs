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
    var sudoku = new VariableInteger[9, 9];
    for (var i = 0; i < 9; i++)
    {
      var line = lines[i].Replace(" ", "").ToCharArray();
      for (var j = 0; j < 9; j++)
      {
        var ch = line[j];
        var name = $"s{i}{j}";
        sudoku[i, j] = ch == '.' ? new VariableInteger(name, 1, 9) : new VariableInteger(name, int.Parse(ch.ToString()), int.Parse(ch.ToString()));
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

    var variables = new List<VariableInteger>();
    for (var row = 0; row < board.GetLength(0); row++)
    {
      var rowInts = board.SliceRow(row);
      variables.AddRange(rowInts);
    }

    var constraints = GetConstraints(board);
    var state = new StateInteger(variables, constraints);
    var searchResult = state.Search();

    return searchResult == StateOperationResult.Solved;
  }

  private static List<IConstraint> GetConstraints(VariableInteger[,] sudoku)
  {
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
    // TODO   numbers in nonet unique

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

    // TODO   sum in nonet must equal 45

    return constraints;
  }
}
