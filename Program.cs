using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            int moves = 0;
            bool isCheckMate = false;
            bool isStalemate = false;
            bool endgame = false;
            bool lackOfPieces = false;
            string[] isCheck = new string[3];
            ChessPieceCount eatPiece = new ChessPieceCount();
            ChessPiece[,] board = { { new Rook('W'), new Knight('W'), new Bishop('W'), new Queen('W'), new King('W'), new Bishop('W'), new Knight('W'), new Rook('W')},
                                    { new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W'), new Pawn('W')},
                                    { new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'),},
                                    { new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'),},
                                    { new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'),},
                                    { new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'), new Blank('E'),},
                                    { new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'), new Pawn('B'),},
                                    { new Rook('B'), new Knight('B'), new Bishop('B'), new Queen('B'), new King('B'), new Bishop('B'), new Knight('B'), new Rook('B'),}  };
            Instructions();
            printBoard(board);
            while (!endgame)
            {

                string[] isValid = askAndCheckNextMove(board, moves);

                bool correctPieceChosen = checkWhoseTurn(isValid[1], moves, board);
                if (correctPieceChosen)
                {
                    string[] Possible = IsPossible(board, isValid[1], moves);
                    if(Possible[3] == "True"){moves++;}
                    if(Possible[4] == "True"){ moves++; }
                    if (Possible[0] == "True")
                    {
                        ChessPiece[,] newBoard = Moving(board, Possible[1], Possible[2], moves, eatPiece);
                        if (!(board[0, 0] == null))
                        {
                            isCheck = Check(newBoard, moves);
                            if (moves > 25)
                            {
                                lackOfPieces = LackOfPieces(newBoard);
                                if (lackOfPieces) { Console.WriteLine("It's a tie"); endgame = true;}
                            }
                            if(eatPiece.getCount() == 50) { Console.WriteLine("It's a tie"); endgame = true; }
                            isStalemate = stalemate(newBoard);
                            if(isStalemate){ Console.WriteLine("It's a tie"); endgame = true;}
                            if (isCheck[0] == "True")
                            { 
                                isCheckMate = checkMate(newBoard, int.Parse(isCheck[1]), int.Parse(isCheck[2]), moves);
                                if (isCheckMate)
                                {
                                    if (isstill(newBoard, int.Parse(isCheck[1]), int.Parse(isCheck[2]), moves) is true)
                                        printBoard(newBoard);

                                    else
                                        isCheckMate = false;
                                }
                            }
                        }
                        moves++;
                        if(isCheckMate)
                        {
                            string name = moves % 2 == 0 ? "White" : "Black";
                            Console.WriteLine("  -----------");
                            Console.WriteLine("---CHECKMATE----");
                            Console.WriteLine("  -----------");
                            Console.WriteLine("{0} wins", name);
                            endgame = true;
                        }
                        printBoard(newBoard);
                        if (isCheck[0] == "True")
                        {
                            Console.WriteLine("  ------");
                            Console.WriteLine("---CHECK----");
                            Console.WriteLine("  ------");
                        }
                    }
                }
            }
            while (true) { }
        }

        static void Instructions()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("-          This is a Chess Game        -");
            Console.WriteLine("-    To choose a piece from the board  -");
            Console.WriteLine("-   Choose the piece you want to move  -");
            Console.WriteLine("-  And and where you want to move it to -");
            Console.WriteLine("-            For Example: A2A3          -");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("                 GOOD LUCK!");
        }   
        static void printBoard(ChessPiece[,] board)
        {
            Console.WriteLine("");
            Console.WriteLine("  A   B   C   D   E   F   G   H");
            Console.WriteLine("+---+---+---+---+---+---+---+---+");
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    Console.Write("|");
                    Console.Write(board[i, g] + " ");
                }
                Console.Write("|   " + (i + 1));
                Console.WriteLine("");
                Console.WriteLine("+---+---+---+---+---+---+---+---+");
            }
            Console.WriteLine("");
        }
        static string[] askAndCheckNextMove(ChessPiece[,] board, int moves)
        {
            bool wantToMoveValid = false;
            string wantToMove;
            string[] list = new string[2];
            while (!wantToMoveValid)
            {
                if (moves % 2 == 0)         //Checking if its white's turn
                    Console.WriteLine("It is WHITE's turn\nWhich piece do you want to move and to where?:(ex: 'A1A2')");

                if (moves % 2 != 0)          // checking if its black's turn
                    Console.WriteLine("It is BLACK's turn\nWhich piece do you want to move and to where?:(ex: 'A1A2')");

                wantToMove = Console.ReadLine();
                if (wantToMove == "")
                    Console.WriteLine("You pressed ENTER without any number");        //If user pressed nothing, looping over again


                else if (wantToMove.Length < 4)
                    Console.WriteLine("You entered less than required, enter which piece you want to move and to where"); // if user entered less than 4 letters, loop over again
                else if (wantToMove.Length > 4)
                    Console.WriteLine("You enterede more than required, enter which piece you want to move and to where");
                else
                    wantToMoveValid = checkUserInput(wantToMove);       // If user didnt press enter without any value or less than 4 letters, then checking if the letters are legit

                list[0] = wantToMoveValid + "";
                list[1] = wantToMove;
            }
            return list;
        }
        static bool checkUserInput(string userInput)
        {
            string checkLetters = "" + userInput[0] + userInput[2];
            string checkNumbers = "" + userInput[1] + userInput[3];
            checkLetters = checkLetters.ToUpper();
            string availableLetters = "ABCDEFGH";
            string availableNumbers = "12345678";
            bool validLetter = false;
            bool validNumber = false;
            int countValidLetters = 0;
            int countValidNumbers = 0;
            for (int i = 0; i < checkLetters.Length && !validLetter; i++)
            {
                for (int g = 0; g < availableLetters.Length && !validLetter; g++)
                {
                    if (checkLetters[i] == availableLetters[g])
                    {
                        countValidLetters++;
                        g = availableLetters.Length;
                    }
                }
            }

            for (int i = 0; i < checkNumbers.Length && !validNumber; i++)
            {
                for (int g = 0; g < availableNumbers.Length; g++)
                {
                    if (checkNumbers[i] == availableNumbers[g])
                    {
                        countValidNumbers++;
                        g = availableLetters.Length;
                    }
                }
            }
            validLetter = countValidLetters == 2 ? true : false;
            validNumber = countValidNumbers == 2 ? true : false;

            if (validNumber && validLetter)
                return true;
            if (!validLetter)
                Console.WriteLine("You entered an invalid letter");
            if (!validNumber)
                Console.WriteLine("You entered an invalid number");
            return false;
        }
    

    static bool checkWhoseTurn(string wantToMove, int move, ChessPiece[,] board)
        {
            bool checkWhoseTurn = true;
            string movingPiece = "" + wantToMove[0] + wantToMove[1];  // User chosen piece to move
            char whoseTurn = move % 2 == 0 ? 'W' : 'B';   //Is it white's turn or black's turn

            string tempLocation = changeTo(board, movingPiece);
            int column = tempLocation[0] - '0';
            int row = tempLocation[2] - '0';
            if (whoseTurn == 'B' && board[column, row].getColor() == 'W')        // If its blackplayer's turn and a white soldier was picked
            {
                Console.WriteLine("You chose a white soldier and its black player's turn");
                checkWhoseTurn = false;
            }
            if (whoseTurn == 'W' && board[column, row].getColor() == 'B')
            {
                Console.WriteLine("You chose a black soldier and its white player's turn");
                checkWhoseTurn = false;
            }
            return checkWhoseTurn;
        }

        static string[] IsPossible(ChessPiece[,] board, string wantToMove, int moves) 
        {
            
            string movingPiece = "" + wantToMove[0] + wantToMove[1];  // User chosen piece to move
            string moveTo = "" + wantToMove[2] + wantToMove[3];
            string pieceLocation = changeTo(board, movingPiece);
            string moveToLocation = changeTo(board, moveTo);
            bool canIt = canDoThatMove(board, pieceLocation, moveToLocation, moves);
            bool enpassant = false;
            bool promotion = false;


            int currentColumn = pieceLocation[0] - '0';
            int currentRow = pieceLocation[2] - '0';

            int nextColumn = moveToLocation[0] - '0';
            int nextRow = moveToLocation[2] - '0';

            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;

            if (!canIt && currentColumn == 3 && board[currentColumn,currentRow].getColor() == 'B' && currentMinusNextColumn == -1 && currentMinusNextRow == -1 && board[currentColumn, currentRow - 1] is Pawn && board[nextColumn, nextRow] is Blank)
            {
                if (moves - ((Pawn)board[currentColumn, currentRow - 1]).getEnPassant() == 1)
                {
                    EnPassant(board, currentColumn, currentRow, nextColumn, nextRow, currentRow - 1);
                    enpassant = true;
                }
            }
            if (!canIt && currentRow == 3  && board[currentColumn, currentRow].getColor() == 'B' && currentMinusNextColumn == -1 && currentMinusNextRow == 1 && board[currentColumn, currentRow + 1] is Pawn && board[nextColumn, nextRow] is Blank)
            {
                if (moves - ((Pawn)board[currentColumn, currentRow + 1]).getEnPassant() == 1)
                {
                    EnPassant(board, currentColumn, currentRow, nextColumn, nextRow, currentRow + 1);
                    enpassant = true;
                }
            }
            if (!canIt && currentColumn == 4 && board[currentColumn,currentRow].getColor() == 'W'  && currentMinusNextColumn == 1 && currentMinusNextRow == 1 && board[currentColumn, currentRow + 1] is Pawn && board[nextColumn, nextRow] is Blank)
            {
                if (moves - ((Pawn)board[currentColumn, currentRow + 1]).getEnPassant() == 1)
                {
                    EnPassant(board, currentColumn, currentRow, nextColumn, nextRow, currentRow + 1);
                    enpassant = true;
                }
            }
            if (!canIt && currentColumn == 4 && board[currentColumn, currentRow].getColor() == 'W'  && currentMinusNextColumn == 1 && currentMinusNextRow == -1 && board[currentColumn, currentRow - 1] is Pawn && board[nextColumn, nextRow] is Blank)
            {
                if (moves - ((Pawn)board[currentColumn, currentRow - 1]).getEnPassant() == 1)
                {
                    EnPassant(board, currentColumn, currentRow, nextColumn, nextRow, currentRow - 1);
                    enpassant = true;
                }
            }
            if (!canIt && board[currentColumn, currentRow].getColor() == 'W' && currentColumn == 6 && nextColumn == 7)
            {
                Promotion(board, currentColumn, currentRow, nextColumn, nextRow, 'W');
                promotion = true;
            }
            if (!canIt && board[currentColumn, currentRow].getColor() == 'B' && currentColumn == 1 && nextColumn == 0)
            {
                Promotion(board, currentColumn, currentRow, nextColumn, nextRow, 'B');
                promotion = true;
            }


            string[] result = new string[5];
            result[0] = canIt + "";
            result[1] = pieceLocation;
            result[2] = moveToLocation;
            result[3] = enpassant + "";
            result[4] = promotion + "";
            return result;
        }
        static void Promotion(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, char color)
        {
            int count = 1;
            board[currentColumn, currentRow] = new Blank('E');
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
        }
        static void EnPassant(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, int newrow)
        {
            board[currentColumn, newrow] = new Blank('E');
            ChessPiece temp = board[currentColumn, currentRow];
            board[nextColumn, nextRow] = temp;
            board[currentColumn, currentRow] = new Blank('E');
            printBoard(board);
        }

        static bool LackOfPieces(ChessPiece[,] board)
        {
            int wCount = 0;
            int bCount = 0;
            for(int i = 0; i < board.GetLength(0); i++)
            {
                for(int g = 0; g < board.GetLength(1); g++)
                {
                    if(board[i, g].getColor() == 'W')
                        wCount++;

                    if(board[i,g].getColor() == 'B')
                        bCount++;
                }
            }
            if(wCount == 1 && bCount == 1)
                return true;

            if(wCount == 1 && bCount == 2)
            {
                int count = 0;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int g = 0; g < board.GetLength(1); g++)
                    {
                        if (board[i, g].getColor() == 'W' && board[i, g] is King)
                            count++;

                        if (board[i, g].getColor() == 'B' && (board[i, g] is Bishop || board[i, g] is Knight || board[i,g] is King))
                            count++;
                    }
                }
                if (count == 3)
                {
                    return true;
                }
            }
            if(wCount == 2 && bCount == 1)
            {
                int count = 0;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int g = 0; g < board.GetLength(1); g++)
                    {
                        if (board[i, g].getColor() == 'B' && board[i, g] is King)
                            count++;

                        if (board[i, g].getColor() == 'W' && (board[i, g] is Bishop || board[i, g] is Knight))
                            count++;
                    }
                }
                if(count == 3)
                {
                    return true;
                }
            }
            return false;
        }
        static bool canDoThatMove(ChessPiece[,] board, string pieceLocation, string MovetoLocation, int moves)
        {
            // board[pieceLocationColumn, pieceLocationRow] is the soldier that user wants to move
            // board[column - 1, row] is the spot that the user wants to move it to.
            int pieceLocationColumn = pieceLocation[0] - '0';
            int pieceLocationRow = pieceLocation[2] - '0';

            int moveToLocationColumn = MovetoLocation[0] - '0';
            int moveToLocationRow = MovetoLocation[2] - '0';
            bool canIt = false;
            //Checking  which piece the user want to move, once detects then goes to its setmove function with
            //the current location minus the desired new location
            if (board[pieceLocationColumn, pieceLocationRow] is Pawn)
                canIt = ((Pawn)board[pieceLocationColumn, pieceLocationRow]).setMove( board, pieceLocationColumn , pieceLocationRow, moveToLocationColumn, moveToLocationRow, false, moves); // Finding out if the column is more than the pawn can make
            if (board[pieceLocationColumn, pieceLocationRow] is Blank)
                canIt = ((Blank)board[pieceLocationColumn, pieceLocationRow]).setMove( board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow, false, moves);
            if (board[pieceLocationColumn, pieceLocationRow] is Knight)
                canIt = ((Knight)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow, false, moves);
            if (board[pieceLocationColumn, pieceLocationRow] is Rook)
                canIt = ((Rook)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow, false, moves);
            if (board[pieceLocationColumn, pieceLocationRow] is Bishop)
                canIt = ((Bishop)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow, false, moves);
            if (board[pieceLocationColumn, pieceLocationRow] is King)
                canIt = ((King)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow, false, moves);
            if (board[pieceLocationColumn, pieceLocationRow] is Queen)
                canIt = ((Queen)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, moveToLocationColumn, moveToLocationRow, false, moves);

            return canIt;
        }
        static ChessPiece[,] Moving(ChessPiece[,] board, string pieceLocation, string moveToLocation, int moves, ChessPieceCount eatPiece)
        {
            int pieceLocationColumn = pieceLocation[0] - '0';  // The piece location before moves
            int pieceLocationRow = pieceLocation[2] - '0';
            int column = moveToLocation[0] - '0';
            int row = moveToLocation[2] - '0';
            int Achraha = 0;
            char color = 'S';
            bool isPossible = false;

            string classname = board[pieceLocationColumn, pieceLocationRow].GetType().Name;
            ChessPiece tempLocationPieceBeforeMoves = null;
            if (board[pieceLocationColumn, pieceLocationRow] is King && board[column, row] is Rook)
            {
                isPossible = ((King)board[pieceLocationColumn, pieceLocationRow]).setMove(board, pieceLocationColumn, pieceLocationRow, column, row, true, moves);
                if (isPossible)
                {
                    Achraha++;
                    tempLocationPieceBeforeMoves = (King)board[pieceLocationColumn, pieceLocationRow];
                    if (board[pieceLocationColumn, pieceLocationRow].getColor() == 'B')
                    {
                        color = 'B';
                        if (column == 7 && row == 0)
                            board[pieceLocationColumn, pieceLocationRow - 2] = tempLocationPieceBeforeMoves; //king

                        if (column == 7 && row == 7)
                            board[pieceLocationColumn, pieceLocationRow + 2] = tempLocationPieceBeforeMoves;


                    }
                    if (board[pieceLocationColumn, pieceLocationRow].getColor() == 'W')
                    {
                        color = 'W';
                        if (column == 0 && row == 0)
                            board[pieceLocationColumn, pieceLocationRow - 2] = tempLocationPieceBeforeMoves;

                        if (column == 0 && row == 7)
                            board[pieceLocationColumn, pieceLocationRow + 2] = tempLocationPieceBeforeMoves;
                    }
                    board[pieceLocationColumn, pieceLocationRow] = new Blank('E');
                }
            }
            if (Achraha == 0)
            {
                if (classname == "Pawn")
                    tempLocationPieceBeforeMoves = (Pawn)board[pieceLocationColumn, pieceLocationRow];

                if (classname == "Blank")
                    tempLocationPieceBeforeMoves = (Blank)board[pieceLocationColumn, pieceLocationRow];

                if (classname == "Knight")
                    tempLocationPieceBeforeMoves = (Knight)board[pieceLocationColumn, pieceLocationRow];

                if (classname == "Rook")
                    tempLocationPieceBeforeMoves = (Rook)board[pieceLocationColumn, pieceLocationRow];

                if (classname == "Bishop")
                    tempLocationPieceBeforeMoves = (Bishop)board[pieceLocationColumn, pieceLocationRow];

                if (classname == "King")
                    tempLocationPieceBeforeMoves = (King)board[pieceLocationColumn, pieceLocationRow];

                if (classname == "Queen")
                    tempLocationPieceBeforeMoves = (Queen)board[pieceLocationColumn, pieceLocationRow];
            }



            // board[pieceLocationColumn, pieceLocationRow] is the soldier that user wants to move
            // board[column - 1, row] is the spot that the user wants to move it to.

            string classname2 = board[column, row].GetType().Name;
            ChessPiece tempNewLocation = null;

            if (Achraha > 0)
                tempNewLocation = (Rook)board[column, row];


            if (Achraha == 0)
            {
                if (classname2 == "Pawn")
                    tempNewLocation = (Pawn)board[column, row];

                else if (classname2 == "Blank")
                    tempNewLocation = (Blank)board[column, row];

                else if (classname2 == "Knight")
                    tempNewLocation = (Knight)board[column, row];

                else if (classname2 == "Rook")
                    tempNewLocation = (Rook)board[column, row];

                else if (classname2 == "Bishop")
                    tempNewLocation = (Bishop)board[column, row];

                else if (classname2 == "King")
                    tempNewLocation = (King)board[column, row];

                else if (classname2 == "Queen")
                    tempNewLocation = (Queen)board[column, row];
            }

            if (board[column, row] is Blank && Achraha == 0)
            {
                board[pieceLocationColumn, pieceLocationRow] = tempNewLocation;
                eatPiece.setCount();
            }
            else if (Achraha == 0)
            {
                if (board[pieceLocationColumn, pieceLocationRow].getColor() != board[column, row].getColor())
                    Console.WriteLine("You ate his " + board[column, row].GetType().Name);
                board[pieceLocationColumn, pieceLocationRow] = new Blank('E');
                eatPiece.restartCount();
                if (board[column, row].GetType().Name == "King")
                    board[0, 0] = null;

            }
            if(Achraha == 0)
                board[column, row] = tempLocationPieceBeforeMoves;
            else
            {
                if (color == 'B')
                {
                    if (column == 7 && row == 0)
                        board[column, row + 3] = tempNewLocation;


                    if(column == 7 && row == 7)
                        board[column, row - 2] = tempNewLocation;
                }
                if(color == 'W')
                {

                    if(column == 0 && row == 0)
                        board[column, row + 3] = tempNewLocation;


                    if(column == 0 && row == 7)
                        board[column, row - 2] = tempNewLocation;

                }
                board[column, row] = new Blank('E');
            }

            
            return board;
        }

        static string changeTo(ChessPiece[,] board, string moveTo)   //7|1
        {
            string tempString = moveTo.ToUpper();
            int column = (char)tempString[1] - '0';     // converts the number as char to int
            int row = 0;
            switch ((char)tempString[0])
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
            string moveToLocation = "" + (column - 1) + "|" + row;
            return moveToLocation;
        }


        static string[] Check(ChessPiece[,] board, int moves)
        {
            int tempWhiteKingcolumn = 0;
            int tempWhiteKingRow = 0;

            int tempBlackKingcolumn = 0;
            int tempBlackKingRow = 0;
            string[] result = new string[3];

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for(int g = 0; g < board.GetLength(1); g++)
                {
                    if(board[i, g].getColor() == 'W' && board[i,g] is King)
                    {
                        tempWhiteKingcolumn = i;
                        tempWhiteKingRow = g;
                    }
                    if(board[i,g].getColor() == 'B' && board[i,g] is King)
                    {
                        tempBlackKingcolumn = i;
                        tempBlackKingRow = g;
                    }
                }
            }
            int currentkingcolumn = 0;
            int currentkingrow = 0;
            bool check = false;
            int count = 0;
            bool isCheck = false;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    if(!(board[i, g] is Blank))
                    {

                        currentkingcolumn = board[i, g].getColor() == 'W' ? tempBlackKingcolumn : tempWhiteKingcolumn;
                        currentkingrow = currentkingcolumn == tempWhiteKingcolumn ? tempWhiteKingRow : tempBlackKingRow;

                        if (board[i, g] is Pawn)
                        {
                            check = ((Pawn)board[i, g]).setMove(board, i, g, currentkingcolumn, currentkingrow, true, moves);
                            if(check is true) { count++; }
                        }
                        if (board[i, g] is Rook)
                        {
                            check = ((Rook)board[i, g]).setMove(board, i, g, currentkingcolumn, currentkingrow, true, moves);
                            if (check is true) { count++; }
                        }
                        if (board[i, g] is Bishop)
                        {
                            check = ((Bishop)board[i, g]).setMove(board, i, g, currentkingcolumn, currentkingrow, true, moves);
                            if (check is true) { count++; }
                        }
                        if (board[i, g] is Knight)
                        {
                            check = ((Knight)board[i, g]).setMove(board, i, g, currentkingcolumn, currentkingrow, true, moves);
                            if (check is true) { count++; }
                        }
                        if (board[i, g] is Queen)
                        {
                            check = ((Queen)board[i, g]).setMove(board, i, g, currentkingcolumn, currentkingrow, true, moves);
                            if (check is true) { count++; }
                        }
                    }
                }
            }
            if(count > 0) { isCheck = true; }
            result[0] = isCheck + "";
            result[1] = currentkingcolumn + "";
            result[2] = currentkingrow + "";
            return result; 
        }
        static bool checkMate(ChessPiece[,] board, int currentColumn, int currentRow, int moves)
        {
            bool isCheckMate = false;
            int newColumn = currentColumn - 1; // Up
            int newRow = currentRow;
            if (newColumn <= 7 && newColumn >= 0 && newRow <= 7 && newRow >= 0)
            {
                isCheckMate = ((King)board[currentColumn, currentRow]).setMove(board, currentColumn, currentRow, newColumn, newRow, true, moves);
                if (isCheckMate)
                {
                    ChessPiece tempnew = board[newColumn, newRow];
                    ChessPiece tempold = board[currentColumn, currentRow];
                    board[newColumn, newRow] = tempold;
                    board[currentColumn, currentRow] = tempnew;
                    if (Check(board, moves)[0] == "False")
                        isCheckMate = false;

                    board[newColumn, newRow] = tempnew;
                    board[currentColumn, currentRow] = tempold;
                    if (!isCheckMate) { return isCheckMate; }
                }
            }
            newColumn = currentColumn + 1; // Down
            if (newColumn <= 7 && newColumn >= 0 && newRow <= 7 && newRow >= 0)
            {
                isCheckMate = ((King)board[currentColumn, currentRow]).setMove(board, currentColumn, currentRow, newColumn, newRow, true, moves);
                if (isCheckMate)
                {
                    ChessPiece tempnew = board[newColumn, newRow];
                    ChessPiece tempold = board[currentColumn, currentRow];
                    board[newColumn, newRow] = tempold;
                    board[currentColumn, currentRow] = tempnew;
                    if (Check(board, moves)[0] == "False")
                        isCheckMate = false;

                    board[newColumn, newRow] = tempnew;
                    board[currentColumn, currentRow] = tempold;
                    if (!isCheckMate) { return isCheckMate; }
                }
            }

            newColumn = currentColumn; // left
            newRow = currentRow - 1;
            if (newColumn <= 7 && newColumn >= 0 && newRow <= 7 && newRow >= 0)
            {
                isCheckMate = ((King)board[currentColumn, currentRow]).setMove(board, currentColumn, currentRow, newColumn, newRow, true, moves);
                if (isCheckMate)
                {
                    ChessPiece tempnew = board[newColumn, newRow];
                    ChessPiece tempold = board[currentColumn, currentRow];
                    board[newColumn, newRow] = tempold;
                    board[currentColumn, currentRow] = tempnew;
                    if (Check(board, moves)[0] == "False")
                        isCheckMate = false;

                    board[newColumn, newRow] = tempnew;
                    board[currentColumn, currentRow] = tempold;
                    if (!isCheckMate) { return isCheckMate; }
                }
            }

            newColumn = currentColumn; // right
            newRow = currentRow + 1;
            if (newColumn <= 7 && newColumn >= 0 && newRow <= 7 && newRow >= 0)
            {
                isCheckMate = ((King)board[currentColumn, currentRow]).setMove(board, currentColumn, currentRow, newColumn, newRow, true, moves);
                if (isCheckMate)
                {
                    ChessPiece tempnew = board[newColumn, newRow];
                    ChessPiece tempold = board[currentColumn, currentRow];
                    board[newColumn, newRow] = tempold;
                    board[currentColumn, currentRow] = tempnew;
                    if (Check(board, moves)[0] == "False")
                        isCheckMate = false;

                    board[newColumn, newRow] = tempnew;
                    board[currentColumn, currentRow] = tempold;
                    if (!isCheckMate) { return isCheckMate; }
                }
            }

            newColumn = currentColumn - 1; // top left diagonally
            newRow = currentRow - 1;
            if (newColumn <= 7 && newColumn >= 0 && newRow <= 7 && newRow >= 0)
            {
                isCheckMate = ((King)board[currentColumn, currentRow]).setMove(board, currentColumn, currentRow, newColumn, newRow, true, moves);
                if (isCheckMate)
                {
                    ChessPiece tempnew = board[newColumn, newRow];
                    ChessPiece tempold = board[currentColumn, currentRow];
                    board[newColumn, newRow] = tempold;
                    board[currentColumn, currentRow] = tempnew;
                    if (Check(board, moves)[0] == "False")
                        isCheckMate = false;

                    board[newColumn, newRow] = tempnew;
                    board[currentColumn, currentRow] = tempold;
                    if (!isCheckMate) { return isCheckMate; }
                }
            }
            newColumn = currentColumn - 1;//top right diagonally
            newRow = currentRow + 1;
            if (newColumn <= 7 && newColumn >= 0 && newRow <= 7 && newRow >= 0)
            {
                isCheckMate = ((King)board[currentColumn, currentRow]).setMove(board, currentColumn, currentRow, newColumn, newRow, true, moves);
                if (isCheckMate)
                {
                    ChessPiece tempnew = board[newColumn, newRow];
                    ChessPiece tempold = board[currentColumn, currentRow];
                    board[newColumn, newRow] = tempold;
                    board[currentColumn, currentRow] = tempnew;
                    if (Check(board, moves)[0] == "False")
                        isCheckMate = false;

                    board[newColumn, newRow] = tempnew;
                    board[currentColumn, currentRow] = tempold;
                    if (!isCheckMate) { return isCheckMate; }
                }
            }
            newColumn = currentColumn + 1; // bottom left diagonally
            newRow = currentRow - 1;


                if (newColumn <= 7 && newColumn >= 0 && newRow <= 7 && newRow >= 0)
                {
                    isCheckMate = ((King)board[currentColumn, currentRow]).setMove(board, currentColumn, currentRow, newColumn, newRow, true, moves);
                    if (isCheckMate)
                    {
                        ChessPiece tempnew = board[newColumn, newRow];
                        ChessPiece tempold = board[currentColumn, currentRow];
                        board[newColumn, newRow] = tempold;
                        board[currentColumn, currentRow] = tempnew;
                        if (Check(board, moves)[0] == "False")
                            isCheckMate = false;

                        board[newColumn, newRow] = tempnew;
                        board[currentColumn, currentRow] = tempold;
                        if (!isCheckMate) { return isCheckMate; }
                    }
                }
            newColumn = currentColumn + 1; // bottom right diagonally
            newRow = currentRow + 1;
            if (newColumn <= 7 && newColumn >= 0 && newRow <= 7 && newRow >= 0)
            {
                isCheckMate = ((King)board[currentColumn, currentRow]).setMove(board, currentColumn, currentRow, newColumn, newRow, true, moves);
                if (isCheckMate)
                {
                    ChessPiece tempnew = board[newColumn, newRow];
                    ChessPiece tempold = board[currentColumn, currentRow];
                    //ChessPiece tempblank = new Blank('E');
                    board[newColumn, newRow] = tempold;
                    board[currentColumn, currentRow] = tempnew;
                    //board[currentColumn, currentRow] = tempblank;
                    if (Check(board, moves)[0] == "False")
                        isCheckMate = false;

                    board[newColumn, newRow] = tempnew;
                    board[currentColumn, currentRow] = tempold;
                    if (!isCheckMate) { return isCheckMate; }
                }
            }
            if (isCheckMate)
                return true;
            return false;
        }
        static bool stalemate(ChessPiece[,] board)
        {
            int blackKingColumn = 0;
            int blackKingRow = 0;
            int whiteKingColumn = 0;
            int whiteKingRow = 0;
            for(int i = 0; i < board.GetLength(0); i++)
            {
                for(int g = 0; g < board.GetLength(1); g++)
                {
                    if(!(board[i, g] is Blank))
                    {
                        if(board[i, g] is King)
                        {
                            if (board[i, g].getColor() == 'B')
                            {
                                blackKingColumn = i;
                                blackKingRow = g;
                            }
                            if(board[i, g].getColor() == 'W')
                            {
                                whiteKingColumn = i;
                                whiteKingRow = g;
                            }
                        }

                        if(board[i, g] is Pawn)
                        {
                            if(board[i, g].getColor() == 'W')
                            {
                                int newrow = g;
                                int newcolumn = i + 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow] is Blank) { return false; } // if going one square down, then return false
                                newrow = g - 1;
                                newcolumn = i - 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow].getColor() == 'B') { return false; } // if going down left diagonally and there is black piece, return false
                                newrow = g + 1;
                                newcolumn = i + 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow].getColor() == 'B') { return false; }
                            }
                            if (board[i, g].getColor() == 'B')
                            {
                                int newrow = g;
                                int newcolumn = i - 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow] is Blank) { return false; } // if going one square up, then return false
                                newrow = g - 1;
                                newcolumn = i - 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow].getColor() == 'W') { return false; } // if going up left diagonally and there is white piece, return false
                                newrow = g + 1;
                                newcolumn = i - 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow].getColor() == 'W') { return false; }
                            }
                        }
                        if(board[i, g] is Bishop)
                        {
                            char color = board[i, g].getColor();
                            int newrow = g - 1;
                            int newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)){ return false; }//if one square left diagonally is blank or black, return false
                            newrow = g + 1;
                            newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g - 1;
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g + 1;
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                        }
                        if(board[i, g] is Rook)
                        {
                            char color = board[i, g].getColor();
                            int newrow = g + 1;
                            int newcolumn = i;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g;
                            newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                        }
                        if(board[i, g] is Knight)
                        {
                            char color = board[i, g].getColor();
                            int newrow = g + 2;
                            int newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g + 1;
                            newcolumn = i - 2;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g - 2;
                            newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newcolumn = i + 2;
                            newrow = g - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                        }
                        if (board[i, g] is Queen)
                        {
                            char color = board[i, g].getColor();
                            int newrow = g - 1;
                            int newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }//if one square left diagonally is blank or black, return false
                            newrow = g + 1;
                            newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g - 1;
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g + 1;
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }


                            newrow = g;
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; } // if going one square down, then return false
                            newrow = g - 1;
                            newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; } // if going down left diagonally and there is black piece, return false
                            newrow = g + 1;
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                            newrow = g;
                            newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; } // if going one square up, then return false
                            newrow = g - 1;
                            newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; } // if going up left diagonally and there is white piece, return false
                            newrow = g + 1;
                            newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color)) { return false; }
                        }
                    }
                }
            }
            return true;
        }
                 
        static bool swapForCheck(ChessPiece[,] board, int newcolumn, int newrow, int oldcolumn, int oldrow, int moves)
        {
            ChessPiece tempnew = board[newcolumn, newrow];
            ChessPiece tempold = board[oldcolumn, oldrow];
            board[newcolumn, newrow] = tempold;
            board[oldcolumn, oldrow] = tempnew;
            if(Check(board, moves)[0] == "False")
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
        }
        static bool isstill(ChessPiece[,] board, int currentkingcolumn, int currentkingrow, int moves)
        {
            char color = board[currentkingcolumn, currentkingrow].getColor();
            bool stop = false;
            for(int i = 0; i < board.GetLength(0); i++)
            {
                for (int g = 0; g < board.GetLength(1); g++)
                {
                    if(!(board[i, g] is Blank) && board[i,g].getColor() == color)
                    {
                        if(board[i, g] is Rook)
                        {
                            for(int j = i - 1;!stop && j <= 7 && j >=0; j--) // UP only column changes
                            {
                                if(board[j, g] is Blank || board[j, g].getColor() != board[i, g].getColor())
                                {

                                    if(swapForCheck(board, j, g, i, g, moves) is false)
                                    {
                                        return false; //IF this is false, it means it swapped the new row and column and checked if there is still a check, if it returned false, then there is no checkmate
                                    }
                                    

                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for(int j = i + 1; !stop && j <=7 && j>=0; j++)
                            {
                                if(board[j, g] is Blank || board[j, g].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, j, g, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for(int newrow = g + 1; !stop && newrow <= 7 && newrow >= 0; newrow++)
                            {
                                if(board[i, newrow] is Blank || board[i, newrow].getColor()!=board[i, g].getColor())
                                {
                                    if (swapForCheck(board, i, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for(int newrow = g - 1; !stop && newrow <= 7 && newrow >= 0; newrow--)
                            {
                                if(board[i, newrow] is Blank || board[i, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, i, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                        }
                        if(board[i, g] is Bishop)
                        {
                            for(int newcolumn = i - 1, newrow = g + 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn--, newrow++)
                            {
                                if(board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true;  }
                            }
                            stop = false;
                            for(int newcolumn = i - 1, newrow = g - 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn--, newrow--)
                            {
                                if (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for(int newcolumn = i + 1, newrow = g - 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn++, newrow--)
                            {
                                if (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for(int newcolumn = i+1, newrow= g + 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn++, newrow++)
                            {
                                if (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                        }
                        if(board[i, g] is Knight)
                        {
                            char color2 = board[i, g].getColor();
                            int newrow = g + 2;
                            int newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color2))
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                    return false;
                            }
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color2))
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                    return false;
                            }
                            newrow = g + 1;
                            newcolumn = i - 2;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color2))
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                    return false;
                            }
                            newrow = g - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color2))
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                    return false;
                            }
                            newrow = g - 2;
                            newcolumn = i - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color2))
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                    return false;
                            }
                            newcolumn = i + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color2))
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                    return false;
                            }
                            newcolumn = i + 2;
                            newrow = g - 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color2))
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                    return false;
                            }
                            newrow = g + 1;
                            if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != color2))
                            {
                                if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                    return false;
                            }


                        }
                        if(board[i, g] is Pawn)
                        {
                            if (board[i, g].getColor() == 'W')
                            {
                                int newrow = g;
                                int newcolumn = i + 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow] is Blank)
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                } // if going one square down, then return false
                                newrow = g - 1;
                                newcolumn = i - 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow].getColor() == 'B')
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                } // if going down left diagonally and there is black piece, return false
                                newrow = g + 1;
                                newcolumn = i + 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow].getColor() == 'B')
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                            }
                            if (board[i, g].getColor() == 'B')
                            {
                                int newrow = g;
                                int newcolumn = i - 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow] is Blank) 
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                } // if going one square up, then return false
                                newrow = g - 1;
                                newcolumn = i - 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow].getColor() == 'W')
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                } // if going up left diagonally and there is white piece, return false
                                newrow = g + 1;
                                newcolumn = i - 1;
                                if ((newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0) && board[newcolumn, newrow].getColor() == 'W')
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                            }
                        }
                        if(board[i, g] is Queen)
                        {
                            for (int j = i - 1; !stop && j <= 7 && j >= 0; j--) // UP only column changes
                            {
                                if (board[j, g] is Blank || board[j, g].getColor() != board[i, g].getColor())
                                {

                                    if (swapForCheck(board, j, g, i, g, moves) is false)
                                    {
                                        return false; //IF this is false, it means it swapped the new row and column and checked if there is still a check, if it returned false, then there is no checkmate
                                    }


                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for (int j = i + 1; !stop && j <= 7 && j >= 0; j++)
                            {
                                if (board[j, g] is Blank || board[j, g].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, j, g, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for (int newrow = g + 1; !stop && newrow <= 7 && newrow >= 0; newrow++)
                            {
                                if (board[i, newrow] is Blank || board[i, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, i, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for (int newrow = g - 1; !stop && newrow <= 7 && newrow >= 0; newrow--)
                            {
                                if (board[i, newrow] is Blank || board[i, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, i, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for (int newcolumn = i - 1, newrow = g + 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn--, newrow++)
                            {
                                if (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for (int newcolumn = i - 1, newrow = g - 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn--, newrow--)
                            {
                                if (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for (int newcolumn = i + 1, newrow = g - 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn++, newrow--)
                            {
                                if (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                            for (int newcolumn = i + 1, newrow = g + 1; !stop && newcolumn <= 7 && newcolumn >= 0 && newrow <= 7 && newrow >= 0; newcolumn++, newrow++)
                            {
                                if (board[newcolumn, newrow] is Blank || board[newcolumn, newrow].getColor() != board[i, g].getColor())
                                {
                                    if (swapForCheck(board, newcolumn, newrow, i, g, moves) is false)
                                        return false;
                                }
                                else { stop = true; }
                            }
                            stop = false;
                        }
                    }
                }
            }
            return true;
        }
    }
    interface Piece
    {
        void setColor(char color);
        void setName(char color);
        bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves);
        int getTotalMoves();

    }

    class ChessPiece
    {
        int totalMoves;
        char color;
        public ChessPiece(char color)
        {
            setColor(color);
        }

        public void setColor(char color)
        {
            if (color == 'W' || color == 'B' || color == 'E')
            {
                this.color = color;
            }
            else
                Console.WriteLine("You entered an invalid color, choose W for white or B for black");
            return;
        }
        public char getColor()
        {
            return color;
        }
        public void setTotalMove()
        {
            totalMoves++;
        }
    }
    class ChessPieceCount
    {
        int count;
        public ChessPieceCount() { }
        public void setCount()
        {
            count++;
        }
        public void restartCount()
        {
            count = 0;
        }
        public int getCount()
        {
            return count;
        }
    }
    class Blank : ChessPiece, Piece
    {
        string name;
        public Blank(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board,int column, int move, int row, int colum, bool check, int moves)
        {
            if(!check)
                 Console.WriteLine("Empty spot cannot be chosen");
            return false;
        }

        public void setName(char color)
        {
            name = color == 'E' ? "EE" : "";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            int totalMoves = 0;
            return totalMoves;
        }
    }
    class Pawn : ChessPiece, Piece
    {
        int totalMoves = 0;
        string name;
        int EnPassant = 0; //הכאה דרך הילוכו
        public Pawn(char color) : base(color)
        {
            setName(color);
        }
        public void setName(char color)
        {
            this.name = color == 'W' ? "WP" : "BP";
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves) // 1 0
        {
            char currentPieceColor = this.getColor();
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = false;
            if (board[nextColumn, nextRow].getColor() == currentPieceColor)
            {
                string pieceName = board[nextColumn, nextRow].GetType().Name;
                if(!check)
                    Console.WriteLine("You cant eat your own {0}", pieceName);
                return false;
            }
            if(!check && totalMoves == 0) { this.EnPassant += moves; }


            if(this.getColor() == 'B' && currentColumn == 3 && currentMinusNextColumn == -1 && currentMinusNextRow == -1 && board[currentColumn, currentRow - 1] is Pawn && board[nextColumn, nextRow] is Blank)
            {
                if(moves - ((Pawn)board[currentColumn, currentRow - 1]).getEnPassant() == 1)
                {
                    return false;
                }
            }
            if (this.getColor() == 'B' && currentColumn == 3 && currentMinusNextColumn == -1 && currentMinusNextRow == 1 && board[currentColumn, currentRow + 1] is Pawn && board[nextColumn, nextRow] is Blank)
            {
                if (moves - ((Pawn)board[currentColumn, currentRow + 1]).getEnPassant() == 1)
                {
                    return false;
                }
            }
            if (this.getColor() == 'W' && currentColumn == 4 && currentMinusNextColumn == 1 && currentMinusNextRow == 1 && board[currentColumn, currentRow + 1] is Pawn && board[nextColumn, nextRow] is Blank)
            {
                if(moves - ((Pawn)board[currentColumn, currentRow + 1]).getEnPassant() == 1)
                {
                    return false;
                }
            }
            if (this.getColor() == 'W' && currentColumn == 4 && currentMinusNextColumn == 1 && currentMinusNextRow == -1 && board[currentColumn, currentRow - 1] is Pawn && board[nextColumn, nextRow] is Blank)
            {
                if (moves - ((Pawn)board[currentColumn, currentRow - 1]).getEnPassant() == 1)
                {
                    return false;
                }
            }


            //-----------------------
            if (totalMoves == 0)
            {
                if (this.getColor() == 'B')
                {
                    if ((currentMinusNextColumn == -1 || currentMinusNextColumn == -2) && currentMinusNextRow == 0)
                    {
                        if (currentMinusNextColumn == -2)
                        {
                            if (board[currentColumn - 1, currentRow] is Blank && board[nextColumn, nextRow] is Blank)
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
                        if (currentMinusNextColumn == -1)
                        {
                            if (board[nextColumn, nextRow] is Blank)
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
                    if ((currentMinusNextColumn == 2 || currentMinusNextColumn == 1) && currentMinusNextRow == 0)
                    {
                        if (currentMinusNextColumn == 2)
                        {
                            if (board[currentColumn + 1, currentRow] is Blank && board[nextColumn, nextRow] is Blank)
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
                        if (currentMinusNextColumn == 1)
                        {
                            if (board[nextColumn, nextRow] is Blank)
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
                    if(currentMinusNextColumn == -1 && currentMinusNextRow == 0 && board[nextColumn, nextRow] is Blank)
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }


                    else if(board[nextColumn, nextRow].getColor() == 'W' && currentMinusNextColumn == -1 && (currentMinusNextRow == -1 || currentMinusNextRow == 1))
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }
                    else
                    {
                        if (!check)
                            Console.WriteLine("Pawn can only move one square forward or eat one square diagonally");
                        return false;
                    }
                }
                if(board[currentColumn, currentRow].getColor() == 'W')
                {
                    if(currentMinusNextColumn == 1 && currentMinusNextRow == 0 && board[nextColumn, nextRow] is Blank)
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }
                    else if (board[nextColumn, nextRow].getColor() == 'B' && currentMinusNextColumn == 1 && (currentMinusNextRow == 1 || currentMinusNextRow == -1))
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }
                    else
                    {
                        if (!check)
                            Console.WriteLine("Pawn can only move one square forward or eat one square diagonally");
                        return false;
                    }
                }







            //-----------------------
            /*if (currentMinusNextRow != 0 && (currentMinusNextRow == -1 || currentMinusNextRow == 1) && (currentMinusNextColumn == -1 || currentMinusNextColumn == 1))
            {
                if (board[nextColumn, nextRow] is Blank)
                {
                    if (!check)
                        Console.WriteLine("Pawn can only eat that direction, other than that only move forward");
                    return false;
                }
                else
                {
                    if (!check)
                        totalMoves++;
                    isPossible = true;
                }

            }
            else if(!isPossible && totalMoves > 0 && (currentMinusNextRow > 1 || currentMinusNextRow < -1 || currentMinusNextColumn > 1 || currentMinusNextColumn < -1))
            {
                if (!check)
                    Console.WriteLine("Pawn can only eat one square diagonally");
                return false;
            }
            if (totalMoves == 0)
            {
                if (currentColumn > nextColumn) 
                {
                    bool anyPieceBlocking = false;
                    for (int i = currentColumn - 1; !anyPieceBlocking && i >= nextColumn; i--)
                    {
                        if(!(board[i, currentRow] is Blank))
                        {
                            anyPieceBlocking = true;
                            if (!check)
                                Console.WriteLine("There is other piece blocking you from doing that move");
                            return false;
                        }
                    }
                }
                if(currentColumn < nextColumn)
                {
                    bool anyPieceBlocking = false;
                    for (int i = currentColumn + 1; !anyPieceBlocking && i <= nextColumn; i++)
                    {
                        if (!(board[i, currentRow] is Blank))
                        {
                            anyPieceBlocking = true;
                            if (!check)
                                Console.WriteLine("There is other piece blocking you from doing that move");
                            return false;
                        }
                    }
                }
                if (currentPieceColor == 'W')
                    if (currentMinusNextRow == 0 && (currentMinusNextColumn == 1 || currentMinusNextColumn == 2))
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }
                if (currentPieceColor == 'B')
                    if (currentMinusNextRow == 0 && (currentMinusNextColumn == -1 || currentMinusNextColumn == -2))
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }
            }
            if(totalMoves > 0)
            {
                if (!isPossible && currentPieceColor == 'W')
                    if (currentMinusNextColumn == 1 && currentMinusNextRow == 0)
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }
                if (!isPossible && currentPieceColor == 'B')
                    if (currentMinusNextColumn == -1 && currentMinusNextRow == 0)
                    {
                        if (!check)
                            totalMoves++;
                        isPossible = true;
                    }
                if (!(board[nextColumn, nextRow] is Blank))
                {
                    if (!check)
                        Console.WriteLine("Cannot do that move, there is other piece blocking you");
                    return false;
                }
            }*/
            if (!check && isPossible && this.getColor() == 'W' && currentColumn == 6 && nextColumn == 7 && !(board[nextColumn, nextRow] is King))
            {
                return false;
            }
            if (!check && isPossible && this.getColor() == 'B' && currentColumn == 1 && nextColumn == 0 && !(board[nextColumn, nextRow] is King))
            {
                return false;
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
            return name;
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
        public Knight(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            if (currentMinusNextColumn == 1 && currentMinusNextRow == -2||currentMinusNextColumn == 1 && currentMinusNextRow == 2 ||currentMinusNextColumn == -2 && currentMinusNextRow == 1 || currentMinusNextColumn == 2 && currentMinusNextRow == 1 || currentMinusNextColumn == 2 && currentMinusNextRow == -1 || currentMinusNextColumn == -2 && currentMinusNextRow == -1 || currentMinusNextColumn == -1 && currentMinusNextRow == 2 || currentMinusNextColumn == -1 && currentMinusNextRow == -2)
            {
                if (!check)
                    totalMoves++;
                return true;
            }
            if (!check)
                Console.WriteLine("Knight cant do that move, it can only move in L shape");
            return false;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WN" : "BN";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
    }
    class Rook : ChessPiece, Piece
    {
        int totalMoves = -1;
        string name;
        public Rook(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            int count = 0;
            if (board[nextColumn, nextRow].getColor() == this.getColor())
            {
                string pieceName = board[nextColumn, nextRow].GetType().Name;
                if (!check)
                    Console.WriteLine("You can't eat your own {0}", pieceName);
                isPossible = false;
            }
            if (currentColumn > nextColumn && currentMinusNextRow == 0)
            {
                count++;
                for (int i = currentColumn - 1; isPossible &&  i > nextColumn;i--)
                {
                    if(!(board[i, currentRow] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if(currentRow > nextRow && currentMinusNextColumn == 0)
            {
                count++;
                for(int i = currentRow - 1;isPossible && i > nextRow; i--)
                {
                    if(!(board[currentColumn, i] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if(currentColumn < nextColumn && currentMinusNextRow == 0)
            {
                count++;
                for(int i = currentColumn + 1;isPossible && i < nextColumn; i++)
                {
                    if(!(board[i, currentRow] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if(currentRow > nextRow && currentMinusNextColumn == 0)
            {
                count++;
                for(int i = currentRow - 1; i > nextRow; i--)
                {
                    if(!(board[currentColumn, i] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if (count == 0)
                isPossible = false;
            if (!(isPossible) && !check)
            {
                Console.WriteLine("Rook cant move over other piece");
                return isPossible;
            }
            if (!check)
                totalMoves++;
            return isPossible;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WR" : "BR";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
        public void setTotalMoves()
        {
            totalMoves++;
        }
    }
    class Bishop : ChessPiece, Piece
    {
        int totalMoves = 0;
        string name;
        public Bishop(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            if(board[nextColumn, nextRow].getColor() == this.getColor())
            {
                if (board[nextColumn, nextRow] is King) { }
                else
                {
                    string pieceName = board[nextColumn, nextRow].GetType().Name;
                    if (!check)
                        Console.WriteLine("You can't eat your own {0}", pieceName);
                    return false;
                }
            }
            // --------------------------------------------------CHECKING FOR BLACK--------------------------------------
            if (currentColumn > nextColumn && currentRow > nextRow)
            {
                for (int i = currentColumn - 1, g = currentRow - 1; isPossible && i > nextColumn && g > nextRow; i--, g--)
                {
                    if (!(board[i, g] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if(currentColumn > nextColumn && currentRow < nextRow)
            {
                for(int i = currentColumn - 1, g = currentRow + 1; isPossible && i > nextColumn && g < nextRow; i--, g++)
                {
                    if(!(board[i, g] is Blank))
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
                    if (!(board[i, g] is Blank))
                    {
                        isPossible = false;
                    }
                }
            }
            if (currentColumn < nextColumn && currentRow > nextRow)
            {
                for (int i = currentColumn + 1, g = currentRow - 1; isPossible && i < nextColumn && g > nextRow; i++, g--)
                {
                    if (!(board[i, g] is Blank))
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
                if(!(tempNextColumn == tempNextRow))
                {
                    if(!check)
                        Console.WriteLine("Bishop can only move any number of squares diagonally");
                    isPossible = false;
                    return isPossible;
                }
            }

            if (!(isPossible) && !check)
            {
                Console.WriteLine("Bishop cannot go over other piece");
                return isPossible;
            }
            totalMoves++;
            return isPossible;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WB" : "BB";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
    }
    class King : ChessPiece, Piece
    {
        int totalMoves = -1;
        string name;
        public King(char color) : base(color)
        {
            setName(color);
        }
        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            if(board[nextColumn, nextRow] is Rook && board[nextColumn, nextRow].getColor() == this.getColor())
            {
                if(currentRow > nextRow && currentMinusNextColumn == 0)
                {
                    for(int i = currentRow - 1; isPossible && i > nextRow; i--)
                    {
                        if(!(board[currentColumn, i] is Blank))
                        {
                            isPossible = false;
                            if(!check)
                                Console.WriteLine("Castling isn't possible, there are pieces inbetween");
                            return isPossible;
                        }
                    }
                }
                if(currentRow < nextRow && currentMinusNextColumn == 0)
                {
                    for(int i = currentRow + 1; isPossible && i < nextRow; i++)
                    {
                        if(!(board[currentColumn, i] is Blank))
                        {
                            isPossible = false;
                            if(!check)
                                Console.WriteLine("Castling isn't possible, there are pieces inbetween");
                            return isPossible;
                        }
                    }
                }


                if(((Rook)board[nextColumn, nextRow]).getTotalMoves() == 0 || ((Rook)board[nextColumn, nextRow]).getTotalMoves() == -1)
                {
                    if(this.getTotalMoves() == 0 || this.getTotalMoves() == -1)
                    {
                        ((Rook)board[nextColumn, nextRow]).setTotalMoves();
                        isPossible = true;
                        if(!check)
                            totalMoves++;
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
                    if(!check)
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
            if(isPossible && board[nextColumn, nextRow].getColor() != this.getColor())
            {
                if (!check)
                    totalMoves++;
                isPossible = true;
                return isPossible;
            }
            if (isPossible && !(board[nextColumn, nextRow] is Blank))
            {
                if (!check)
                    Console.WriteLine("King cannot go on top of other piece");
                isPossible = false;
            }


            return isPossible;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WK" : "BK";
        }
        public override string ToString()
        {
            return name;
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }
    }
    class Queen : ChessPiece, Piece
    {
        string name;
        int totalMoves;
        public Queen(char color) : base(color)
        {
            setName(color);
        }
        public int getTotalMoves()
        {
            return totalMoves;
        }

        public bool setMove(ChessPiece[,] board, int currentColumn, int currentRow, int nextColumn, int nextRow, bool check, int moves)
        {
            int currentMinusNextColumn = nextColumn - currentColumn;
            int currentMinusNextRow = nextRow - currentRow;
            bool isPossible = true;
            bool diagonally = false;
            bool leftright = false;
            bool updown = false;
            char color = this.getColor();

            if (!diagonally)
            {
                int tempNextColumn = currentMinusNextColumn > 0 ? currentMinusNextColumn : -(currentMinusNextColumn);
                int tempNextRow = currentMinusNextRow > 0 ? currentMinusNextRow : -(currentMinusNextRow);
                if (tempNextColumn == tempNextRow)
                {
                    diagonally = true;
                }
            }
            if (!leftright)
            {
                if(currentMinusNextColumn == 0)
                {
                    leftright = true;
                }
            }
            if (!updown)
            {
                if(currentMinusNextRow == 0)
                {
                    updown = true;
                }
            }
            if(!diagonally && !leftright && !updown)
            {
                if (!check)
                    Console.WriteLine("Queen can move diagonally, left, right bottom up only");
                isPossible = false;
                return isPossible;
            }

            if (board[nextColumn, nextRow].getColor() == this.getColor())
            {
                if (!check)
                    Console.WriteLine("You can't move your queen on top of your other piece");
                return false;
            }
            if (color == 'B')
            {
                if (diagonally)
                {
                    if (currentColumn > nextColumn && currentRow < nextRow)
                    {
                        for (int i = currentColumn - 1, g = currentRow + 1; isPossible && i > nextColumn && g < currentRow; i--, g++)
                        {
                            if (!(board[i, g] is Blank))
                                isPossible = false;
                        }
                    }
                    if (currentColumn > nextColumn && currentRow > nextRow)
                    {
                        for (int i = currentColumn - 1, g = currentRow - 1; isPossible && i > nextColumn && g > nextRow; i--, g--)
                        {
                            if (!(board[i, g] is Blank))
                                isPossible = false;
                        }
                    }
                }
                //--
                if (leftright)
                {
                    if (currentRow < nextRow && currentMinusNextColumn == 0)
                    {
                        for (int i = currentRow + 1; isPossible && i < nextRow; i++)
                        {
                            if (!(board[currentColumn, i] is Blank))
                                isPossible = false;
                        }
                    }
                    if (currentRow > nextRow && currentMinusNextColumn == 0)
                    {
                        for (int i = currentRow - 1; isPossible && i > nextRow; i--)
                        {
                            if (!(board[currentColumn, i] is Blank))
                                isPossible = false;
                        }
                    }
                }
                if (updown)
                {
                    if (currentColumn < nextColumn && currentMinusNextRow == 0)
                    {
                        for (int i = currentColumn + 1; isPossible && i < nextColumn; i++)
                        {
                            if (!(board[i, currentRow] is Blank))
                                isPossible = false;
                        }
                    }
                    if (currentColumn > nextColumn && currentMinusNextRow == 0)
                    {
                        for (int i = currentColumn - 1; isPossible && i > nextColumn; i--)
                        {
                            if (!(board[i, currentRow] is Blank))
                                isPossible = false;
                        }
                    }
                }
                
                //-----
            }
            if(color == 'W')
            {
                if (diagonally)
                {
                    if (currentColumn < nextColumn && currentRow < nextRow)
                    {
                        for (int i = currentColumn + 1, g = currentRow + 1; i < nextColumn && g < currentRow; i++, g++)
                        {
                            if (!(board[i, g] is Blank))
                                isPossible = false;
                        }
                    }
                    if (currentColumn < nextColumn && currentRow > nextRow)
                    {
                        for (int i = currentColumn + 1, g = currentRow - 1; !diagonally && i < nextColumn && g > nextRow; i++, g--)
                        {
                            if (!(board[i, g] is Blank))
                                isPossible = false;
                        }
                    }
                }
                if (leftright)
                {
                    if (currentRow < nextRow && currentMinusNextColumn == 0)
                    {
                        for (int i = currentRow + 1; isPossible && i < nextRow; i++)
                        {
                            if (!(board[currentColumn, i] is Blank))
                                isPossible = false;
                        }
                    }
                    if (currentRow > nextRow && currentMinusNextColumn == 0)
                    {
                        for (int i = currentRow - 1; isPossible && i > nextRow; i--)
                        {
                            if (!(board[currentColumn, i] is Blank))
                                isPossible = false;
                        }
                    }
                }
                if (updown)
                {
                    if (currentColumn < nextColumn && currentMinusNextRow == 0)
                    {
                        for (int i = currentColumn + 1; isPossible && i < nextColumn; i++)
                        {
                            if (!(board[i, currentRow] is Blank))
                                isPossible = false;
                        }
                    }
                    if (currentColumn > nextColumn && currentMinusNextRow == 0)
                    {
                        for (int i = currentColumn - 1; isPossible && i > nextColumn; i--)
                        {
                            if (!(board[i, currentRow] is Blank))         
                                isPossible = false;

                        }
                    }
                }
            }
            if (!isPossible && !check)
                Console.WriteLine("Queen cannot go over other piece");
            if (isPossible && !check)
                totalMoves++;
            return isPossible;
        }

        public void setName(char color)
        {
            this.name = color == 'W' ? "WQ" : "BQ";
        }
        public override string ToString()
        {
            return name;
        }
    }
}
