using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static int n = 5; // Rows
    static int m = 5; // Columns

    static int speed = 150; //Spped

    static int totoRow = 0;
    static int totoCol = 0;
    static int allyRow = 0;
    static int allyCol = 0;
    static char[,] maze = new char[n, m];

    static bool TotoMoved;

    static char[,] minefield = {
    { 'X', '.', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' },
    { 'X', '.', '.', 'X', '.', 'X', '.', '.', '.', '.', '.', '.', '.', '.', 'X', 'X', 'X', 'X', 'X', 'X' },
    { 'X', '.', 'X', 'X', '.', 'X', '.', 'X', 'X', 'X', 'X', 'X', 'X', 'X', '.', '.', '.', '.', '.', 'X' },
    { 'X', '.', 'X', '.', '.', '.', '.', 'X', '.', '.', '.', 'X', '.', 'X', '.', 'X', 'X', 'X', '.', 'X' },
    { 'X', '.', '.', '.', 'X', '.', 'X', 'X', '.', 'X', '.', 'X', '.', 'X', '.', 'X', '.', '.', '.', 'X' },
    { 'X', '.', 'X', '.', 'X', '.', '.', 'X', '.', 'X', '.', 'X', '.', 'X', '.', 'X', 'X', 'X', '.', 'X' },
    { 'X', '.', 'X', '.', 'X', '.', 'X', 'X', '.', 'X', '.', '.', '.', 'X', '.', '.', '.', 'X', '.', 'X' },
    { 'X', '.', 'X', 'X', 'X', '.', '.', 'X', '.', 'X', 'X', 'X', 'X', 'X', 'X', '.', 'X', 'X', '.', 'X' },
    { 'X', '.', '.', '.', 'X', '.', 'X', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', 'X', '.', 'X' },
    { 'X', 'X', 'X', 'X', 'X', '.', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', '.', 'X', '.', 'X' },
    { 'X', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', 'X', '.', 'X', '.', 'X' },
    { 'X', '.', '.', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', '.', 'X', '.', 'X', '.', 'X' },
    { 'X', '.', '.', '.', '.', 'X', '.', '.', '.', '.', '.', '.', '.', '.', '.', 'X', '.', 'X', '.', 'X' },
    { 'X', '.', '.', '.', '.', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', '.', 'X', '.', 'X', '.', 'X' },
    { 'X', '.', '.', '.', '.', 'X', '.', '.', '.', '.', '.', '.', '.', '.', '.', 'X', '.', 'X', '.', 'X' },
    { 'X', '.', '.', '.', '.', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', '.', 'X', '.', 'X', '.', 'X' },
    { 'X', '.', '.', '.', '.', 'X', '.', '.', '.', '.', '.', '.', '.', '.', '.', 'X', '.', 'X', '.', 'X' },
    { 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', '.', 'X' },
    { 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', '.', 'X' },
    { 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', '.', 'X' },

};

    static List<Tuple<int, int>> totoPath = new List<Tuple<int, int>>();
    static List<Tuple<int, int>> availableMoves = new List<Tuple<int, int>>();
    static List<Tuple<int, int>> reversedMoves = new List<Tuple<int, int>>();

    static void Main()
    {
        Console.CursorVisible = false;


        maze = GenerateMaze();
        //InitializeMaze();

        FindAndInitializeTotoshka();

        PrintMinefield();
        Thread.Sleep(200);

        while (true)
        {
            MoveTotoshkaAndPrint();
        }
    }

    static void InitializeMaze()
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                maze[i, j] = minefield[i, j];
            }
        }
    }

    static void FindAndInitializeTotoshka()
    {
        for (int j = 0; j < m; j++)
        {
            if (maze[0, j] == '.')
            {
                totoCol = j;
                MoveTotoshka(0, totoCol);
                totoPath.Add(new Tuple<int, int>(0, totoCol));
                break;
            }
        }
    }

    static Tuple<int, int> GetFirstAvailableMove(int row, int col)
    {
        int[,] directions = {
        { 1, 0 },     // Down
        { 1, 1 },     // Down-Right
        { 1, -1 },    // Down-Left
        { 0, 1 },     // Right
        { -1, 1 },    // Up-Right
        { -1, 0 },    // Up
        { -1, -1 },   // Up-Left
        { 0, -1 }     // Left
    };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newRow = row + directions[i, 0];
            int newCol = col + directions[i, 1];

            if (newRow >= 0 && newRow < n && newCol >= 0 && newCol < m && maze[newRow, newCol] == '.' && IsValidMove(newRow, newCol))
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        int surroundRow = newRow + j;
                        int surroundCol = newCol + k;

                        if (surroundRow >= 0 && surroundRow < n && surroundCol >= 0 && surroundCol < m &&
                            maze[surroundRow, surroundCol] == '.' &&
                            !availableMoves.Contains(new Tuple<int, int>(surroundRow, surroundCol)))
                        {
                            availableMoves.Add(new Tuple<int, int>(surroundRow, surroundCol));
                        }
                    }
                }

                availableMoves.Remove(new Tuple<int, int>(newRow, newCol));

                return new Tuple<int, int>(newRow, newCol);
            }
        }

        return null;
    }

    static void MoveTotoshkaAndPrint()
    {
        if (totoRow == n - 1)
        {
            maze[totoRow, totoCol] = '.';
        }
        else
        {
            Console.WriteLine($"Current Totoshka position: ({totoRow}, {totoCol})");

            Tuple<int, int> firstAvailableMove = GetFirstAvailableMove(totoRow, totoCol);

            if (firstAvailableMove != null)
            {
                Console.WriteLine("Trying to move Totoshka:");

                int newRow = firstAvailableMove.Item1;
                int newCol = firstAvailableMove.Item2;

                Console.WriteLine($"  To ({newRow}, {newCol})");

                if (IsValidMove(newRow, newCol))
                {
                    Console.WriteLine($"Moving Totoshka to ({newRow}, {newCol})");
                    MoveTotoshka(newRow, newCol);
                    TotoMoved = true;
                }
                else
                {
                }
            }
            else
            {
                TotoMoved = false;
                HandleStuckTotoshka();
            }
        }
        if (TotoMoved)
        {
            MoveAlly();
        }
        PrintMinefield();
        Thread.Sleep(speed);
        Console.WriteLine($"Totoshka Path: {string.Join(" -> ", totoPath)}");
    }

    static void HandleStuckTotoshka()
    {
        Console.WriteLine("Totoshka is stuck. Reversing moves...");

        MoveAllyReversed(totoPath[totoPath.Count - 1].Item1, totoPath[totoPath.Count - 1].Item2);

        for (int i = totoPath.Count - 2; i >= 0; i--)
        {
            Tuple<int, int> previousPosition = totoPath[i];
            int reverseRow = previousPosition.Item1;
            int reverseCol = previousPosition.Item2;
            Tuple<int, int> surroundedPosition = IsPositionSurrounded(reverseRow, reverseCol);

            if (!reversedMoves.Contains(surroundedPosition))
            {
                Console.WriteLine($"Reversing to ({reverseRow}, {reverseCol})");

                Tuple<int, int> previousAllyPosition = totoPath[i + 1];
                int allyCurrentRow = previousAllyPosition.Item1;
                int allyCurrentCol = previousAllyPosition.Item2;

                MoveTotoshka(reverseRow, reverseCol);

                MoveAllyReversed(allyCurrentRow, allyCurrentCol);

                PrintMinefield();
                Thread.Sleep(speed);

                reversedMoves.Add(surroundedPosition);

                if (surroundedPosition != null)
                {

                    MoveTotoshka(surroundedPosition.Item1, surroundedPosition.Item2);
                    MoveAllyReversed(reverseRow, reverseCol);
                    availableMoves.Remove(new Tuple<int, int>(surroundedPosition.Item1, surroundedPosition.Item2));
                    maze[allyCurrentRow, allyCurrentCol] = '.';

                    break; 
                }
                else
                {
                    Console.WriteLine("Move is in reversedMoves. Skipping move.");
                }
            }
        }
        Console.WriteLine("Moves reversed. Resuming the simulation.");
    }

    static Tuple<int, int> IsPositionSurrounded(int row, int col)
    {
        if (availableMoves.Contains(new Tuple<int, int>(row - 1, col)))
        {
            return new Tuple<int, int>(row - 1, col);
        }
        else if (availableMoves.Contains(new Tuple<int, int>(row + 1, col)))
        {
            return new Tuple<int, int>(row + 1, col);
        }
        else if (availableMoves.Contains(new Tuple<int, int>(row, col - 1)))
        {
            return new Tuple<int, int>(row, col - 1);
        }
        else if (availableMoves.Contains(new Tuple<int, int>(row, col + 1)))
        {
            return new Tuple<int, int>(row, col + 1);
        }
        else if (availableMoves.Contains(new Tuple<int, int>(row - 1, col - 1)))
        {
            return new Tuple<int, int>(row - 1, col - 1);
        }
        else if (availableMoves.Contains(new Tuple<int, int>(row - 1, col + 1)))
        {
            return new Tuple<int, int>(row - 1, col + 1);
        }
        else if (availableMoves.Contains(new Tuple<int, int>(row + 1, col - 1)))
        {
            return new Tuple<int, int>(row + 1, col - 1);
        }
        else if (availableMoves.Contains(new Tuple<int, int>(row + 1, col + 1)))
        {
            return new Tuple<int, int>(row + 1, col + 1);
        }

        return null;
    }

    static void MoveAllyReversed(int newRow, int newCol)
    {
        maze[allyRow, allyCol] = '.';
        allyRow = newRow;
        allyCol = newCol;
        maze[allyRow, allyCol] = 'A';
    }

    static bool IsValidMove(int row, int col)
    {
        if (row < 0 || row >= n || col < 0 || col >= m || maze[row, col] != '.' || totoPath.Contains(new Tuple<int, int>(row, col)))
        {
            return false;
        }

        return true;
    }

    static void PrintMinefield()
    {
        Console.Clear();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (i == totoRow && j == totoCol && totoRow != n - 1)
                {
                    Console.Write("T ");
                    Tuple<int, int> currentPosition = new Tuple<int, int>(totoRow, totoCol);

                    if (!totoPath.Contains(currentPosition))
                    {
                        totoPath.Add(currentPosition);
                    }
                }
                else
                {
                    Console.Write($"{maze[i, j]} ");
                }
            }
            Console.WriteLine();
        }
    }

    static void MoveAlly()
    {
        Tuple<int, int> lastTotoPosition = totoPath[totoPath.Count - 1];


        if (totoPath.Count > 0)
        {

            if (totoPath.Count > 1 && TotoMoved)
            {
                maze[allyRow, allyCol] = '.';
            }

            if (allyRow == n - 1)
            {
                maze[allyRow, allyCol] = '.';
            }
            else if (totoRow == n - 1)
            {
                allyRow = totoRow;
                allyCol = totoCol;
                maze[totoRow, totoCol] = 'A';
            }
            else
            {
                allyRow = lastTotoPosition.Item1;
                allyCol = lastTotoPosition.Item2;
                maze[allyRow, allyCol] = 'A';
            }
        }
    }

    static void MoveTotoshka(int newRow, int newCol)
    {

        maze[totoRow, totoCol] = '.';
        totoRow = newRow;
        totoCol = newCol;
        maze[totoRow, totoCol] = 'T';
    }

    static char[,] GenerateMaze()
    {
        List<Tuple<int, int>> pathCoordinates = new List<Tuple<int, int>>();

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                maze[i, j] = ' ';
            }
        }

        int startColumn = new Random().Next(m);
        maze[0, startColumn] = '.';
        pathCoordinates.Add(new Tuple<int, int>(0, startColumn));

        GeneratePath(maze, pathCoordinates, 0, startColumn, n - 1);

        for (int i = 1; i < n; i++)
        {
            int lastColumn = pathCoordinates[pathCoordinates.Count - 1].Item2;
            int diff = lastColumn - startColumn;

            if (diff == 0)
            {
                maze[i, lastColumn] = '.';
                pathCoordinates.Add(new Tuple<int, int>(i, lastColumn));
            }
            else if (diff > 0)
            {
                for (int j = lastColumn - 1; j >= lastColumn - diff; j--)
                {
                    maze[i, j] = '.';
                    pathCoordinates.Add(new Tuple<int, int>(i, j));
                }
            }
            else
            {
                for (int j = lastColumn + 1; j <= lastColumn - diff; j++)
                {
                    maze[i, j] = '.';
                    pathCoordinates.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        SurroundPathWithX(maze, pathCoordinates);

        return maze;
    }

    static void GeneratePath(char[,] maze, List<Tuple<int, int>> pathCoordinates, int currentRow, int currentColumn, int endRow)
    {
        Random random = new Random();
        int maxIterations = 1000;

        int iterations = 0;
        while (currentRow < endRow && iterations < maxIterations)
        {
            int moveRow, moveCol;

            do
            {
                moveRow = random.Next(3) - 1;
                moveCol = random.Next(3) - 1;
            } while (moveRow == 0 && moveCol == 0);

            int newRow = currentRow + moveRow;
            int newColumn = currentColumn + moveCol;

            if (newRow >= 0 && newRow < n && newColumn >= 0 && newColumn < m && maze[newRow, newColumn] == ' ')
            {
                maze[newRow, newColumn] = '.';
                pathCoordinates.Add(new Tuple<int, int>(newRow, newColumn));
                currentRow = newRow;
                currentColumn = newColumn;
            }

            iterations++;
        }
    }

    static void SurroundPathWithX(char[,] maze, List<Tuple<int, int>> pathCoordinates)
    {
        foreach (var coordinate in pathCoordinates)
        {
            int row = coordinate.Item1;
            int col = coordinate.Item2;

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < n && j >= 0 && j < m && maze[i, j] == ' ')
                    {
                        maze[i, j] = 'X';
                    }
                }
            }
        }
    }
}