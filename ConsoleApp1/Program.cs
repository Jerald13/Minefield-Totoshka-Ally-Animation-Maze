using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static int n = 20; // Rows
    static int m = 20; // Columns

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


    // Lists to track the path taken by Totoshka and available moves around Totoshka
    static List<Tuple<int, int>> totoPath = new List<Tuple<int, int>>();
    static List<Tuple<int, int>> availableMoves = new List<Tuple<int, int>>();
    static List<Tuple<int, int>> reversedMoves = new List<Tuple<int, int>>();

    static void Main()
    {
        Console.CursorVisible = false;

        // Initialize the maze with the minefield

        //maze = GenerateMaze();
        InitializeMaze();

        // Find a valid starting position for "T" in the first row
        FindAndInitializeTotoshka();

        // Display the initial minefield
        PrintMinefield();
        Thread.Sleep(200);

        // Main loop for moving Totoshka and Ally
        while (true)
        {
            MoveTotoshkaAndPrint();
        }
    }

    // Constant Maze
    static void InitializeMaze()
    {
        // Copy the minefield to the maze
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
        // Find a valid starting position for "T" in the first row
        for (int j = 0; j < m; j++)
        {
            if (maze[0, j] == '.')
            {
                totoCol = j;
                MoveTotoshka(0, totoCol); // Initialize "T" at the valid starting position
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

        //{ 0, -1 } ,    // Left
        //{ 1, -1 },    // Down-Left
        //{ 0, 1 },     // Right
        //{ -1, 1 },    // Up-Right
        //{ 1, 1 },     // Down-Right
        //{ -1, -1 },   // Up-Left
        //{ -1, 0 },    // Up
        //{ 1, 0 },     // Down

    };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newRow = row + directions[i, 0];
            int newCol = col + directions[i, 1];

            if (newRow >= 0 && newRow < n && newCol >= 0 && newCol < m && maze[newRow, newCol] == '.' && IsValidMove(newRow, newCol))
            {
                // Check the surrounding positions for available moves
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
                            // Add the surrounding available move to the global list
                            availableMoves.Add(new Tuple<int, int>(surroundRow, surroundCol));
                        }
                    }
                }

                // Remove the position from availableMoves
                availableMoves.Remove(new Tuple<int, int>(newRow, newCol));

                // Return the first available move
                return new Tuple<int, int>(newRow, newCol);
            }
        }

        // Return null or some default value if no valid move is found
        return null;
    }






    static void MoveTotoshkaAndPrint()
    {
        // If "T" reaches the last row, clear its position in the maze
        if (totoRow == n - 1)
        {
            maze[totoRow, totoCol] = '.';
        }
        else
        {
            Console.WriteLine($"Current Totoshka position: ({totoRow}, {totoCol})");

            // Get the first available move excluding visited positions
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

        // Move Ally and display the updated minefield
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

        // Move Ally to the last position of T
        MoveAllyReversed(totoPath[totoPath.Count - 1].Item1, totoPath[totoPath.Count - 1].Item2);

        for (int i = totoPath.Count - 2; i >= 0; i--)
        {
            Tuple<int, int> previousPosition = totoPath[i];
            int reverseRow = previousPosition.Item1;
            int reverseCol = previousPosition.Item2;
            // Check if the move is not in reversedMoves before allowing Totoshka to move
            Tuple<int, int> surroundedPosition = IsPositionSurrounded(reverseRow, reverseCol);

            if (!reversedMoves.Contains(surroundedPosition))
            {


                Console.WriteLine($"Reversing to ({reverseRow}, {reverseCol})");

                // Store ally's current position
                Tuple<int, int> previousAllyPosition = totoPath[i + 1];
                int allyCurrentRow = previousAllyPosition.Item1;
                int allyCurrentCol = previousAllyPosition.Item2;

                MoveTotoshka(reverseRow, reverseCol);

                // Move Ally to the reversed position
                MoveAllyReversed(allyCurrentRow, allyCurrentCol);

                PrintMinefield();
                Thread.Sleep(speed);

                // Check if the reversed position is surrounded by available moves
                reversedMoves.Add(surroundedPosition);

                if (surroundedPosition != null)
                {

                    MoveTotoshka(surroundedPosition.Item1, surroundedPosition.Item2);
                    MoveAllyReversed(reverseRow, reverseCol);
                    availableMoves.Remove(new Tuple<int, int>(surroundedPosition.Item1, surroundedPosition.Item2));
                    maze[allyCurrentRow, allyCurrentCol] = '.';

                    // Add the move to reversedMoves
                    break; // End the function loop

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

        // Return null if the position is not surrounded
        return null;
    }



    static void MoveAllyReversed(int newRow, int newCol)
    {
        maze[allyRow, allyCol] = '.';
        allyRow = newRow;
        allyCol = newCol;
        maze[allyRow, allyCol] = 'A';
    }



    //static bool HasSurroundingAvailableMoves(int row, int col)
    //{
    //    // Check if there are available moves in the surrounding positions
    //    List<Tuple<int, int>> surroundingMoves = GetAvailableMoves(row, col, totoPath);

    //    foreach (var move in surroundingMoves)
    //    {
    //        int newRow = move.Item1;
    //        int newCol = move.Item2;

    //        if (maze[newRow, newCol] == '.')
    //        {
    //            // Found an available move in the surrounding positions
    //            return true;
    //        }
    //    }

    //    // No available move in the surrounding positions
    //    return false;
    //}





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

                    // Check if the position is not already in totoPath before adding
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
            //else if (IsTotoshkaSurroundedByAlly())
            //{
            //    Console.WriteLine("Totoshka is surrounded by Ally! Moving Ally to previous step.");
            //    MoveAllyToPreviousStep();
            //}
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




    //static void MoveAllyToPreviousStep()
    //{
    //    while (totoPath.Count > 1)
    //    {
    //        // Get the last position of 'T'
    //        Tuple<int, int> lastTotoPosition = totoPath[totoPath.Count - 1];
    //        int totoRow = lastTotoPosition.Item1;
    //        int totoCol = lastTotoPosition.Item2;

    //        // Clear the current position of 'A'
    //        maze[allyRow, allyCol] = '.';

    //        // Move 'A' to the previous step
    //        allyRow = totoRow;
    //        allyCol = totoCol;
    //        maze[allyRow, allyCol] = 'A';

    //        Console.WriteLine("Moving 'A' to its previous step.");

    //        // Print the minefield
    //        PrintMinefield();
    //        Thread.Sleep(150);

    //        // Check if 'T' found a new available move
    //        if (MoveTotoshkaToRemaining())
    //        {
    //            // 'T' found a new move, exit the loop
    //            break;
    //        }
    //    }

    //    Console.WriteLine("No previous step available for 'A' or 'T' remains surrounded.");
    //}







    static char[,] GenerateMaze()
    {
        List<Tuple<int, int>> pathCoordinates = new List<Tuple<int, int>>();

        // Initialize the maze with empty spaces
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                maze[i, j] = ' ';
            }
        }

        // Randomly choose the starting position in the first row
        int startColumn = new Random().Next(m);
        maze[0, startColumn] = '.';
        pathCoordinates.Add(new Tuple<int, int>(0, startColumn));

        // Generate the path from the starting position to the ending position
        GeneratePath(maze, pathCoordinates, 0, startColumn, n - 1);

        // Ensure a clear connection to the end of the fifth row
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

        // Surround the path with "X"
        SurroundPathWithX(maze, pathCoordinates);

        return maze;
    }


    static void GeneratePath(char[,] maze, List<Tuple<int, int>> pathCoordinates, int currentRow, int currentColumn, int endRow)
    {
        Random random = new Random();
        int maxIterations = 1000; // Set a maximum number of iterations to avoid an infinite loop

        int iterations = 0;
        while (currentRow < endRow && iterations < maxIterations)
        {
            // Generate a random direction to move (up, down, left, right, or diagonals)
            int moveRow, moveCol;

            do
            {
                moveRow = random.Next(3) - 1;
                moveCol = random.Next(3) - 1;
            } while (moveRow == 0 && moveCol == 0);

            int newRow = currentRow + moveRow;
            int newColumn = currentColumn + moveCol;

            // Ensure the new position is within bounds and hasn't been visited before
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

            // Check and surround with "X" if there's an adjacent empty space
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