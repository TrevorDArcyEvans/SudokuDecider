namespace SudokuDecider;

using System.Collections.Generic;

public static class ArrayExtensions
{
  public static IEnumerable<T> SliceRow<T>(this T[,] matrix, int row)
  {
    for (var i = 0; i < matrix.GetLength(0); i++)
    {
      yield return matrix[row, i];
    }
  }

  public static IEnumerable<T> SliceCol<T>(this T[,] matrix, int col)
  {
    for (var i = 0; i < matrix.GetLength(1); i++)
    {
      yield return matrix[i, col];
    }
  }
}
