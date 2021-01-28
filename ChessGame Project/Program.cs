using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            ChessGame newgame = new ChessGame();
            newgame.play();


        }
    }
    class ChessGame
    {
        public void play()
        {
            ChessPiece[][,] boardHistory = new ChessPiece[50][,];
            int totalGameMoves = 0;
            string userInput = "";
            bool isInputInsertedCorrectly = false;
            bool isCorrectSoldierPicked = false;
            string chosenPieceBeforeMoving = "";
            string movingPiece = "";
            bool gameOn = true;
            bool threeTimesSameBoard = false;
            int fiftyMovesRoleAnyPieceEaten = 0;

            ChessPiece[,] newBoard = null;
            ChessPiece[,] board = { { new Rook('W') , new Knight('W'), new Bishop('W'), new Queen('W'), new King('W') , new Bishop('W'), new Knight('W'), new Rook('W') },
                                    { new Pawn('W') , new Pawn('W')  , new Pawn('W')  , new Pawn('W') , new Pawn('W') , new Pawn('W')  , new Pawn('W')  , new Pawn('W') },
                                    { null          , null           , null           , null          , null          , null           , null           , null          },
                                    { null          , null           , null           , null          , null          , null           , null           , null          },
                                    { null          , null           , null           , null          , null          , null           , null           , null          },
                                    { null          , null           , null           , null          , null          , null           , null           , null          },
                                    { new Pawn('B') , new Pawn('B')  , new Pawn('B')  , new Pawn('B') , new Pawn('B') , new Pawn('B')  , new Pawn('B')  , new Pawn('B') },
                                    { new Rook('B') , new Knight('B'), new Bishop('B'), new Queen('B'), new King('B') , new Bishop('B'), new Knight('B'), new Rook('B') }};


            /*ChessPiece[,] board = { { new Rook('W') , null           , null           , null          , null          , null           , null           , null          },
                                    { null          , null           , null           , null          , null          , null           , null           , null          },
                                    { null          , null           , null           , null          , null          , null           , null           , null          },
                                    { null          , null           , null           , null          , null          , null           , null           , null          },
                                    { new Pawn('W') , new Rook('B')  , null           , new Rook('B') , null          , null           , null           , null          },
                                    { new King('W') , null           , null           , null          , null          , null           , null           , null          },
                                    { null          , null           , new King('B')  , null          , null          , null           , null           , null          },
                                    { null          , null           , null           , null          , null          , null           , null           , null          }};*/
            chessGameInstructions();
            printBoard(board);
            while (gameOn)
            {
                while (!isCorrectSoldierPicked)
                {
                    while (!isInputInsertedCorrectly)
                    {
                        userInput = RequestFromUserPieceToMove(board, totalGameMoves);
                        isInputInsertedCorrectly = checkUserInput(userInput);
                    }
                    isInputInsertedCorrectly = false;
                    chosenPieceBeforeMoving = "" + userInput[0] + userInput[1];
                    movingPiece = "" + userInput[2] + userInput[3];
                    chosenPieceBeforeMoving = convertUserInputToIndex(board, chosenPieceBeforeMoving);
                    movingPiece = convertUserInputToIndex(board, movingPiece);
                    isCorrectSoldierPicked = checkWhoseTurn(chosenPieceBeforeMoving, totalGameMoves, board);
                }
                if (isMovePossible(board, chosenPieceBeforeMoving, movingPiece, totalGameMoves))
                {
                    int currentRow = chosenPieceBeforeMoving[1] - '0';
                    int currentColumn = chosenPieceBeforeMoving[0] - '0';
                    int nextRow = movingPiece[1] - '0';
                    int nextColumn = movingPiece[0] - '0';
                    if (isPiecePromoted(board, currentColumn, currentRow, nextColumn, nextRow))
                    {
                        totalGameMoves++;
                        isCorrectSoldierPicked = false;
                        continue;
                    }
                    if (board[nextColumn, nextRow] is Rook && board[nextColumn, nextRow].getColor() == board[currentColumn, currentRow].getColor())
                    {
                        makeCastleMove(board, currentRow, currentColumn, nextRow, nextColumn);
                        totalGameMoves++;
                        printBoard(board);
                        isCorrectSoldierPicked = false;
                        continue;
                    }
                    if (swapForCheck(board, nextColumn, nextRow, currentColumn, currentRow, totalGameMoves, board[currentColumn, currentRow].getColor()) is false)
                    {
                        newBoard = movingPieceInBoard(board, chosenPieceBeforeMoving, movingPiece, fiftyMovesRoleAnyPieceEaten);
                        if (totalGameMoves > 25)
                            if (isBoardLackOfPieces(newBoard))
                            {
                                Console.WriteLine("It's a tie");
                                break;
                            }
                        if(fiftyMovesRoleAnyPieceEaten > 0)
                            boardHistory = new ChessPiece[50][,];
                        boardHistory[totalGameMoves] = AddBoardToBoardHistory(newBoard, totalGameMoves);
                        threeTimesSameBoard = checkBoardThreeTimes(boardHistory, newBoard);
                        if (threeTimesSameBoard)
                        {
                            Console.WriteLine("It's a tie, three times same board");
                            break;
                        }
                        printBoard(newBoard);


                        string kingThreatned = isCheck(newBoard, totalGameMoves, 'E');
                        if (kingThreatned != "False")
                        {
                            int kingcolumn = kingThreatned[0] - '0';
                            int kingrow = kingThreatned[1] - '0';
                            if (isKingBlocked(newBoard, kingcolumn, kingrow, totalGameMoves))
                            {
                                if (isCheckMate(newBoard, kingcolumn, kingrow, totalGameMoves))
                                {
                                    checkMateAnnoucement(totalGameMoves);
                                    gameOn = false;
                                }
                                else
                                    checkAnnoucement();
                            }
                            else
                                checkAnnoucement();
                        }
                        else
                        {
                            if(isStaleMate(newBoard, totalGameMoves)) 
                            { 
                                Console.WriteLine("It's a tie"); 
                                break;
                            }
                        }
                        totalGameMoves++;
                    }
                    else
                        Console.WriteLine("Not possible, king will be in check");
                }
                isCorrectSoldierPicked = false;
            }
            while (true) { }
        }
        void chessGameInstructions()
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("-          This is a Chess Game         -");
            Console.WriteLine("-    To choose a piece from the board   -");
            Console.WriteLine("-   Choose the piece you want to move   -");
            Console.WriteLine("-  And and where you want to move it to -");
            Console.WriteLine("-            For Example: A2A3          -");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("                 GOOD LUCK!");
        } // Prints the game instructions
        void checkMateAnnoucement(int totalGameMoves)
        {
            string name = totalGameMoves % 2 == 0 ? "White" : "Black";
            Console.WriteLine("  -----------");
            Console.WriteLine("---CHECKMATE----");
            Console.WriteLine("  -----------");
            Console.WriteLine("{0} wins", name);
        } // Annoucement that there is checkmate
        void checkAnnoucement()
        {
            Console.WriteLine("  ------");
            Console.WriteLine("---CHECK----");
            Console.WriteLine("  ------");
        } // Annoucement that there is a check
        void printBoard(ChessPiece[,] board)
        {
            Console.WriteLine("");
            Console.WriteLine("  A   B   C   D   E   F   G   H");
            Console.WriteLine("+---+---+---+---+---+---+---+---+");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    Console.Write("|");
                    if (board[i, g] is null)
                        Console.Write("EE ");
                    else
                        Console.Write(board[i, g] + " ");
                }
                Console.Write("|   " + (i + 1));
                Console.WriteLine("");
                Console.WriteLine("+---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("");
        } // Prints the board
        string RequestFromUserPieceToMove(ChessPiece[,] board, int gameTotalMoves)
        {
            bool wantToMoveValid = false;
            string wantToMove = "";
            while (!wantToMoveValid)
            {
                if (gameTotalMoves % 2 == 0)         //Checking if its white's turn
                    Console.WriteLine("It is WHITE's turn\nWhich piece do you want to move and to where?:(ex: 'A1A2')");

                if (gameTotalMoves % 2 != 0)          // checking if its black's turn
                    Console.WriteLine("It is BLACK's turn\nWhich piece do you want to move and to where?:(ex: 'A1A2')");

                wantToMove = Console.ReadLine();
                if (wantToMove == "")
                    Console.WriteLine("You pressed ENTER without any number");        //If user pressed nothing, looping over again

                else if (wantToMove.Length != 4)
                    Console.WriteLine("Incorrect input, you need to enter which piece you want to move and to where"); // if user entered less than 4 letters, loop over again

                else
                    wantToMoveValid = true;      // If user didnt press enter without any value or less than 4 letters, then checking if the letters are legit
            }
            wantToMove.ToUpper();
            return wantToMove;
        } // Asks the user to choose a piece to move
        bool checkUserInput(string userInput)
        {
            string convertUserInputUpper = userInput.ToUpper();
            string availableLetters = "ABCDEFGH";
            string availableNumbers = "12345678";
            int countValidLettersNNumbers = 0;
            bool isLetterValid = false;
            for (int i = 0; i < userInput.Length; i++)
            {
                for (int g = 0; g < availableLetters.Length && !isLetterValid; g++)
                {
                    if (i == 0 || i == 2)
                        if (convertUserInputUpper[i] == availableLetters[g])
                        {
                            countValidLettersNNumbers++;
                            isLetterValid = true;
                        }

                    if (i == 1 || i == 3)
                        if (convertUserInputUpper[i] == availableNumbers[g])
                        {
                            countValidLettersNNumbers++;
                            isLetterValid = true;
                        }
                }
                isLetterValid = false;
            }
            if (countValidLettersNNumbers == 4)
                return true;
            else
                Console.WriteLine("You entered invalid numbers/letters");
            return false;
        } // This functions checks if the user input that was inserted is valid
        ChessPiece[,] AddBoardToBoardHistory(ChessPiece[,] board, int moves)
        {

            ChessPiece[,] boardHistory = new ChessPiece[8, 8];
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    boardHistory[i, g] = board[i, g];
                }
            }

            return boardHistory;
        } // This function adds the board to board history
        string convertUserInputToIndex(ChessPiece[,] board, string userInput)   
        {
            string userInputToUpper = userInput.ToUpper();
            int column = (char)userInput[1] - '0';     // converts the number as char to int
            int row = 0;
            switch (userInputToUpper[0])
            {
                case 'A':
                    row = 0;
                    break;
                case 'B':
                    row = 1;
                    break;
                case 'C':
                    row = 2;
                    break;
                case 'D':
                    row = 3;
                    break;
                case 'E':
                    row = 4;
                    break;
                case 'F':
                    row = 5;
                    break;
                case 'G':
                    row = 6;
                    break;
                case 'H':
                    row = 7;
                    break;
                default:
                    Console.WriteLine("BUG IN CHANGE TO");
                    break;
            }
            return "" + (column - 1) + row;
        } // Converts the letter that the user inserted to an index number
        bool checkWhoseTurn(string chosenPieceBeforeMoving, int gameTotalMoves, ChessPiece[,] board)
        {
            //This function is checking if the correct soldier was chosen, if white player actually picked white soldier and not black.
            bool checkWhoseTurn = true;
            int row = chosenPieceBeforeMoving[1] - '0';
            int column = chosenPieceBeforeMoving[0] - '0';
            char isBlackTurn = gameTotalMoves % 2 == 0 ? 'W' : 'B';   //Is it white's turn or black's turn
            if (!(board[column, row] is null))
            {
                if (isBlackTurn == 'B' && board[column, row].getColor() == 'W')        // If its blackplayer's turn and a white soldier was picked
                {
                    Console.WriteLine("You chose a white soldier and its black player's turn");
                    checkWhoseTurn = false;
                }
                if (isBlackTurn == 'W' && board[column, row].getColor() == 'B')
                {
                    Console.WriteLine("You chose a black soldier and its white player's turn");
                    checkWhoseTurn = false;
                }
            }
            else
            {
                Console.WriteLine("You chose an empty square");
                checkWhoseTurn = false;
            }
            return checkWhoseTurn;
        } // This functions checks if the user picked his piece and not the other player piece
        bool isMovePossible(ChessPiece[,] board, string currentPieceLocation, string movingPieceToLocation, int totalGameMoves)
        {
            int currentRow = currentPieceLocation[1] - '0';
            int currentColumn = currentPieceLocation[0] - '0';
            int nextRow = movingPieceToLocation[1] - '0';
            int nextColumn = movingPieceToLocation[0] - '0';
            bool isMoveLegal = true;

            isMoveLegal = board[currentColumn, currentRow].setMove(board, currentColumn, currentRow, nextColumn, nextRow, false, totalGameMoves);

            if (isMoveLegal && board[currentColumn, currentRow] is King)
                isMoveLegal = isKingMovalAllowed(board, currentColumn, currentRow, nextColumn, nextRow, totalGameMoves);

            return isMoveLegal;
        } // This function checks if the move is legal
        bool canRookSaveKing(ChessPiece[,] board, int currentkingcolumn, int currentkingrow, int totalGameMoves, char color)
        {

            bool stop = false;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    if (board[i, g] is Rook && board[i, g].getColor() == color)
                    {
                        for (int j = i - 1; !stop && j <= 7 && j >= 0; j--) // UP only column changes
                        {
                            if (board[j, g] is null || board[j, g].getColor() != board[i, g].getColor())
                            {
                                if (swapForCheck(board, j, g, i, g, totalGameMoves, color) is false)
                                    return true; //IF this is false, it means it swapped the new row and column and checked if there is still a check, if it returned false, then there is no checkmate
                            }
                            else { stop = true; }
                        }
                        stop = false;
                        for (int j = i + 1; !stop && j <= 7 && j >= 0; j++)
                        {
                            if (board[j, g] is null || board[j, g].getColor() != board[i, g].getColor())
                            {
                                if (swapForCheck(board, j, g, i, g, totalGameMoves, color) is false)
                                    return true;
                            }
                            else { stop = true; }
                        }
                        stop = false;
                        for (int newrow = g + 1; !stop && newrow <= 7 && newrow >= 0; newrow++)
                        {
                            if (board[i, newrow] is null || board[i, newrow].getColor() != board[i, g].getColor())
                            {
                                if (swapForCheck(board, i, newrow, i, g, totalGameMoves, color) is false)
                                    return true;
                            }
                            else { stop = true; }
                        }
                        stop = false;
                        for (int newrow = g - 1; !stop && newrow <= 7 && newrow >= 0; newrow--)
                        {
                            if (board[i, newrow] is null || board[i, newrow].getColor() != board[i, g].getColor())
                            {
                                if (swapForCheck(board, i, newrow, i, g, totalGameMoves, color) is false)
                                    return true;
                            }
                            else { stop = true; }
                        }
                        stop = false;

                    }
                }
            }
            return false;
        } // Determine if rook can save king
        void makeCastleMove(ChessPiece[,] board, int currentRow, int currentColumn, int nextRow, int nextColumn)
        {
            King tempLocationPieceBeforeMoves = null;
            Rook tempNewLocation = null;
            tempLocationPieceBeforeMoves = (King)board[currentColumn, currentRow];
            tempNewLocation = (Rook)board[nextColumn, nextRow];
            if (board[currentColumn, currentRow].getColor() == 'B')
            {
                if (nextColumn == 7 && nextRow == 0)
                {
                    board[currentColumn, currentRow - 2] = tempLocationPieceBeforeMoves;
                    board[nextColumn, nextRow + 3] = tempNewLocation;
                }
                if (nextColumn == 7 && nextRow == 7)
                {
                    board[currentColumn, currentRow + 2] = tempLocationPieceBeforeMoves;
                    board[nextColumn, nextRow - 2] = tempNewLocation;
                }
            }
            if (board[currentColumn, currentRow].getColor() == 'W')
            {
                if (nextColumn == 0 && nextRow == 0)
                {
                    board[currentColumn, currentRow - 2] = tempLocationPieceBeforeMoves;
                    board[nextColumn, nextRow + 3] = tempNewLocation;
                }
                if (nextColumn == 0 && nextRow == 7)
                {
                    board[nextColumn, nextRow - 2] = tempNewLocation;
                    board[currentColumn, currentRow + 2] = tempLocationPieceBeforeMoves;
                }
            }
            board[currentColumn, currentRow] = null;
            board[nextColumn, nextRow] = null;
        } // This function makes the castle movement.
        bool isStaleMate(ChessPiece[,] board, int totalGameMoves)
        {
            int newcolumn = 0;
            int newrow = 0;
            int count = 0;
            int knightCount = 0;
            int countWhiteAvailableMoves = 0;
            int countBlackAvailableMoves = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    if (!(board[i, g] is null))
                    {
                        char currentColor = board[i, g].getColor();
                        if (!(board[i, g] is Knight))
                        {

                            newcolumn = i;
                            newrow = g + 1;
                            while (count < 9)
                            {
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow] is null)
                                    if (board[i, g].setMove(board, i, g, newcolumn, newrow, true, totalGameMoves))
                                    {
                                        if (!(swapForCheck(board, newcolumn, newrow, i, g, totalGameMoves, currentColor)))
                                        {
                                            if (currentColor == 'W') { countWhiteAvailableMoves++; }
                                            else { countBlackAvailableMoves++; }
                                            break;
                                        }
                                    }
                                if (count == 0) { newrow = g + 1; }
                                if (count == 1) { newrow = g - 1; }
                                if (count == 2) { newrow = g; newcolumn = i - 1; }
                                if (count == 3) { newcolumn = i + 1; }
                                if (count == 4) { newrow = g + 1; }
                                if (count == 5) { newrow = g - 1; }
                                if (count == 6) { newcolumn = i - 1; newrow = g + 1; }
                                if (count == 7) { newrow = g - 1; }
                                count++;
                            }
                        }
                        else
                        {
                            while (knightCount < 8)
                            {
                                newrow = g + 2;
                                newcolumn = i - 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is null || board[newcolumn, newrow].getColor() != currentColor) && !(swapForCheck(board, newcolumn, newrow, i, g, totalGameMoves, currentColor)))
                                {
                                    countBlackAvailableMoves++;
                                    break;
                                }
                                if (knightCount == 0) { newcolumn = i + 1; }
                                if (knightCount == 1) { newrow = g + 1; newcolumn = i - 2; }
                                if (knightCount == 2) { newrow = g - 1; }
                                if (knightCount == 3) { newrow = g - 2; newcolumn = i - 1; }
                                if (knightCount == 4) { newcolumn = i + 1; }
                                if (knightCount == 5) { newcolumn = i + 2; newrow = g - 1; }
                                if (knightCount == 6) { newrow = g + 1; }
                                knightCount++;
                            }
                        }

                    }
                }
                count = 0;
                knightCount = 0;
            }
            if (countWhiteAvailableMoves == 0 || countBlackAvailableMoves == 0)
                return true;
            return false;
        } // Checks if there is stalemate
        bool checkBoardThreeTimes(ChessPiece[][,] boardHistory, ChessPiece[,] board)
        {
            int[] count = new int[500];
            int index = 0;
            int howManyAreSame = 0;
            for (int i = 0; i < boardHistory[index].GetLength(0); i++)
            {
                for (int g = 0; g < boardHistory[index].GetLength(1); g++)
                {
                    if (boardHistory[index][i, g] == board[i, g])
                        count[index]++;
                }
                if (i == 7) { i = -1; index++; }
                if (boardHistory[index] == null) { break; ; }
            }
            for (int i = 0; i < count.Length; i++)
            {
                if (count[i] == 0) { break; }
                if (count[i] == 64) { howManyAreSame++; }
            }
            if (howManyAreSame == 3)
                return true;
            return false;
        } // Runs through the board history and checks if the board is the same three times.
        bool isBoardLackOfPieces(ChessPiece[,] board)
        {
            int randomPieces = 0;
            int whiteKnightBishop = 0;
            int blackKnightBishop = 0;
            int whiteKing = 0;
            int blackKing = 0;
            bool isBoardLackOfPieces = false;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (!(board[i, j] is null))
                    {
                        if (board[i, j].getColor() == 'W')
                        {
                            if (board[i, j] is King) { whiteKing++; }
                            else if (board[i, j] is Knight || board[i, j] is Bishop) { whiteKnightBishop++; }
                            else randomPieces++;
                        }
                        if (board[i, j].getColor() == 'B')
                        {
                            if (board[i, j] is King) { blackKing++; }
                            else if (board[i, j] is Knight || board[i, j] is Bishop) { blackKnightBishop++; }
                            else randomPieces++;
                        }
                    }
                }
            }
            if (whiteKing == 1 && blackKing == 1 && randomPieces == 0 && whiteKnightBishop == 0 && blackKnightBishop == 0)
                return true;
            if (whiteKing == 1 && blackKing == 1 && (whiteKnightBishop == 1 && blackKnightBishop == 0) || (whiteKnightBishop == 0 && blackKnightBishop == 1))
                return true;
            if (whiteKing == 1 && blackKing == 1 && whiteKnightBishop == 1 && blackKnightBishop == 1)
                return true;
            return isBoardLackOfPieces;
        } // Applying the lack of pieces role
        bool canBishopSaveKing(ChessPiece[,] board, int currentkingcolumn, int currentkingrow, int totalGameMoves, char color)
        {
            bool stop = false;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    if (board[i, g] is Bishop && board[i, g].getColor() == color)
                    {
                        for (int newcolumn = i - 1, newrow = g + 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn--, newrow++)
                        {
                            if (board[newcolumn, newrow] is null || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, totalGameMoves, color) is false)
                                    return true;
                            }
                            else { stop = true; }
                        }
                        stop = false;
                        for (int newcolumn = i - 1, newrow = g - 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn--, newrow--)
                        {
                            if (board[newcolumn, newrow] is null || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, totalGameMoves, color) is false)
                                    return true;
                            }
                            else { stop = true; }
                        }
                        stop = false;
                        for (int newcolumn = i + 1, newrow = g - 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn++, newrow--)
                        {
                            if (board[newcolumn, newrow] is null || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, totalGameMoves, color) is false)
                                    return true;
                            }
                            else { stop = true; }
                        }
                        stop = false;
                        for (int newcolumn = i + 1, newrow = g + 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn++, newrow++)
                        {
                            if (board[newcolumn, newrow] is null || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, totalGameMoves, color) is false)
                                    return true;
                            }
                            else { stop = true; }
                        }
                    }
                }
            }
            return false;
        } // Deterimne if bishop can save king
        bool canKnightSaveKing(ChessPiece[,] board, int currentkingcolumn, int currentkingrow, int totalGameMoves, char color)
        {
            int countForNextColumnRow = 0;
            for (int i = 0; countForNextColumnRow < 7 && i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    if (board[i, g] is Knight && board[i, g].getColor() == color)
                    {
                        int newrow = g + 2;
                        int newcolumn = i - 1;
                        if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is null || board[newcolumn, newrow].getColor() != board[i, g].getColor()))
                        {
                            if (swapForCheck(board, newcolumn, newrow, i, g, totalGameMoves, color) is false)
                                return true;
                        }
                        if (countForNextColumnRow == 0) { newcolumn = i + 1; }
                        if (countForNextColumnRow == 1) { newrow = g + 1; newcolumn = 1 - 2; }
                        if (countForNextColumnRow == 2) { newrow = g - 1; }
                        if (countForNextColumnRow == 3) { newrow = g - 2; newcolumn = i - 1; }
                        if (countForNextColumnRow == 4) { newcolumn = i + 1; }
                        if (countForNextColumnRow == 5) { newcolumn = i + 2; newrow = g - 1; }
                        if (countForNextColumnRow == 6) { newrow = g + 1; }
                        countForNextColumnRow++;
                    }
                }
            }
            return false;
        } // Determine if knight can save king
        bool canPawnSaveKing(ChessPiece[,] board, int currentkingcolumn, int currentkingrow, int totalGameMoves, char color)
        {
            int countForNextRowColumn = 0;
            int newcolumn = 0;
            int newrow = 0;
            for (int i = 0; countForNextRowColumn < 2 && i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    if (board[i, g] is Pawn && board[i, g].getColor() == color)
                    {
                        newrow = g;
                        newcolumn = i + 1;
                        while (countForNextRowColumn < 3 && board[i, g].getColor() == 'W')
                        {
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is null || board[newcolumn, newrow].getColor() != color))
                                if (swapForCheck(board, newcolumn, newrow, i, g, totalGameMoves, color) is false)
                                    return true;

                            if (countForNextRowColumn == 0) { newrow = g - 1; newcolumn = i - 1; }
                            if (countForNextRowColumn == 1) { newrow = g + 1; newcolumn = i + 1; }
                            countForNextRowColumn++;
                        }
                        newrow = g;
                        newcolumn = i - 1;
                        while (countForNextRowColumn < 3 && board[i, g].getColor() == 'B')
                        {
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is null || board[newcolumn, newrow].getColor() != color))
                                if (swapForCheck(board, newcolumn, newrow, i, g, totalGameMoves, color) is false)
                                    return true;
                            if (countForNextRowColumn == 0) { newrow = g - 1; newcolumn = i - 1; }
                            if (countForNextRowColumn == 1) { newrow = g + 1; newcolumn = i - 1; }
                            countForNextRowColumn++;
                        }
                    }
                    countForNextRowColumn = 0;
                }
                countForNextRowColumn = 0;
            }
            return false;
        } // Determine if pawn can save king
        bool canQueenSaveKing(ChessPiece[,] board, int currentkingcolumn, int currentkingrow, int totalGameMoves, char color)
        {

            bool canBishop = false;
            bool canRook = false;
            canBishop = canBishopSaveKing(board, currentkingcolumn, currentkingrow, totalGameMoves, color);
            canRook = canRookSaveKing(board, currentkingcolumn, currentkingrow, totalGameMoves, color);
            if (canRook || canBishop)
                return true;
            return false;
        } // Determine if queen can save king
        bool isCheckMate(ChessPiece[,] board, int currentkingcolumn, int currentkingrow, int totalGameMoves)
        {
            char color = board[currentkingcolumn, currentkingrow].getColor();
            bool canBishop, canRook, canPawn, canQueen, canKnight;
            canBishop = canBishopSaveKing(board, currentkingcolumn, currentkingrow, totalGameMoves, color);
            canRook = canRookSaveKing(board, currentkingcolumn, currentkingrow, totalGameMoves, color);
            canPawn = canPawnSaveKing(board, currentkingcolumn, currentkingrow, totalGameMoves, color);
            if (canBishop || canRook)
                canQueen = true;
            canKnight = canKnightSaveKing(board, currentkingcolumn, currentkingrow, totalGameMoves, color);
            if (canKnight || canBishop || canPawn || canRook)
                return false;
            return true;
        } // Checks if there is checkmate
        bool isKingMovalAllowed(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, int moves)
        {
            bool isKingMovalAllowed = true;
            if (swapForCheck(board, nextColumn, nextRow, currentColumn, currentRow, moves, 'E') is true)
            {
                Console.WriteLine("The King cannot move there, it will be check");
                isKingMovalAllowed = false;
            }
            if (isKingMovalAllowed && board[nextColumn, nextRow] is Rook)
            {
                if ((nextColumn == 7 || nextColumn == 0) && nextRow == 7)
                {
                    for (int i = currentRow + 1; isKingMovalAllowed && i < nextRow; i++)
                    {
                        if (swapForCheck(board, nextColumn, i, currentColumn, currentRow, moves, 'E') is true)
                        {
                            Console.WriteLine("Cannot do that, king will be in check");
                            isKingMovalAllowed = false;
                        }
                    }
                }
            }
            if ((nextColumn == 7 || nextColumn == 0) && nextRow == 0)
            {
                for (int i = currentRow - 1; isKingMovalAllowed && i > nextRow; i--)
                {
                    if (swapForCheck(board, nextColumn, i, currentColumn, currentRow, moves, 'E') is true)
                    {
                        Console.WriteLine("Cannot do that, king will be in check");
                        isKingMovalAllowed = false;
                    }
                }
            }
            return isKingMovalAllowed;
        } // Checks if king makes a move, if there is a check afterward.
        bool isPiecePromoted(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow)
        {
            bool isPiecePromoted = false;
            if (board[currentColumn, currentRow].getColor() == 'W')
            {
                if (currentColumn == 6 && nextColumn == 7)
                {
                    Promotion(board, currentColumn, currentRow, nextColumn, nextRow, 'W');
                    isPiecePromoted = true;
                }
            }
            else if (board[currentColumn, currentRow].getColor() == 'B')
            {
                if (currentColumn == 1 && nextColumn == 0)
                {
                    Promotion(board, currentColumn, currentRow, nextColumn, nextRow, 'B');
                    isPiecePromoted = true;
                }
            }
            return isPiecePromoted;
        } // Checks if one of the players reached top/bottom of the board and promote his piece
       /* void movingPieceEnPassant(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, int newrow)
        {
            board[currentColumn, newrow] = null;
            ChessPiece temp = board[currentColumn, currentRow];
            board[nextColumn, nextRow] = temp;
            board[currentColumn, currentRow] = null;
            printBoard(board);
        }*/
        void Promotion(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, char color)
        {
            int count = 1;
            board[currentColumn, currentRow] = null;
            while (count == 1)
            {
                Console.WriteLine("Which piece do you want to promote your Pawn?\nBishop, Queen, Rook, Knight");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "Bishop":
                        board[nextColumn, nextRow] = new Bishop(color);
                        count = 0;
                        break;
                    case "Queen":
                        board[nextColumn, nextRow] = new Queen(color);
                        count = 0;
                        break;
                    case "Rook":
                        board[nextColumn, nextRow] = new Rook(color);
                        count = 0;
                        break;
                    case "Knight":
                        board[nextColumn, nextRow] = new Knight(color);
                        count = 0;
                        break;
                    default:
                        Console.WriteLine("You didn't enter a valid piece name");
                        break;
                }
            }
            printBoard(board);
        } // If a piece is promoted returns true, promotes the piece.
        ChessPiece[,] movingPieceInBoard(ChessPiece[,] board, string currentPieceLocation, string movingPieceToLocation, int fiftyMovesRoleAnyPieceEaten)
        {
            int currentRow = currentPieceLocation[1] - '0';
            int currentColumn = currentPieceLocation[0] - '0';
            int nextRow = movingPieceToLocation[1] - '0';
            int nextColumn = movingPieceToLocation[0] - '0';
            if (!(board[nextColumn, nextRow] is null))
            {   
                if(!(board[currentColumn, currentRow] is Pawn))
                    if (board[currentColumn, currentRow].getColor() != board[nextColumn, nextRow].getColor())
                        fiftyMovesRoleAnyPieceEaten = 0;
                else fiftyMovesRoleAnyPieceEaten++;
            }
            else if (board[currentColumn, currentRow] is Pawn)
                fiftyMovesRoleAnyPieceEaten = 0;
            else fiftyMovesRoleAnyPieceEaten++;
            ChessPiece currentPiece = board[currentColumn, currentRow];
            ChessPiece newLocationPiece = board[nextColumn, nextRow];
            if(!(board[nextColumn, nextRow] is null))
                if (board[nextColumn, nextRow].getColor() != board[currentColumn, currentRow].getColor())
                    newLocationPiece = null;
            
            board[currentColumn, currentRow] = newLocationPiece;
            board[nextColumn, nextRow] = currentPiece;
            return board;
        } // Makes the move in the board, returns a new board.
        string kingLocation(ChessPiece[,] board)
        {
            string whiteKingColumnRow = "";
            string blackKingColumnRow = "";
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    if (!(board[i, g] is null))
                    {
                        if (board[i, g].getColor() == 'W' && board[i, g] is King)
                        {
                            whiteKingColumnRow += i + "" + g;
                        }
                        if (board[i, g].getColor() == 'B' && board[i, g] is King)
                        {
                            blackKingColumnRow += i + "" +  g;
                        }
                    }
                }
            }
            return "" + whiteKingColumnRow + "" + blackKingColumnRow;
        } // Finds the location of the kings.
        string isCheck(ChessPiece[,] board, int moves, char currentKingInThreat)
        {
            // returns string, if no check then returns false. otherwise returns the threatened king location
            string kingsLocation = kingLocation(board);
            int whiteKingColumn = kingsLocation[0] - '0';
            int whiteKingRow = kingsLocation[1] - '0';
            int blackKingColumn = kingsLocation[2] - '0';
            int blackKingRow = kingsLocation[3] - '0';
            int currentKingColumn = 0;
            int currentKingRow = 0;
            bool isCheck = false;
            string result;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    if (!(board[i, g] is null))
                    {
                        if (currentKingInThreat == 'E')
                        {
                            currentKingColumn = board[i, g].getColor() == 'W' ? blackKingColumn : whiteKingColumn;
                            currentKingRow = board[i, g].getColor() == 'W' ? blackKingRow : whiteKingRow;
                            isCheck = board[i, g].setMove(board, i, g, currentKingColumn, currentKingRow, true, moves);
                        }
                        else
                        {
                            currentKingColumn = currentKingInThreat == 'W' ? whiteKingColumn : blackKingColumn;
                            currentKingRow = currentKingInThreat == 'W' ? whiteKingRow : blackKingRow;
                            if(board[i, g].getColor() != currentKingInThreat)
                                isCheck = board[i, g].setMove(board, i, g, currentKingColumn, currentKingRow, true, moves);
                        }
                    }
                    if (isCheck)
                        break;
                }
                if (isCheck)
                    break;
            }

            if (isCheck)
                result = currentKingColumn + "" + currentKingRow;
            else
                result = "False";
            return result;
        } // Runs through the board to see if there is a check
        bool isKingBlocked(ChessPiece[,] board, int currentColumn, int currentRow, int moves)
        {
            bool isKingBlocked = true;
            bool isMoveLegal = false;
            bool isCheckMateAfterMoval = false;
            int countToChangeNewColumnNRow = 0;
            int newColumn = currentColumn - 1; // Up
            int newRow = currentRow;
            while (isKingBlocked && countToChangeNewColumnNRow < 7)
            {
                if (newColumn <= 7 && newColumn >= 0 && newRow <= 7 && newRow >= 0)
                {
                    isMoveLegal = board[currentColumn, currentRow].setMove(board, currentColumn, currentRow, newColumn, newRow, true, moves);
                    if (isMoveLegal)
                    {
                        isCheckMateAfterMoval = swapForCheck(board, newColumn, newRow, currentColumn, currentRow, moves, 'E');
                        if (!isCheckMateAfterMoval) { isKingBlocked = false; return isKingBlocked; }
                    }
                }
                if (countToChangeNewColumnNRow == 0) { newColumn = currentColumn + 1; }
                if (countToChangeNewColumnNRow == 1) { newColumn = currentColumn; newRow = currentRow - 1; }
                if (countToChangeNewColumnNRow == 2) { newColumn = currentColumn; newRow = currentRow + 1; }
                if (countToChangeNewColumnNRow == 3) { newColumn = currentColumn - 1; newRow = currentRow - 1; }
                if (countToChangeNewColumnNRow == 4) { newColumn = currentColumn - 1; newRow = currentRow + 1; }
                if (countToChangeNewColumnNRow == 5) { newColumn = currentColumn + 1; newRow = currentRow - 1; }
                if (countToChangeNewColumnNRow == 6) { newColumn = currentColumn + 1; newRow = currentRow + 1; }
                countToChangeNewColumnNRow++;
            }
        
            return true;
        } // Checks if the king can make a move
        bool swapForCheck(ChessPiece[,] board, int newcolumn, int newrow, int oldcolumn, int oldrow, int moves, char wb)
        {
            ChessPiece tempnew = null;
            ChessPiece tempold = null;
            ChessPiece checkIfKingEat;
            ChessPiece blank;
            if (board[oldcolumn, oldrow] is King && !(board[newcolumn, newrow] is null))
            {
                checkIfKingEat = null;
                tempnew = board[newcolumn, newrow];
                tempold = board[oldcolumn, oldrow];
                board[newcolumn, newrow] = tempold;
                board[oldcolumn, oldrow] = checkIfKingEat;
            }
            else if (!(board[newcolumn, newrow] is null))
            {
                if (board[newcolumn, newrow].getColor() != 'E' && board[newcolumn, newrow].getColor() != board[oldcolumn, oldrow].getColor())
                {
                    blank = null;
                    tempnew = board[newcolumn, newrow];
                    tempold = board[oldcolumn, oldrow];
                    board[newcolumn, newrow] = tempold;
                    board[oldcolumn, oldrow] = blank;
                }
            }
            else
            {
                tempnew = board[newcolumn, newrow];
                tempold = board[oldcolumn, oldrow];
                board[newcolumn, newrow] = tempold;
                board[oldcolumn, oldrow] = tempnew;
            }
            if (isCheck(board, moves, wb) == "False")
            {
                board[newcolumn, newrow] = tempnew;
                board[oldcolumn, oldrow] = tempold;
                return false; //IF FALSE THERE IS NO CHECKMATE
            }
            else
            {
                board[newcolumn, newrow] = tempnew;
                board[oldcolumn, oldrow] = tempold;
            }
            return true;
        } // Checks if there is a check with the new pieces of the inserted input.
    }
    interface Piece
    {
        bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves);
    }
    class ChessPiece
    {
        int totalMoves;
        char color;
        public ChessPiece(char color){ this.color = color; }
        public char getColor()
        {
            return color;
        }
        public virtual bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int totalGameMoves) { return false; }
        public override string ToString()
        {
            return color == 'W' ? "W" : "B";
        }
    }
    class Pawn : ChessPiece, Piece
    {
        int totalMoves = 0;
        int EnPassant = 0; //הכאה דרך הילוכו
        char color;
        public Pawn(char color) : base(color){ }
        public override bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int totalGameMoves) // 1 0
        {
            char currentPieceColor = this.getColor();
            int nextMinusCurrentColumn = nextColumn - currentColumn;
            int nextMinusCurrentRow = nextRow - currentRow;
            bool isPossible = false;
            if (!(board[nextColumn, nextRow] is null))
            {
                if (board[nextColumn, nextRow].getColor() == currentPieceColor)
                {
                    string pieceName = board[nextColumn, nextRow].GetType().Name;
                    if (!check)
                        Console.WriteLine("You cant eat your own {0}", pieceName);
                    return false;
                }
            }
            if (!check && totalMoves == 0) { this.EnPassant += totalGameMoves; }
            bool enpassant = false;
            if (currentColumn == 3 && board[currentColumn, currentRow].getColor() == 'B' && nextMinusCurrentColumn == -1)
            {
                if (nextMinusCurrentRow == -1 && board[currentColumn, currentRow - 1] is Pawn && board[nextColumn, nextRow] is null)
                {
                    if (totalGameMoves - ((Pawn)board[currentColumn, currentRow - 1]).getEnPassant() == 1)
                    {
                        if (!check)
                            board[currentColumn, currentRow - 1] = null;
                        enpassant = true;
                    }
                }
                if (nextMinusCurrentRow == 1 && board[currentColumn, currentRow + 1] is Pawn && board[nextColumn, nextRow] is null)
                {
                    if (totalGameMoves - ((Pawn)board[currentColumn, currentRow + 1]).getEnPassant() == 1)
                    {
                        if (!check)
                            board[currentColumn, currentRow + 1] = null;
                        enpassant = true;
                    }
                }
            }
            if (currentColumn == 4 && board[currentColumn, currentRow].getColor() == 'W' && nextMinusCurrentColumn == 1)
            {
                if (nextMinusCurrentRow == 1 && board[currentColumn, currentRow + 1] is Pawn && board[nextColumn, nextRow] is null)
                {
                    if (totalGameMoves - ((Pawn)board[currentColumn, currentRow + 1]).getEnPassant() == 1)
                    {
                        if (!check)
                            board[currentColumn, currentRow + 1] = null;
                        enpassant = true;
                    }
                }
                if (nextMinusCurrentRow == -1 && board[currentColumn, currentRow - 1] is Pawn && board[nextColumn, nextRow] is null)
                {
                    if (totalGameMoves - ((Pawn)board[currentColumn, currentRow - 1]).getEnPassant() == 1)
                    {
                        if(!check)
                            board[currentColumn, currentRow - 1] = null;
                        enpassant = true;
                    }
                }
            }
            if (enpassant)
                return true;

            //-----------------------
            if (totalMoves == 0)
            {
                if (this.getColor() == 'B')
                {
                    if ((nextMinusCurrentColumn == -1 || nextMinusCurrentColumn == -2) && nextMinusCurrentRow == 0)
                    {
                        if (nextMinusCurrentColumn == -2)
                        {
                            if (board[currentColumn - 1, currentRow] is null && board[nextColumn, nextRow] is null)
                            {
                                if (!check)
                                    totalMoves++;
                                return true;
                            }
                            else
                            {
                                if (!check)
                                    Console.WriteLine("There is other piece blocking you, you cannot do that move");
                                return false;
                            }
                        }
                        if (nextMinusCurrentColumn == -1)
                        {
                            if (board[nextColumn, nextRow] is null)
                            {
                                if (!check)
                                    totalMoves++;
                                return true;
                            }
                            else
                            {
                                if (!check)
                                    Console.WriteLine("There is another piece blocking you");
                                return false;
                            }

                        }
                    }
                }
                if (board[currentColumn, currentRow].getColor() == 'W')
                {
                    if ((nextMinusCurrentColumn == 2 || nextMinusCurrentColumn == 1) && nextMinusCurrentRow == 0)
                    {
                        if (nextMinusCurrentColumn == 2)
                        {
                            if (board[currentColumn + 1, currentRow] is null && board[nextColumn, nextRow] is null)
                            {
                                if (!check)
                                    totalMoves++;
                                return true;
                            }
                            else
                            {
                                if (!check)
                                    Console.WriteLine("There is another piece blocking you");
                                return false;
                            }
                        }
                        if (nextMinusCurrentColumn == 1)
                        {
                            if (board[nextColumn, nextRow] is null)
                            {
                                if (!check)
                                    totalMoves++;
                                return true;
                            }
                            else
                            {
                                if (!check)
                                    Console.WriteLine("There is another piece blocking you");
                                return false;
                            }
                        }
                    }
                }
            }

            if (this.getColor() == 'B')
            {
                if (nextMinusCurrentColumn == -1 && nextMinusCurrentRow == 0 && board[nextColumn, nextRow] is null)
                {
                    if (!check)
                        totalMoves++;
                    isPossible = true;
                }


                else if (!(board[nextColumn, nextRow] is null))
                {
                    if (board[nextColumn, nextRow].getColor() == 'W' && nextMinusCurrentColumn == -1 && (nextMinusCurrentRow == -1 || nextMinusCurrentRow == 1))
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }
                }
                else
                {
                    if (!check)
                        Console.WriteLine("Pawn can only move one square forward or eat one square diagonally");
                    return false;
                }
            }
            if (board[currentColumn, currentRow].getColor() == 'W')
            {
                if (nextMinusCurrentColumn == 1 && nextMinusCurrentRow == 0 && board[nextColumn, nextRow] is null)
                {
                    if (!check)
                        totalMoves++;
                    isPossible = true;
                }
                else if (!(board[nextColumn, nextRow] is null))
                {
                    if (board[nextColumn, nextRow].getColor() == 'B' && nextMinusCurrentColumn == 1 && (nextMinusCurrentRow == 1 || nextMinusCurrentRow == -1))
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }
                }
                else
                {
                    if (!check)
                        Console.WriteLine("Pawn can only move one square forward or eat one square diagonally");
                    return false;
                }
            }
            if (isPossible) { return true; }
            if (totalMoves == 0)
                if (!check)
                    Console.WriteLine("Pawn can move 1 or two squares in its first move");
            if (totalMoves > 0)
                if (!check)
                    Console.WriteLine("Pawn can only move one square forward");
            return false;

        }

        public override string ToString()
        {
            return base.ToString() + "P";
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
        public int getEnPassant()
        {
            return EnPassant;
        }

    }
    class Knight : ChessPiece, Piece
    {
        int totalMoves = 0;
        string name;
        public Knight(char color) : base(color) { }
        public override bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            if (!(board[nextColumn, nextRow] is null))
            {
                if (board[nextColumn, nextRow].getColor() == this.getColor())
                {
                    string pieceName = board[nextColumn, nextRow].GetType().Name;
                    if (!check)
                        Console.WriteLine("You already have {0} there", pieceName);
                    return false;
                }
            }
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            if (currentMinusNextColumn == 1 && currentMinusNextRow == -2 || currentMinusNextColumn == 1 && currentMinusNextRow == 2 || currentMinusNextColumn == -2 && currentMinusNextRow == 1 || currentMinusNextColumn == 2 && currentMinusNextRow == 1 || currentMinusNextColumn == 2 && currentMinusNextRow == -1 || currentMinusNextColumn == -2 && currentMinusNextRow == -1 || currentMinusNextColumn == -1 && currentMinusNextRow == 2 || currentMinusNextColumn == -1 && currentMinusNextRow == -2)
            {
                if (!check)
                    totalMoves++;
                return true;
            }
            if (!check)
                Console.WriteLine("Knight cant do that move, it can only move in L shape");
            return false;
        }
        public override string ToString()
        {
            return base.ToString() + "N";
        }
    }
    class Rook : ChessPiece, Piece
    {
        int totalMoves = 0;
        public Rook(char color) : base(color) { }
        public override bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            int count = 0;
            if (!(board[nextColumn, nextRow] is null))
            {
                if (board[nextColumn, nextRow].getColor() == this.getColor())
                {
                    string pieceName = board[nextColumn, nextRow].GetType().Name;
                    if (!check )
                        Console.WriteLine("You can't eat your own {0}", pieceName);
                    isPossible = false;
                    return isPossible;
                }
            }
            if (currentColumn > nextColumn && currentMinusNextRow == 0)
            {
                count++;
                for (int i = currentColumn - 1; isPossible && i > nextColumn; i--)
                {
                    if (!(board[i, currentRow] is null))
                    {
                        isPossible = false;
                    }
                }
            }
            if (currentRow > nextRow && currentMinusNextColumn == 0)
            {
                count++;
                for (int i = currentRow - 1; isPossible && i > nextRow; i--)
                {
                    if (!(board[currentColumn, i] is null))
                    {
                        isPossible = false;
                    }
                }
            }
            if (currentColumn < nextColumn && currentMinusNextRow == 0)
            {
                count++;
                for (int i = currentColumn + 1; isPossible && i < nextColumn; i++)
                {
                    if (!(board[i, currentRow] is null))
                    {
                        isPossible = false;
                    }
                }
            }
            if (currentRow < nextRow && currentMinusNextColumn == 0)
            {
                count++;
                for (int i = currentRow + 1; i < nextRow; i++)
                {
                    if (!(board[currentColumn, i] is null))
                    {
                        isPossible = false;
                    }
                }
            }
            if (count == 0)
                isPossible = false;
            if (!(isPossible) && !check)
            {
                Console.WriteLine("{0} cant move over other piece", board[currentColumn, currentRow].GetType().Name);
                return isPossible;
            }
            if (!check)
                totalMoves++;
            return isPossible;
        }
        public override string ToString()
        {
            return base.ToString() + "R";
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
        public void increaseTotalMoves()
        {
            totalMoves++;
        }
    }
    class Bishop : ChessPiece, Piece
    {
        int totalMoves = 0;
        public Bishop(char color) : base(color) { }
        public override bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            if (!(board[nextColumn, nextRow] is null))
            {
                if (board[nextColumn, nextRow].getColor() == this.getColor())
                {
                    if (board[nextColumn, nextRow] is King) { }
                    else
                    {
                        string pieceName = board[nextColumn, nextRow].GetType().Name;
                        if (!check && board[currentColumn, currentRow].GetType().Name != "Queen")
                            Console.WriteLine("You can't eat your own {0}", pieceName);
                        return false;
                    }
                }
            }
            // --------------------------------------------------CHECKING FOR BLACK--------------------------------------
            if (currentColumn > nextColumn && currentRow > nextRow)
            {
                for (int i = currentColumn - 1, g = currentRow - 1; isPossible && i > nextColumn && g > nextRow; i--, g--)
                {
                    if (!(board[i, g] is null))
                    {
                        isPossible = false;
                    }
                }
            }
            if (currentColumn > nextColumn && currentRow < nextRow)
            {
                for (int i = currentColumn - 1, g = currentRow + 1; isPossible && i > nextColumn && g < nextRow; i--, g++)
                {
                    if (!(board[i, g] is null))
                    {
                        isPossible = false;
                    }
                }
            }
            //----------------------------------------------CHECKING FOR WHITE ----------------------------
            if (currentColumn < nextColumn && currentRow < nextRow)
            {
                for (int i = currentColumn + 1, g = currentRow + 1; isPossible && i < nextColumn && g < nextRow; i++, g++)
                {
                    if (!(board[i, g] is null))
                    {
                        isPossible = false;
                    }
                }
            }
            if (currentColumn < nextColumn && currentRow > nextRow)
            {
                for (int i = currentColumn + 1, g = currentRow - 1; isPossible && i < nextColumn && g > nextRow; i++, g--)
                {
                    if (!(board[i, g] is null))
                    {
                        isPossible = false;
                    }
                }
            }
            //------------------
            if (isPossible)
            {
                int tempNextColumn = currentMinusNextColumn > 0 ? currentMinusNextColumn : -(currentMinusNextColumn);
                int tempNextRow = currentMinusNextRow > 0 ? currentMinusNextRow : -(currentMinusNextRow);
                if (!(tempNextColumn == tempNextRow))
                {
                    if (!check && board[currentColumn, currentRow].GetType().Name != "Queen")
                        Console.WriteLine("Bishop can only move any number of squares diagonally");
                    isPossible = false;
                    return isPossible;
                }
            }

            if (!(isPossible) && !check && board[currentColumn, currentRow].GetType().Name != "Queen")
            {
                Console.WriteLine("Bishop cannot go over other piece");
                return isPossible;
            }
            totalMoves++;
            return isPossible;
        }

        public override string ToString()
        {
            return base.ToString() + "B";
        }
    }
    class King : ChessPiece, Piece
    {
        int totalMoves = 0;
        public King(char color) : base(color) { }
        public override bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;

            if (board[nextColumn, nextRow] is Rook && board[nextColumn, nextRow].getColor() == this.getColor())
            {
                if (currentRow > nextRow && currentMinusNextColumn == 0)
                {
                    for (int i = currentRow - 1; isPossible && i > nextRow; i--)
                    {
                        if (!(board[currentColumn, i] is null))
                        {
                            isPossible = false;
                            if (!check)
                                Console.WriteLine("Castling isn't possible, there are pieces inbetween");
                            return isPossible;
                        }
                    }
                }
                if (currentRow < nextRow && currentMinusNextColumn == 0)
                {
                    for (int i = currentRow + 1; isPossible && i < nextRow; i++)
                    {
                        if (!(board[currentColumn, i] is null))
                        {
                            isPossible = false;
                            if (!check)
                                Console.WriteLine("Castling isn't possible, there are pieces inbetween");
                            return isPossible;
                        }
                    }
                }

                if (((Rook)board[nextColumn, nextRow]).getTotalMoves() == 0)
                {
                    if (this.getTotalMoves() == 0)
                    {

                        isPossible = true;
                        if (check)
                        {
                            ((Rook)board[nextColumn, nextRow]).increaseTotalMoves();
                            totalMoves++;
                        }
                        return isPossible;
                    }
                    else
                    {
                        if (!check)
                            Console.WriteLine("King already moved, castling is only possible if it was its first move");
                        isPossible = false;
                        return isPossible;
                    }
                }
                else
                {
                    if (!check)
                        Console.WriteLine("Rook already moved, castling is only possible if it was its first move");
                    isPossible = false;
                    return isPossible;
                }
            }

            if (currentMinusNextRow > 1 || currentMinusNextRow < -1 || currentMinusNextColumn > 1 || currentMinusNextColumn < -1)
            {
                if (!check)
                    Console.WriteLine("King can only move one square in any direction");
                isPossible = false;
            }
            if (isPossible && !(board[nextColumn, nextRow] is null))
            {
                if (board[nextColumn, nextRow].getColor() != this.getColor())
                {
                    if (check)
                        totalMoves++;
                    isPossible = true;
                    return isPossible;
                }
            }
            if (isPossible && !(board[nextColumn, nextRow] is null))
            {
                if (!check)
                    Console.WriteLine("King cannot go on top of other piece");
                isPossible = false;
            }

            if (isPossible && !check)
                totalMoves++;
            return isPossible;
        }

        public override string ToString()
        {
            return base.ToString() + "K";
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
    }
    class Queen : ChessPiece, Piece
    {
        int totalMoves;
        Bishop bishop;
        Rook rook;
        public Queen(char color) : base(color) { }
        public override bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            bool isLegalMove = false;
            bishop = new Bishop(getColor());
            rook = new Rook(getColor());
            if (bishop.setMove(board, currentColumn, currentRow, nextColumn, nextRow, check, moves) || rook.setMove(board, currentColumn, currentRow, nextColumn, nextRow, check, moves))
                isLegalMove = true;
            return isLegalMove;
        }
        public override string ToString()
        {
            return base.ToString() + "Q";
        }
    }
}
