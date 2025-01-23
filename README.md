# Sudoku Solver

A less straightforward sudoku solver in C#

```text
5 3 . . 7 . . . . 
6 . . 1 9 5 . . . 
. 9 8 . . . . 6 . 
8 . . . 6 . . . 3 
4 . . 8 . 3 . . 1 
7 . . . 2 . . . 6 
. 6 . . . . 2 8 . 
. . . 4 1 9 . . 5 
. . . . 8 . . 7 9 

Attempting solution...

5 3 4 6 7 8 9 1 2 
6 7 2 1 9 5 3 4 8 
1 9 8 3 4 2 5 6 7 
8 5 9 7 6 1 4 2 3 
4 2 6 8 5 3 7 9 1 
7 1 3 9 2 4 8 5 6 
9 6 1 5 3 7 2 8 4 
2 8 7 4 1 9 6 3 5 
3 4 5 2 8 6 1 7 9 
```

## [Rules of Sudoku](https://www.sudokuonline.io/tips/sudoku-rules)

1. Each row must contain the numbers from 1 to 9, without repetitions
2. Each column must contain the numbers from 1 to 9, without repetitions
3. The digits can only occur once per 3x3 block (nonet)
4. The sum of every single row, column, and nonet must equal 45

## Prerequisites

* .NET 9 SDK

## Getting started

```bash
# clone repo
git clone https://github.com/TrevorDArcyEvans/SudokuDecider.git

# build code
cd SudokuDecider
dotnet build

# run code
cd SudokuDecider
dotnet run
```
