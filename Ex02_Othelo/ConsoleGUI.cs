using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02_Othelo
{
    public class ConsoleGUI
    {
        private const char k_SecondPlayerSignal = 'O';
        private const char k_FirstPlayerSignal = 'X';
        private const string k_PlayAgainstComputer = "COMPUTER";
        private static char s_CurrentSignalTurn;
        private static OtheloGameRules s_OtheloGamePolicy;

        public void StartOtheloGame()
        {
            OtheloGamePlayer firstPlayer, secondPlayer;
            OtheloGameBoard otheloGameBoard;
            
            welcomeAndInitializeFirstPlayer(out firstPlayer);

            chooseAndInitializePlayerToPlayWith(out secondPlayer);

            chooseTheSizeOfTheOtheloBoardGame(out otheloGameBoard);

            // Create an OtheloGamePolicy object
            s_OtheloGamePolicy = new OtheloGameRules(otheloGameBoard.OtheloGameBoardMatrixDimension, secondPlayer);

            s_CurrentSignalTurn = k_FirstPlayerSignal; // Always the 'X' player starts first
            
            startPlayOtheloGame(firstPlayer, secondPlayer, otheloGameBoard);
        }

        private void welcomeAndInitializeFirstPlayer(out OtheloGamePlayer o_FirstPlayer)
        {
            string firstPlayerName;

            Console.WriteLine(@"                                                   
                                            Hello:) Welcome to Othelo game

                                               Please enter your name :");

            firstPlayerName = getNameAndCheckIfNameIsNoLonger();

            o_FirstPlayer = new OtheloGamePlayer(firstPlayerName, k_FirstPlayerSignal, OtheloGameBoard.eGameTypes.Player);
        }

        private string getNameAndCheckIfNameIsNoLonger()
        {
            const int k_MaxPlayerNameLength = 10;

            string playerName = Console.ReadLine();

            checkIfUserInputWantToExitFromGame(playerName);

            while (playerName.Length > k_MaxPlayerNameLength || playerName.Length == 0)
            {
                Console.WriteLine(@"
                                   Illegal player name! Must insert name and limit to 15 letters!!! 
                                             Please enter your name again :");

                playerName = Console.ReadLine();

                checkIfUserInputWantToExitFromGame(playerName);
            }

            return playerName;
        }

        private void chooseAndInitializePlayerToPlayWith(out OtheloGamePlayer o_SecondPlayer)
        {
            int playerChoise;

            playerChoise = getUserChoiseToPlayWith();

            o_SecondPlayer = new OtheloGamePlayer();

            switch (playerChoise)
            {
                case 1: initializeOtheloGameAgainstAnotherPlayer(out o_SecondPlayer);
                    break;
                case 2: initializeOtheloGameAgainstTheComputer(out o_SecondPlayer);
                    break;
            }
        }

        private int getUserChoiseToPlayWith()
        {
            string strPlayerChoise;
            int intPlayerChoise;
            const bool v_SuccessParseStringToInt = true;
            const bool v_UserInsert1Or2Key = true;
            const int k_FirstCorrectChoiseValue = 1;
            const int k_SecondCorrectChoiseValue = 2;

            Console.WriteLine(@"
                                       please choose player to play with :
                                       To play against another player enter '1'
                                       To play against the computer enter '2'");

            strPlayerChoise = Console.ReadLine();

            checkIfUserInputWantToExitFromGame(strPlayerChoise);

            while(int.TryParse(strPlayerChoise, out intPlayerChoise) != v_SuccessParseStringToInt || 
                   checkUserInsertCorrectValue(intPlayerChoise, k_FirstCorrectChoiseValue, k_SecondCorrectChoiseValue) != v_UserInsert1Or2Key)
            {
                Console.WriteLine(@"
                                      Wrong input!!! Please choose '1' or '2'");

                strPlayerChoise = Console.ReadLine();

                checkIfUserInputWantToExitFromGame(strPlayerChoise);
            }

            return intPlayerChoise;
        }

        private bool checkUserInsertCorrectValue(int i_IntPlayerChoise, int i_FirstPossibleCorrectValue, int i_SecondPossibleCorrectValue)
        {
            bool userInsertCorrectChoise = true;
            
            if(i_IntPlayerChoise != i_FirstPossibleCorrectValue && i_IntPlayerChoise != i_SecondPossibleCorrectValue)
            {
                userInsertCorrectChoise = !userInsertCorrectChoise;
            }

            return userInsertCorrectChoise;
        }

        private void initializeOtheloGameAgainstAnotherPlayer(out OtheloGamePlayer o_SecondPlayer)
        {
            string secondPlayerName;

            Console.WriteLine(@"                                                   
                                        O.K! you chose to play with another player 

                                     So, please enter the name of the second player :");

            secondPlayerName = getNameAndCheckIfNameIsNoLonger();

            o_SecondPlayer = new OtheloGamePlayer(secondPlayerName, k_SecondPlayerSignal, OtheloGameBoard.eGameTypes.Player);
        }

        private void chooseTheSizeOfTheOtheloBoardGame(out OtheloGameBoard o_OtheloGameBoard)
        {
            string strOtheloBoardDimension;
            int intOtheloBoardDimension;
            const int k_FirstCorrectBoardDimensionValue = 6; 
            const int k_SecondCorrectBoardDimensionValue = 8;
            const bool v_SuccessParseStringToInt = true;
            const bool v_UserInsertCorrectValue = true;
            
            Console.WriteLine(@"
                                please choose the board dimension - you can choose 6x6 or 8x8 dimensions :
                                To 6x6 dimension insert '6'
                                To 8x8 dimension insert '8'");
            strOtheloBoardDimension = Console.ReadLine();

            checkIfUserInputWantToExitFromGame(strOtheloBoardDimension);

            while(int.TryParse(strOtheloBoardDimension, out intOtheloBoardDimension) != v_SuccessParseStringToInt || 
                  checkUserInsertCorrectValue(intOtheloBoardDimension, k_FirstCorrectBoardDimensionValue, k_SecondCorrectBoardDimensionValue) != v_UserInsertCorrectValue)
            {
                Console.WriteLine(@"
                                   Illegal bord dimension! Only 6x6 or 8x8 is possible !!! 
                                   To 6x6 dimension insert '6'
                                   To 8x8 dimension insert '8'
                                   Please enter your choise again :");

                strOtheloBoardDimension = Console.ReadLine();

                checkIfUserInputWantToExitFromGame(strOtheloBoardDimension);
            }

            o_OtheloGameBoard = new OtheloGameBoard(intOtheloBoardDimension);
        }

        private void initializeOtheloGameAgainstTheComputer(out OtheloGamePlayer o_SecondPlayer)
        {
            o_SecondPlayer = new OtheloGamePlayer(k_PlayAgainstComputer, k_SecondPlayerSignal, OtheloGameBoard.eGameTypes.Computer);
        }

        // $G$ DSN-999 (-5) this method is to long - should be divided to several methods
        private void startPlayOtheloGame(OtheloGamePlayer i_FirstPlayer, OtheloGamePlayer i_SecondPlayer, OtheloGameBoard i_OtheloGameBoard)
        {
            const bool v_HasFreePlaceOnBoardGame = true;
            const bool v_WantToPlayAnotherGameWithTheSamePlayer = true;
            const bool v_CurrentPlayerCanContinueThisTurn = true;
            const bool v_UserChoiseIsLegal = true;
            const bool v_TwoPlayersCanNotMoveInTheGame = true;
            const bool v_UserWantToStartNewGame = true;
            const int k_TimeToDelay = 3000;
            bool wantStartNewGame = true;
            bool currentPlayerCanPlayThisTurn = true;
            int[] positionFromUserInTheBoard;
            
            while (wantStartNewGame == v_UserWantToStartNewGame)
            {
                while (i_OtheloGameBoard.CheckTheOtheloBoardGameHasFreePlace() == v_HasFreePlaceOnBoardGame)
                {
                    // There are still empty cells on board - continue the game
                    i_OtheloGameBoard.PrintOtheloGameBoard();

                    i_OtheloGameBoard.CountAndUpdateHowManySignalsOnBoardFromAnyKind();

                    checkIfPrintMsgAboutTheCurrentPlayerHasNothingToDoThisTurn(ref currentPlayerCanPlayThisTurn);

                    printTheCurrentUserTurn(i_FirstPlayer.PlayerName, i_SecondPlayer.PlayerName);

                    if (s_OtheloGamePolicy.CheckIfCurrentPlayerHasOptionToContinueThisTurn(s_CurrentSignalTurn, i_OtheloGameBoard, i_SecondPlayer) == v_CurrentPlayerCanContinueThisTurn)
                    {
                        // If it's regular player turn ( not the computer )
                        if ((s_CurrentSignalTurn == k_FirstPlayerSignal) || (s_CurrentSignalTurn == k_SecondPlayerSignal && i_SecondPlayer.StateGame != OtheloGameBoard.eGameTypes.Computer))
                        {
                            // There is atleast one option to the current player to continue in this turn 
                            currentPlayerCanContinueToPlayThisTurn(i_OtheloGameBoard, out positionFromUserInTheBoard);

                            /* In case it is possible the current player to continue and he want to put his signal in empty position on the board,
                             * so now you have to check if it's legal */

                            while (s_OtheloGamePolicy.CheckTheUserChoiseIsLegal(s_CurrentSignalTurn, i_OtheloGameBoard, positionFromUserInTheBoard) != v_UserChoiseIsLegal)
                            {
                                // In case the user chose illegal position - try insert new position 
                                Console.WriteLine("Illegal position!!! You can put your signal only if you block the other player!");
                                currentPlayerCanContinueToPlayThisTurn(i_OtheloGameBoard, out positionFromUserInTheBoard);
                            }

                            // In case the position from user is legal - so do it!
                            s_OtheloGamePolicy.PutSignalInTheRequiredUserPositionOnBoard(i_OtheloGameBoard, s_CurrentSignalTurn, positionFromUserInTheBoard);
                        }
                        else if (s_CurrentSignalTurn == k_SecondPlayerSignal && i_SecondPlayer.StateGame == OtheloGameBoard.eGameTypes.Computer)
                        {  // In case it's second player turn and it's the COMPUTER 
                            s_OtheloGamePolicy.PutAutomaticSignalOfCompuerOnBoard(i_OtheloGameBoard, s_CurrentSignalTurn);
                            System.Threading.Thread.Sleep(k_TimeToDelay);
                        }
                    }
                    else
                    { // In case the current player has nothing to do
                        currentPlayerCanPlayThisTurn = !currentPlayerCanPlayThisTurn;
                    }

                    /* In case there isn't possible way to current player to continue this turn - so move to the other player 
                     * We arrive here also when the current player finish his turn and we continue to the other player*/
                    
                    if (checkTheTwoPlayersCanNotMoveInTheGame(i_OtheloGameBoard, i_SecondPlayer) == v_TwoPlayersCanNotMoveInTheGame)
                    {
                        printMassageInCaseTwoPlayersCanNotMoveInTheGame(i_OtheloGameBoard);
                        break;
                    }

                    changeTurnToTheOtherPlayer();
                }

                // No free place on board game or no one from the players can play - so finish the current game and display the points status
                noFreePlaceOnBoardOrNoOneCanPlaySoFinishTheGame(i_FirstPlayer, i_SecondPlayer, i_OtheloGameBoard);

                if (askAboutAnotherGameWithTheSamePlayer() != v_WantToPlayAnotherGameWithTheSamePlayer)
                {
                    printMsgAboutLeaveTheGame();

                    wantStartNewGame = !wantStartNewGame; // Dont start an new game
                }
                else
                { // Want to start an new game
                    startNewGame(i_OtheloGameBoard);
                }
            }
        }

        private void printMassageInCaseTwoPlayersCanNotMoveInTheGame(OtheloGameBoard i_OtheloGameBoard)
        {
            i_OtheloGameBoard.PrintOtheloGameBoard();
            Console.WriteLine();
            Console.WriteLine("**********No one from the players can continue to play!**********");
            Console.WriteLine();
        }
        
        private void checkIfPrintMsgAboutTheCurrentPlayerHasNothingToDoThisTurn(ref bool io_CurrentPlayerCanPlayThisTurn)
        {
            const bool v_currentPlayerCanPlayThisTurn = true;

            if(io_CurrentPlayerCanPlayThisTurn != v_currentPlayerCanPlayThisTurn)
            {
               Console.WriteLine();
               Console.WriteLine("**********Sorry, you have nothing to do this turn so the turn go to the other player**********");
               Console.WriteLine();

               // Change variable to "true" value before finish display this message
               io_CurrentPlayerCanPlayThisTurn = !io_CurrentPlayerCanPlayThisTurn; 
            }
        }

        private bool checkTheTwoPlayersCanNotMoveInTheGame(OtheloGameBoard i_OtheloGameBoard, OtheloGamePlayer i_SecondPlayer)
        {
            bool twoPlayersCanNotMoveInTheGame = true;
            const bool v_FirstPlayerCanContinueTheGame = true;
            const bool v_SecondPlayerCanContinueTheGame = true;

            if (s_OtheloGamePolicy.CheckIfCurrentPlayerHasOptionToContinueThisTurn(k_SecondPlayerSignal, i_OtheloGameBoard, i_SecondPlayer) == v_FirstPlayerCanContinueTheGame ||
                s_OtheloGamePolicy.CheckIfCurrentPlayerHasOptionToContinueThisTurn(k_FirstPlayerSignal, i_OtheloGameBoard, i_SecondPlayer) == v_SecondPlayerCanContinueTheGame)
            {
                twoPlayersCanNotMoveInTheGame = !twoPlayersCanNotMoveInTheGame;
            }

            return twoPlayersCanNotMoveInTheGame;
        }

        private void currentPlayerCanContinueToPlayThisTurn(OtheloGameBoard i_OtheloGameBoard, out int[] o_PositionFromUserInTheBoard)
        {
            bool requiredCellIsEmpty = true;

            askFromCurrentPlayerToInsertThePositionToHisNextStep(out o_PositionFromUserInTheBoard, i_OtheloGameBoard);

            while (i_OtheloGameBoard.CheckTheRequiredPositionOnBoardIsEmpty(o_PositionFromUserInTheBoard[0], o_PositionFromUserInTheBoard[1]) != requiredCellIsEmpty)
            {
                // In case it's possible the current player to continue but he want to put his signal in not empty position
                Console.WriteLine(@"
                            This position isn't empty !!! try again -");

                askFromCurrentPlayerToInsertThePositionToHisNextStep(out o_PositionFromUserInTheBoard, i_OtheloGameBoard);   
            }
        }

        private void noFreePlaceOnBoardOrNoOneCanPlaySoFinishTheGame(OtheloGamePlayer i_FirstPlayer, OtheloGamePlayer i_SecondPlayer, OtheloGameBoard i_OtheloGameBoard)
        {
            i_OtheloGameBoard.CountAndUpdateHowManySignalsOnBoardFromAnyKind();

            printWhoIsTheWinnerOfTheCurrentGame(i_FirstPlayer, i_SecondPlayer, i_OtheloGameBoard);
        }

        private void startNewGame(OtheloGameBoard i_OtheloGameBoard)
        {
            i_OtheloGameBoard.InitializeOtheloGameBoard();

            s_CurrentSignalTurn = k_FirstPlayerSignal; // Always the 'X' player starts first
        }

        private void changeTurnToTheOtherPlayer()
        {
            if(s_CurrentSignalTurn == k_FirstPlayerSignal)
            {
                s_CurrentSignalTurn = k_SecondPlayerSignal;
            }
            else
            {
                s_CurrentSignalTurn = k_FirstPlayerSignal;
            }
        }

        private void askFromCurrentPlayerToInsertThePositionToHisNextStep(out int[] o_PositionFromUserInTheBoard, OtheloGameBoard i_OtheloGameBoard)
        {
            string strUserRowNumberPosition;
            string strUserColumnLetterPosition;
            int intUserRowNumberPosition;
            int convertedIntUserRowNumberPosition;
            int convertedIntUserColumnLetterPosition;

            Console.WriteLine(@"Please choose cell in the board you want to put {0}. First enter the row number and press enter :", s_CurrentSignalTurn);
            
            strUserRowNumberPosition = Console.ReadLine();
            intUserRowNumberPosition = checkTheInputRowNumberPosition(strUserRowNumberPosition, i_OtheloGameBoard);

            Console.WriteLine("Now enter column letter position you want :");

            strUserColumnLetterPosition = Console.ReadLine();
            checkTheInputColumnLetterPosition(ref strUserColumnLetterPosition, i_OtheloGameBoard);

            convertedIntUserRowNumberPosition = intUserRowNumberPosition - 1; // Match between input number row index and matrix row index 

            convertedIntUserColumnLetterPosition = convertTheInputLetterToActualIndexesInTheBoard(strUserColumnLetterPosition);

            // Insert the position from user to the array
            o_PositionFromUserInTheBoard = new int[2];
            o_PositionFromUserInTheBoard[0] = convertedIntUserRowNumberPosition;
            o_PositionFromUserInTheBoard[1] = convertedIntUserColumnLetterPosition;
        }

        private int convertTheInputLetterToActualIndexesInTheBoard(string i_StrUserColumnLetterPosition)
        {
            const int k_AsciiValueOfA = 65;

            return i_StrUserColumnLetterPosition[0] - k_AsciiValueOfA; // Match between input letter column index and matrix column index 
        }

        private int checkTheInputRowNumberPosition(string i_StrUserRowNumberPosition, OtheloGameBoard i_OtheloGameBoard)
        {
            int intUserRowNumberPosition;
            const bool v_UserInsertLegalRowNumber = true;
            const bool v_SuccessParseStringToInt = true;

            checkIfUserInputWantToExitFromGame(i_StrUserRowNumberPosition);

            while(int.TryParse(i_StrUserRowNumberPosition, out intUserRowNumberPosition) != v_SuccessParseStringToInt ||
                  i_OtheloGameBoard.CheckTheInputRowNumberIsNotOutOfBoundsOfTheGameBoard(intUserRowNumberPosition) != v_UserInsertLegalRowNumber)
            {
                Console.WriteLine(@"

                Wrong input row number!!! Out of game board bounds
                Enter again row number value");

                i_StrUserRowNumberPosition = Console.ReadLine();

                checkIfUserInputWantToExitFromGame(i_StrUserRowNumberPosition);
            }
            
            return intUserRowNumberPosition;
        }

        private void checkIfUserInputWantToExitFromGame(string i_StrUserRowNumberPosition)
        {
            if(i_StrUserRowNumberPosition.Equals("Q"))
            {
                Environment.Exit(1);
            }
        }

        private void checkTheInputColumnLetterPosition(ref string io_StrUserColumnLetterPosition, OtheloGameBoard i_OtheloGameBoard)
        {
            const bool v_UserInsertLegalColumnLetterPosition = true;

            checkIfUserInputWantToExitFromGame(io_StrUserColumnLetterPosition);

            while (i_OtheloGameBoard.CheckTheInputColumnLetterIsNotOutOfBoundsOfTheGameBoard(io_StrUserColumnLetterPosition) != v_UserInsertLegalColumnLetterPosition)
            {
                Console.WriteLine(@"
                Wrong input column letter!!! Out of game board bounds
                Enter again column letter value");

                io_StrUserColumnLetterPosition = Console.ReadLine();

                checkIfUserInputWantToExitFromGame(io_StrUserColumnLetterPosition);
            }
        }

        private void printTheCurrentUserTurn(string i_FirstPlayerName, string i_SecondPlayerName)
        {
            string currentNamePlayerTurn = i_FirstPlayerName;

            if(s_CurrentSignalTurn == 'O')
            {
                currentNamePlayerTurn = i_SecondPlayerName;
            }

            string msgAboutCurrentTurn = string.Format(@"{0} is the 'X'  <------>  {1} is the 'O' :   Now it's {2}'s turn", i_FirstPlayerName, i_SecondPlayerName, currentNamePlayerTurn);
            Console.WriteLine();
            Console.WriteLine(msgAboutCurrentTurn);
        }

        private void printWhoIsTheWinnerOfTheCurrentGame(OtheloGamePlayer i_FirstPlayer, OtheloGamePlayer i_SecondPlayer, OtheloGameBoard i_OtheloGameBoard)
        {
            char theWinnerOfTheCurrentGame;
            string msgAboutTheWinner;

            theWinnerOfTheCurrentGame = s_OtheloGamePolicy.CheckWhoIsTheWinnerOfTheCurrentGame(i_OtheloGameBoard, i_FirstPlayer, i_SecondPlayer);

            if(theWinnerOfTheCurrentGame == 'X')
            {
                msgAboutTheWinner = string.Format("{0} is the winner  :))))))", i_FirstPlayer.PlayerName);

                printMsgAboutTheWinner(i_FirstPlayer, i_SecondPlayer, i_OtheloGameBoard, msgAboutTheWinner);
            }
            else if (theWinnerOfTheCurrentGame == 'O')
            {
                msgAboutTheWinner = string.Format("{0} is the winner :))))))", i_SecondPlayer.PlayerName);

                printMsgAboutTheWinner(i_SecondPlayer, i_FirstPlayer, i_OtheloGameBoard, msgAboutTheWinner);
            }
            else 
            {
                // FirstPlayer signals == SecondPlayer signals on board 
                msgAboutTheWinner = string.Format("There is no winner this game", i_FirstPlayer.PlayerName);

                printMsgAboutTheWinner(i_FirstPlayer, i_SecondPlayer, i_OtheloGameBoard, msgAboutTheWinner);
            }   
        }

        private void printMsgAboutTheWinner(OtheloGamePlayer i_FirstPlayer, OtheloGamePlayer i_SecondPlayer, OtheloGameBoard i_OtheloGameBoard, string i_MsgAboutTheWinner)
        {
            string msgAboutCurrentTurn = string.Format(@" GAME OVER ! ! ! {0}  Your points in this game are : {1} : {3} points, {2} : {4} points", i_MsgAboutTheWinner, i_FirstPlayer.PlayerName, i_SecondPlayer.PlayerName, i_FirstPlayer.PlayerPoints, i_SecondPlayer.PlayerPoints);

            Console.WriteLine(msgAboutCurrentTurn);
        }

        private bool askAboutAnotherGameWithTheSamePlayer()
        {
            const bool v_SuccessParseStringToInt = true;
            const bool v_UserInsert1Or2Key = true;
            const int k_FirstCorrectChoiseValue = 1;
            const int k_SecondCorrectChoiseValue = 2;
            bool wantToPlayAnotherGameWithTheSamePlayer = true;
            int intUserChoiseAboutAnotherGame;
            string strUserChoiseAboutAnotherGame;

            string msgAboutAnotherGame = string.Format(@"
            Do you want to play another game ?

            Yes - press 1, No - press 2");
                                                   
             Console.WriteLine(msgAboutAnotherGame);
             strUserChoiseAboutAnotherGame = Console.ReadLine();

             checkIfUserInputWantToExitFromGame(strUserChoiseAboutAnotherGame);

             while((int.TryParse(strUserChoiseAboutAnotherGame, out intUserChoiseAboutAnotherGame) != v_SuccessParseStringToInt) ||
                  (checkUserInsertCorrectValue(intUserChoiseAboutAnotherGame, k_FirstCorrectChoiseValue, k_SecondCorrectChoiseValue) != v_UserInsert1Or2Key))
             {
                Console.WriteLine(@"
                Wrong input!!! Please choose '1' or '2'");

                strUserChoiseAboutAnotherGame = Console.ReadLine();

                checkIfUserInputWantToExitFromGame(strUserChoiseAboutAnotherGame);
            }

             if(intUserChoiseAboutAnotherGame == 2)
              {
                wantToPlayAnotherGameWithTheSamePlayer = !wantToPlayAnotherGameWithTheSamePlayer;
              }

            return wantToPlayAnotherGameWithTheSamePlayer;
        }

        private void printMsgAboutLeaveTheGame()
        {
            Console.WriteLine("Bye Bye!! Please press any key to exit");

            Console.WriteLine("Press enter for exit");
            Console.ReadLine();
        }
    }
}
