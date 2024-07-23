using System.Text.RegularExpressions;

namespace TicTacToe
{
    class Program
    {
        /// <summary>
        ///     The main entry point for the Tic Tac Toe application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        static void Main(string[] args)
        {
            char playerToMove = 'X';
            char[] gameMarkers = ['1', '2', '3', '4', '5', '6', '7', '8', '9'];

            int playerInput;
            int turnCount = 1;

            int[] winningCombination = new int[3];

            // Game loop
            do {
                // Draw game info
                HeadsUpDisplay(playerToMove);
                // Draw game board
                DrawGameBoard(gameMarkers);

                // Get player input
                while (true) {
                    playerInput = int.Parse(GetPlayerInput().ToString());
                    if (playerInput != 0) {
                        if (!IsValidNumber(gameMarkers[playerInput-1].ToString())) {
                            // Console.SetCursorPosition(0, Console.CursorTop - 1);
                            Console.WriteLine("Invalid input. That space is already taken.");
                            continue;
                        }
                        break;
                    }
                }
                // Set marker value to 'X' or 'O'
                gameMarkers[int.Parse(playerInput.ToString()) - 1] = playerToMove;

                if (IsGameOver(gameMarkers, playerToMove, ref winningCombination)) {
                    Console.Clear();
                    HeadsUpDisplay(playerToMove);
                    DrawGameBoard(gameMarkers, winningCombination);
                    Console.WriteLine($"Player {playerToMove} wins!");
                    break;
                }

                // Switch players 
                playerToMove = playerToMove == 'X' ? 'O' : 'X';
                turnCount++;
                Console.Clear();

            } while (turnCount < 9);

            if (turnCount > 9) {
                HeadsUpDisplay(playerToMove);
                DrawGameBoard(gameMarkers);
                Console.WriteLine("Game over. It's a draw!");   
            }      
        }

        /// <summary>
        ///     Determines if the game is over. If a victory condition is met, returns 'true' and saves the winning combination.
        /// </summary>
        /// <param name="gameMarkers">An array of characters representing the current state of the game board.</param>
        /// <param name="playerToMove">The character representing the player who is to make the next move ('X' or 'O').</param>
        /// <param name="winningCombination">An array of integers representing the indices of the winning combination.</param>
        /// <returns>True if the game is over, false otherwise.</returns>
        static bool IsGameOver(char[] gameMarkers, char playerToMove, ref int[] winningCombination) {
            
            // Return early if the active player has played less than three markers.
            if (gameMarkers.Count(ch => ch == playerToMove) < 3) return false;

            // All possible winning combinations
            int[][] winningCombinations = [
                [0, 1, 2],
                [3, 4, 5],
                [6, 7, 8],
                [0, 3, 6],
                [1, 4, 7],
                [2, 5, 8],
                [0, 4, 8],
                [2, 4, 6]
            ];

            foreach (int[] combination in winningCombinations) {
                if (gameMarkers[combination[0]] == playerToMove && 
                    gameMarkers[combination[1]] == playerToMove && 
                    gameMarkers[combination[2]] == playerToMove)
                {
                    winningCombination = [combination[0], combination[1], combination[2]];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Reads and validates player's input. If valid, returns the input as a character.
        /// </summary>
        /// <returns>The player's input as a character.</returns>
        static char GetPlayerInput() {
            // Reads and validates player input.

            string playerInput = Console.ReadLine();

            if (!IsValidNumber(playerInput)) {
                // Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine("Invalid input. Input a number between 1 and 9.");
                return '0';
            } 
            return playerInput[0];
        }

        /// <summary>
        ///     Validates if the player's input is a number between 1 and 9.
        /// </summary>
        /// <param name="playerInput">The player's input.</param>
        /// <returns>True if the input is a number between 1 and 9, false otherwise.</returns>
        private static bool IsValidNumber(string playerInput) {
            string pattern = "^[1-9]$";
            return Regex.IsMatch(playerInput, pattern);
        }

        /// <summary>
        ///     Displays the heads-up display (HUD) for the Tic Tac Toe game, showing the current player and prompting for their move.
        /// </summary>
        /// <param name="playerToMove">The character representing the player who is to make the next move ('X' or 'O').</param>
        static void HeadsUpDisplay(char playerToMove) {
            Console.Clear();

            Console.WriteLine("Welcome to Tic Tac Toe!");

            Console.WriteLine("Player 1: X");
            Console.WriteLine("Player 2: O");
            Console.WriteLine();

            Console.WriteLine($"Player {playerToMove} to move. Select a number between 1 and 9.");
            Console.WriteLine();
        }

        /// <summary>
        ///     Draws the game board to the console, with the winning combination highlighted in red.
        /// </summary>
        /// <param name="gameMarkers">An array of characters representing the current state of the game board.</param>
        static void DrawGameBoard(char[] gameMarkers) {
            // Draws game board with no winning combination highlighted
            Console.WriteLine($" {gameMarkers[0]} | {gameMarkers[1]} | {gameMarkers[2]} ");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {gameMarkers[3]} | {gameMarkers[4]} | {gameMarkers[5]} ");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {gameMarkers[6]} | {gameMarkers[7]} | {gameMarkers[8]} ");
            Console.WriteLine();
        }

        /// <summary>
        ///     Draws the game board to the console, with the winning combination highlighted in red.
        /// </summary>
        /// <param name="gameMarkers">An array of characters representing the current state of the game board.</param>
        /// <param name="winningCombination">An array of integers representing the indices of the winning combination.</param>
        static void DrawGameBoard(char[] gameMarkers, int[] winningCombination) {
            string outString = 
                $" {gameMarkers[0]} | {gameMarkers[1]} | {gameMarkers[2]} \n" + 
                "---+---+---\n" +
                $" {gameMarkers[3]} | {gameMarkers[4]} | {gameMarkers[5]} \n" +
                "---+---+---\n" +
                $" {gameMarkers[6]} | {gameMarkers[7]} | {gameMarkers[8]} \n";

            // Indices of the markers (i.e. 1-9) in the outString.
            int[] markerIndices = [1, 5, 9, 25, 29, 33, 49, 53, 57];
            // Indices of the markers in the winning combination.
            int[] winningIndices = markerIndices.Where((val, idx) => winningCombination.Contains(idx)).ToArray();

            ConsoleColor originalColor = Console.ForegroundColor;

            for (int i = 0; i < outString.Length; i++) {
                if (Array.Exists(winningIndices, element => element == i)) {
                    // Highlight marker in red.
                    Console.ForegroundColor = ConsoleColor.Red;
                } else {
                    // Reset to the original colour.
                    Console.ForegroundColor = originalColor;
                }
                Console.Write(outString[i]);
            }
            Console.WriteLine();
        }
    }
}