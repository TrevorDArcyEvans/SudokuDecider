namespace SudokuDecider;

using System;
using System.IO;

public static class Program
{
  public static void Main(string[] args)
  {
    var lines = File.ReadAllLines(args[0]);
    var sudoku = new char[9, 9];
    for (var i = 0; i < 9; i++)
    {
      var line = lines[i].Replace(" ", "").ToCharArray();
      for (var j = 0; j < 9; j++)
      {
        sudoku[i, j] = line[j];
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

  private static void DumpBoard(char[,] sudoku)
  {
    for (var i = 0; i < sudoku.GetLength(0); i++)
    {
      for (var j = 0; j < sudoku.GetLength(1); j++)
      {
        Console.Write($"{sudoku[i, j]} ");
      }

      Console.WriteLine();
    }
  }

  private static bool Solve(char[,] board)
  {
    if (board == null || board.Length == 0)
    {
      return false;
    }

    return true;
  }
}
