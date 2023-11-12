using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static int n = 5; // Rows
    static int m = 5; // Columns

    static int totoRow = 0;
    static int totoCol = 0;
    static int allyRow = 0;
    static int allyCol = 0;
    static char[,] maze = new char[n, m];

    static char[,] minefield = {
    { 'X', '.', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X' },
    { 'X', '.', '.', 'X', '.', 'X', '.', '.', '.', '.', '.', '.', '.', 'X', 'X', 'X', 'X', 'X', 'X', 'X' },
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
    { 'X', '.', '.', '.', '.', 'X', '.', '.', '.', '.', '.', '.', '.', 'X', '.', 'X', '.', 'X', '.', 'X' },
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

    //Constant Maze
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

    static List<Tuple<int, int>> GetAvailableMoves(int row, int col, List<Tuple<int, int>> visitedPositions)
    {
        int[,] directions = {
        { 1, 0 },     // Down
        { 1, 1 },     // Down-Right
        { 1, -1 },    // Down-Left
        { 0, 1 },     // Right
        { -1, 0 },    // Up
        { 0, -1 } ,    // Left
        { -1, 1 },    // Up-Right
        { -1, -1 }   // Up-Left
   
    };

        List<Tuple<int, int>> availableMoves = new List<Tuple<int, int>>();

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newRow = row + directions[i, 0];
            int newCol = col + directions[i, 1];

            // Check if the new position is within bounds, is a '.', and has not been visited before
            if (newRow >= 0 && newRow < n && newCol >= 0 && newCol < m &&
                maze[newRow, newCol] == '.' && !visitedPositions.Contains(new Tuple<int, int>(newRow, newCol)))
            {
                availableMoves.Add(new Tuple<int, int>(newRow, newCol));
            }
        }

        return availableMoves;
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

            // Get all available moves excluding visited positions
            List<Tuple<int, int>> availableMoves = GetAvailableMoves(totoRow, totoCol, totoPath);

            if (availableMoves.Count > 0)
            {
                Console.WriteLine("Trying to move Totoshka:");

                // Track whether Totoshka has moved in this iteration
                bool hasMoved = false;

                foreach (var move in availableMoves)
                {
                    int newRow = move.Item1;
                    int newCol = move.Item2;

                    Console.WriteLine($"  To ({newRow}, {newCol})");

                    if (IsValidMove(newRow, newCol))
                    {
                        Console.WriteLine($"Moving Totoshka to ({newRow}, {newCol})");
                        MoveTotoshka(newRow, newCol);
                        hasMoved = true;

                        // Remove the position from totoPath
                        // Tuple<int, int> movedPosition = totoPath.Find(p => p.Item1 == newRow && p.Item2 == newCol);
                        // if (movedPosition != null)
                        // {
                        //     totoPath.Remove(movedPosition);
                        // }

                        break;  // Stop after making a valid move
                    }
                    else
                    {
                        Console.WriteLine($"Invalid move to ({newRow}, {newCol})");
                    }
                }

                // If Totoshka hasn't moved, it means it's surrounded; trigger animation
                if (!hasMoved)
                {

                    MoveTotoshkaToRemaining();

                }
            }
            else
            {
                // If no available move, trigger animation
                MoveTotoshkaToRemaining();
            }
        }

        MoveAlly();
        PrintMinefield();
        Thread.Sleep(150);
        Console.WriteLine($"Totoshka Path: {string.Join(" -> ", totoPath)}");
    }

    static bool MoveTotoshkaToRemaining()
    {
        Console.WriteLine("Totoshka is surrounded! Animating to available move...");

        // Get new available moves based on the current position of "T"
        List<Tuple<int, int>> newAvailableMoves = GetAvailableMoves(totoRow, totoCol, totoPath);

        foreach (var newMove in newAvailableMoves)
        {
            int newRow = newMove.Item1;
            int newCol = newMove.Item2;

            Console.WriteLine($"Animating to ({newRow}, {newCol})");

            maze[totoRow, totoCol] = '.';
            totoRow = newRow;
            totoCol = newCol;
            maze[totoRow, totoCol] = 'T';

            PrintMinefield();
            Thread.Sleep(150);

            Console.WriteLine("Totoshka successfully moved to an available position!");

            // Remove the position from totoPath
            Tuple<int, int> movedPosition = totoPath.Find(p => p.Item1 == newRow && p.Item2 == newCol);
            if (movedPosition != null)
            {
                totoPath.Remove(movedPosition);
            }

            // Search for new unique positions after moving 'T'
            SearchForNewUniquePositions();

            // Check if "A" needs to move to its previous step
            if (IsTotoshkaSurroundedByAlly())
            {
                //MoveAllyToPreviousStep();
            }

            return true;
        }

        Console.WriteLine("No available move found. Totoshka remains surrounded!");
        return false;
    }



    static bool IsTotoshkaSurroundedByAlly()
    {
        // Check if "T" is surrounded by "X" and one of the surrounding positions contains "A"
        for (int i = totoRow - 1; i <= totoRow + 1; i++)
        {
            for (int j = totoCol - 1; j <= totoCol + 1; j++)
            {
                if (i >= 0 && i < n && j >= 0 && j < m)
                {
                    if (maze[i, j] == 'X')
                    {
                        // Check if one of the surrounding positions contains 'A'
                        for (int x = i - 1; x <= i + 1; x++)
                        {
                            for (int y = j - 1; y <= j + 1; y++)
                            {
                                if (x >= 0 && x < n && y >= 0 && y < m && maze[x, y] == 'A')
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }





    static void SearchForNewUniquePositions()
    {
        // Get the current position
        Tuple<int, int> currentPosition = new Tuple<int, int>(totoRow, totoCol);

        // Get all available moves excluding visited positions
        List<Tuple<int, int>> newAvailableMoves = GetAvailableMoves(totoRow, totoCol, totoPath);

        // Find new unique positions from the available moves
        List<Tuple<int, int>> newUniquePositions = newAvailableMoves.Except(totoPath).ToList();

        // Add the new unique positions to totoPath
        totoPath.AddRange(newUniquePositions);

        Console.WriteLine("Searching for new unique positions...");

        // Move Totoshka to the new position
        MoveTotoshka(newUniquePositions[0].Item1, newUniquePositions[0].Item2);

        // Print the minefield
        PrintMinefield();
        Thread.Sleep(150);
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
                    totoPath.Add(new Tuple<int, int>(totoRow, totoCol));
                }
                else
                    Console.Write($"{maze[i, j]} ");
            }
            Console.WriteLine();
        }
    }

    //static bool IsValidMove(int row, int col)
    //{
    //    if (row < 0 || row >= n || col < 0 || col >= m || maze[row, col] != '.')
    //    {
    //        return false;
    //    }

    //    // Check if the position is within bounds, is a '.' in the maze, and not the previous step of "T"
    //    if (row >= 0 && row < n && col >= 0 && col < m && maze[row, col] == '.')
    //    {
    //        return true;
    //    }

    //    return false;
    //}


    static bool IsValidMove(int row, int col)
    {
        if (row < 0 || row >= n || col < 0 || col >= m || maze[row, col] != '.')
        {
            return false;
        }

        // Check if the surrounding positions contain 'X'
        bool isSurroundedByX = true;
        for (int i = row - 1; i <= row + 1; i++)
        {
            for (int j = col - 1; j <= col + 1; j++)
            {
                if (i >= 0 && i < n && j >= 0 && j < m && maze[i, j] != 'X')
                {
                    isSurroundedByX = false;
                    break;
                }
            }
        }

        if (isSurroundedByX)
        {
            // Check if one of the surrounding positions contains 'A'
            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = col - 1; j <= col + 1; j++)
                {
                    if (i >= 0 && i < n && j >= 0 && j < m && maze[i, j] == 'A')
                    {
                        // Call a method to move 'A' to its previous step
                        //MoveAllyToPreviousStep();
                    }
                }
            }
        }

        return true;
    }





    static void MoveTotoshka(int newRow, int newCol)
    {
        if (IsValidMove(newRow, newCol))
        {
            maze[totoRow, totoCol] = '.';
            totoRow = newRow;
            totoCol = newCol;
            maze[totoRow, totoCol] = 'T';
        }
    }


    static void MoveAlly()
    {
        //Tuple<int, int> lastTotoPosition = totoPath[totoPath.Count - 1];
        //allyRow = lastTotoPosition.Item1;
        //allyCol = lastTotoPosition.Item2;

        //if (totoPath.Count > 11)
        //{
        //    Console.WriteLine("Totoshka is surrounded by Ally! Moving Ally to previous step.");
        //    //MoveAllyToPreviousStep();
        //}

        if (totoPath.Count > 0)
        {
            if (totoPath.Count > 2 )
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
                Tuple<int, int> lastTotoPosition = totoPath[totoPath.Count - 1];
                allyRow = lastTotoPosition.Item1;
                allyCol = lastTotoPosition.Item2;
                maze[allyRow, allyCol] = 'A';
            }
        }
    }

    static void MoveAllyToPreviousStep()
    {
        while (totoPath.Count > 1)
        {
            // Get the last position of 'T'
            Tuple<int, int> lastTotoPosition = totoPath[totoPath.Count - 1];
            int totoRow = lastTotoPosition.Item1;
            int totoCol = lastTotoPosition.Item2;

            // Clear the current position of 'A'
            maze[allyRow, allyCol] = '.';

            // Move 'A' to the previous step
            allyRow = totoRow;
            allyCol = totoCol;
            maze[allyRow, allyCol] = 'A';

            Console.WriteLine("Moving 'A' to its previous step.");

            // Print the minefield
            PrintMinefield();
            Thread.Sleep(150);

            // Check if 'T' found a new available move
            if (MoveTotoshkaToRemaining())
            {
                // 'T' found a new move, exit the loop
                break;
            }
        }

        Console.WriteLine("No previous step available for 'A' or 'T' remains surrounded.");
    }







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