using System;
using System.Collections.Generic;

namespace Ex02_Othelo
{
    public class OtheloGameRules
    {
        private const char k_PlayerSignalKind1 = 'X';
        private const char k_PlayerSignalKind2 = 'O';
        private const char k_NoWinnerInTheGame = '-';
        private char[,] m_AllCurrentLegalMovesMatrix;
        private List<int> m_ListWithTheCurrentPositinsOptionsOnBoardForRandom;

        public OtheloGameRules(int i_SizeOfGameBoard, OtheloGamePlayer i_SecondPlayer)
        {
            m_AllCurrentLegalMovesMatrix = new char[i_SizeOfGameBoard, i_SizeOfGameBoard];

            if (i_SecondPlayer.StateGame == OtheloGameBoard.eGameTypes.Computer)
            {
                m_ListWithTheCurrentPositinsOptionsOnBoardForRandom = new List<int>();
            }
        }

        public char CheckWhoIsTheWinnerOfTheCurrentGame(OtheloGameBoard i_OtheloGameBoard, OtheloGamePlayer i_FirstPlayer, OtheloGamePlayer i_SecondPlayer)
        {
            char theWinnerOfTheCurrentGame;

            if (i_OtheloGameBoard.NumOfFirstPlayerSignalsOnBoard > i_OtheloGameBoard.NumOfSecondPlayerSignalsOnBoard)
            {
                theWinnerOfTheCurrentGame = k_PlayerSignalKind1;
            }
            else if (i_OtheloGameBoard.NumOfFirstPlayerSignalsOnBoard < i_OtheloGameBoard.NumOfSecondPlayerSignalsOnBoard)
            {
                theWinnerOfTheCurrentGame = k_PlayerSignalKind2;
            }
            else
            {  // FirstPlayer signals == SecondPlayer signals on board 
                theWinnerOfTheCurrentGame = k_NoWinnerInTheGame;
            }

            i_FirstPlayer.PlayerPoints = i_OtheloGameBoard.NumOfFirstPlayerSignalsOnBoard;
            i_SecondPlayer.PlayerPoints = i_OtheloGameBoard.NumOfSecondPlayerSignalsOnBoard;

            return theWinnerOfTheCurrentGame;
        }

        public bool CheckIfCurrentPlayerHasOptionToContinueThisTurn(char i_CurrentSignalTurn, OtheloGameBoard i_OtheloGameBoard, OtheloGamePlayer i_SecondPlayer)
        {
            const bool v_LegalMovesMatrixEmptyFromCurrentSignalTurn = true;
            bool currentPlayerHasOptionToContinueThisTurn = true;

            zeroTheLegalMovesMatrix(i_OtheloGameBoard);

            if (i_SecondPlayer.StateGame == OtheloGameBoard.eGameTypes.Computer && m_ListWithTheCurrentPositinsOptionsOnBoardForRandom.Count != 0)
            {
                m_ListWithTheCurrentPositinsOptionsOnBoardForRandom.Clear();
            }

            createCurrentLegalMovesMatrix(i_OtheloGameBoard, i_CurrentSignalTurn, i_SecondPlayer);

            if (checkIfLegalMovesMatrixEmptyFromCurrentSignalTurn(i_CurrentSignalTurn, i_OtheloGameBoard) == v_LegalMovesMatrixEmptyFromCurrentSignalTurn)
            {
                currentPlayerHasOptionToContinueThisTurn = !currentPlayerHasOptionToContinueThisTurn;
            }

            return currentPlayerHasOptionToContinueThisTurn;
        }

        private void zeroTheLegalMovesMatrix(OtheloGameBoard i_OtheloGameBoard)
        {
            for (int i = 0; i < i_OtheloGameBoard.OtheloGameBoardMatrixDimension; i++)
            {
                for (int j = 0; j < i_OtheloGameBoard.OtheloGameBoardMatrixDimension; j++)
                {
                    this.m_AllCurrentLegalMovesMatrix[i, j] = '\0';
                }
            }
        }

        private bool checkIfLegalMovesMatrixEmptyFromCurrentSignalTurn(char i_CurrentSignalTurn, OtheloGameBoard i_OtheloGameBoard)
        {
            bool legalMovesMatrixEmptyFromCurrentSignalTurn = true;

            for (int i = 0; i < i_OtheloGameBoard.OtheloGameBoardMatrixDimension; i++)
            {
                for (int j = 0; j < i_OtheloGameBoard.OtheloGameBoardMatrixDimension; j++)
                {
                    /* In case the 'LegalMovesMatrix' contains atleast one signal type - 
                     * so it's mean that the current player can play this turn */

                    if (this.m_AllCurrentLegalMovesMatrix[i, j] == i_CurrentSignalTurn)
                    {
                        legalMovesMatrixEmptyFromCurrentSignalTurn = !legalMovesMatrixEmptyFromCurrentSignalTurn;
                        i = i_OtheloGameBoard.OtheloGameBoardMatrixDimension;
                        break;
                    }
                }
            }

            return legalMovesMatrixEmptyFromCurrentSignalTurn;
        }

        private void createCurrentLegalMovesMatrix(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, OtheloGamePlayer i_SecondPlayer)
        {
            fillCurrentLegalMovesMatrix(i_OtheloGameBoard, i_CurrentSignalTurn, i_SecondPlayer);
        }

        private void fillCurrentLegalMovesMatrix(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, OtheloGamePlayer i_SecondPlayer)
        {
            for (int i = 0; i < i_OtheloGameBoard.OtheloGameBoardMatrixDimension; i++)
            {
                for (int j = 0; j < i_OtheloGameBoard.OtheloGameBoardMatrixDimension; j++)
                {
                    fillSpecificCellInLegalMovesMatrix(i_OtheloGameBoard, i_CurrentSignalTurn, i, j, i_SecondPlayer);
                }
            }
        }

        private void fillSpecificCellInLegalMovesMatrix(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int i_RowIndex, int i_ColumnIndex, OtheloGamePlayer i_SecondPlayer)
        {
            const bool v_LegalMove = true;

            if (checkIfLegalMoveAndUpdateLegalMovesMatrix(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowIndex, i_ColumnIndex) == v_LegalMove)
            {
                // Fill the specific cell in the legal moves matrix with the current signal turn
                this.m_AllCurrentLegalMovesMatrix[i_RowIndex, i_ColumnIndex] = i_CurrentSignalTurn;

                // Fill the List with the relevent matrix indexes cell - For the random action later
                if (i_SecondPlayer.StateGame == OtheloGameBoard.eGameTypes.Computer)
                {
                    m_ListWithTheCurrentPositinsOptionsOnBoardForRandom.Add(i_RowIndex); // Put the row index of cell in the even place of the list
                    m_ListWithTheCurrentPositinsOptionsOnBoardForRandom.Add(i_ColumnIndex); // Put the column index of cell in the odd place of the list
                }
            }
        }

        private bool checkIfLegalMoveAndUpdateLegalMovesMatrix(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int i_RowIndex, int i_ColumnIndex)
        {
            const char v_EmptyCellInGameBoard = '\0';
            bool itIsLegalMove = true;

            if (i_OtheloGameBoard.OtheloGameBoardMatrix[i_RowIndex, i_ColumnIndex] != v_EmptyCellInGameBoard)
            {
                itIsLegalMove = !itIsLegalMove;
            }
            else
            {
                itIsLegalMove = checkEightPossibleSidesAroundCellOnBoard(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowIndex, i_ColumnIndex);
            }

            return itIsLegalMove;
        }

        private bool checkEightPossibleSidesAroundCellOnBoard(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int i_RowIndex, int i_ColumnIndex)
        {
            const bool v_GoodSideToBlockTheOpponent = true;
            bool atLeastOneOfTheSidesIsPossibleToBlock = true;
            int atLeastOneOfTheSidesIsOk = 0;

            // By 'rowIndexToMoveAround' and 'columnIndexToMoveAround' indexes you can check all the eight sides around you
            for (int rowIndexToMoveAround = -1; rowIndexToMoveAround <= 1; rowIndexToMoveAround++)
            {
                for (int columnIndexToMoveAround = -1; columnIndexToMoveAround <= 1; columnIndexToMoveAround++)
                {
                    // Irellevent to check my position - only check the cells around me
                    if (rowIndexToMoveAround == 0 && columnIndexToMoveAround == 0)
                    {
                        continue;
                    }

                    if (checkNextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowIndex, i_ColumnIndex, rowIndexToMoveAround, columnIndexToMoveAround) == v_GoodSideToBlockTheOpponent)
                    {
                        atLeastOneOfTheSidesIsOk = 1;
                        rowIndexToMoveAround = 2;
                        break;
                    }
                }
            }

            // No one of the eight sides is possible
            if (atLeastOneOfTheSidesIsOk == 0)
            {
                atLeastOneOfTheSidesIsPossibleToBlock = !atLeastOneOfTheSidesIsPossibleToBlock;
            }

            return atLeastOneOfTheSidesIsPossibleToBlock;
        }

        private bool checkNextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int i_RowIndex, int i_ColumnIndex, int i_RowIndexToMoveAround, int i_ColumnIndexToMoveAround)
        {
            const bool v_StayInBoardBordersIfYouMoveOneSide = true;
            const bool v_StayInBoardBordersIfYouMoveTwoSide = true;
            const bool v_ThereIsMyTypeBlockSignalInTheEndOfThisSideRow = true;
            bool nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType = true;
            int doubleRowIndex = i_RowIndexToMoveAround * 2;
            int doubleColumnIndex = i_ColumnIndexToMoveAround * 2;

            if (checkYouStayInBoardBordersIfYouMoveOneSideNextToPosition(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowIndex, i_ColumnIndex, i_RowIndexToMoveAround, i_ColumnIndexToMoveAround) == v_StayInBoardBordersIfYouMoveOneSide)
            {
                // If the close signal is the opposite of my type - so it is a possible side
                if (i_OtheloGameBoard.OtheloGameBoardMatrix[i_RowIndex + i_RowIndexToMoveAround, i_ColumnIndex + i_ColumnIndexToMoveAround] == i_OtheloGameBoard.GetTheOppositeSignal(i_CurrentSignalTurn))
                {
                    // If there is more than one signal after my signal in this side
                    if (checkThereIsAtLeastOneMoreSignalOnBoardTwoSidesFromMYPosition(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowIndex, i_ColumnIndex, i_RowIndexToMoveAround, i_ColumnIndexToMoveAround) == v_StayInBoardBordersIfYouMoveTwoSide)
                    {
                        if (recursiveMethodToCheckThereIsMyTypeBlockSignalInTheEndOfThisSideRow(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowIndex + doubleRowIndex, i_ColumnIndex + doubleColumnIndex, i_RowIndexToMoveAround, i_ColumnIndexToMoveAround) != v_ThereIsMyTypeBlockSignalInTheEndOfThisSideRow)
                        {
                            nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType = !nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType;
                        }
                    }
                    else
                    {
                        nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType = !nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType;
                    }
                }
                else
                {
                    nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType = !nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType;
                }
            }
            else
            {
                nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType = !nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType;
            }

            return nextSignalInSpecificSideOppositeAndTheRowEndWithMySignalType;
        }

        // $G$ CSS-028 (-3) method shouldn't include more then one return command.
        private bool recursiveMethodToCheckThereIsMyTypeBlockSignalInTheEndOfThisSideRow(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int i_RowIndex, int i_ColumnIndex, int i_RowIndexToMoveAround, int i_ColumnIndexToMoveAround)
        {
            const bool v_ThereIsMyTypeBlockSignalInTheEndOfThisSideRow = true;
            const char k_EmptyCellInGameBoard = '\0';

            /* Stop conditions of the recursion : 
             * If we out of the board bourders */
            if (i_RowIndex == i_OtheloGameBoard.OtheloGameBoardMatrixDimension || i_ColumnIndex == i_OtheloGameBoard.OtheloGameBoardMatrixDimension)
            {
                  return !v_ThereIsMyTypeBlockSignalInTheEndOfThisSideRow;
            }
            else if (i_OtheloGameBoard.OtheloGameBoardMatrix[i_RowIndex, i_ColumnIndex] == i_CurrentSignalTurn)
            {  // If we arrive to signal like my type - so stop and return true , cause bloking can be

                return v_ThereIsMyTypeBlockSignalInTheEndOfThisSideRow;
            }
            else if (i_OtheloGameBoard.OtheloGameBoardMatrix[i_RowIndex, i_ColumnIndex] == k_EmptyCellInGameBoard)
            { // If we arrive to empty place in board - so stop and return false , cause bloking can't be

                return !v_ThereIsMyTypeBlockSignalInTheEndOfThisSideRow;
            }
            else if(i_RowIndex + i_RowIndexToMoveAround < 0 || i_RowIndex + i_RowIndexToMoveAround > i_OtheloGameBoard.OtheloGameBoardMatrixDimension)
            {  // In case the next step is out of rows bounds 

                return !v_ThereIsMyTypeBlockSignalInTheEndOfThisSideRow;
            }
            else if (i_ColumnIndex + i_ColumnIndexToMoveAround < 0 || i_ColumnIndex + i_ColumnIndexToMoveAround > i_OtheloGameBoard.OtheloGameBoardMatrixDimension)
            { // In case the next step is out of columns bounds 

                return !v_ThereIsMyTypeBlockSignalInTheEndOfThisSideRow;
            }
            
            // Recursive calling
            return recursiveMethodToCheckThereIsMyTypeBlockSignalInTheEndOfThisSideRow(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowIndex + i_RowIndexToMoveAround, i_ColumnIndex + i_ColumnIndexToMoveAround, i_RowIndexToMoveAround, i_ColumnIndexToMoveAround);
        }

        private bool checkThereIsAtLeastOneMoreSignalOnBoardTwoSidesFromMYPosition(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int i_RowIndex, int i_ColumnIndex, int i_RowIndexToMoveAround, int i_ColumnIndexToMoveAround)
        {
            bool stayInBoardBordersIfYouMoveOneSide = true;
            int doubleRowIndex = i_RowIndexToMoveAround * 2;
            int doubleColumnIndex = i_ColumnIndexToMoveAround * 2;
            
            if ((i_RowIndex + doubleRowIndex < 0) || (i_RowIndex + doubleRowIndex) > i_OtheloGameBoard.OtheloGameBoardMatrixDimension - 1)
            {    // Out of rows bounds

                stayInBoardBordersIfYouMoveOneSide = !stayInBoardBordersIfYouMoveOneSide;
            }
            else if ((i_ColumnIndex + doubleColumnIndex) < 0 || (i_ColumnIndex + doubleColumnIndex) > i_OtheloGameBoard.OtheloGameBoardMatrixDimension - 1)
            {  // Out of columns bounds

                stayInBoardBordersIfYouMoveOneSide = !stayInBoardBordersIfYouMoveOneSide;
            }

            return stayInBoardBordersIfYouMoveOneSide;
        }

        private bool checkYouStayInBoardBordersIfYouMoveOneSideNextToPosition(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int i_RowIndex, int i_ColumnIndex, int i_RowIndexToMoveAround, int i_ColumnIndexToMoveAround)
        {
                bool stayInBoardBordersIfYouMoveOneSide = true;
            
            if (i_RowIndex + i_RowIndexToMoveAround < 0 || i_RowIndex + i_RowIndexToMoveAround > i_OtheloGameBoard.OtheloGameBoardMatrixDimension - 1)
            { // Out of rows bounds

                stayInBoardBordersIfYouMoveOneSide = !stayInBoardBordersIfYouMoveOneSide;
            }
            else if (i_ColumnIndex + i_ColumnIndexToMoveAround < 0 || i_ColumnIndex + i_ColumnIndexToMoveAround > i_OtheloGameBoard.OtheloGameBoardMatrixDimension - 1)
            { // Out of columns bounds

                stayInBoardBordersIfYouMoveOneSide = !stayInBoardBordersIfYouMoveOneSide;
            }

            return stayInBoardBordersIfYouMoveOneSide;
        }

            public bool CheckTheUserChoiseIsLegal(char i_CurrentSignalTurn, OtheloGameBoard i_OtheloGameBoard, int[] io_PositionFromUserInTheBoard)
            {
                int rowInputIndex = io_PositionFromUserInTheBoard[0], columnInputIndex = io_PositionFromUserInTheBoard[1];
                bool userPositinChoiseIsLegal = true;

                if (m_AllCurrentLegalMovesMatrix[rowInputIndex, columnInputIndex] != i_CurrentSignalTurn)
                {
                    userPositinChoiseIsLegal = !userPositinChoiseIsLegal;
                }

                return userPositinChoiseIsLegal;
            }

            public void PutAutomaticSignalOfCompuerOnBoard(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn)
            {
            const int k_NumOfIdxesInArray = 2;

                // Ref to Random object of the System class
                Random chooseRandomPossibleCellInTheBoard = new Random();

                int randomListIndex = chooseRandomPossibleCellInTheBoard.Next(m_ListWithTheCurrentPositinsOptionsOnBoardForRandom.Count);

                int[] positionInTheBoard = new int[k_NumOfIdxesInArray]; // Array to place the 2 indexes for the current turn of the computer

                checkIfTheRandomValueFromListIsRowOrColumnIndexInTheMatrix(positionInTheBoard, randomListIndex);

                // Put the computer signal on board according the random possible cell you have
                PutSignalInTheRequiredUserPositionOnBoard(i_OtheloGameBoard, i_CurrentSignalTurn, positionInTheBoard);
            }

        // $G$ CSS-013 (-3) Input parameters names should start with i_PascaleCase.
        private void checkIfTheRandomValueFromListIsRowOrColumnIndexInTheMatrix(int[] io_PositionInTheBoard, int i_RandomListIndexToCheck)
            {
                if (i_RandomListIndexToCheck % 2 == 0)
                {
                // The even value index in list is always a row index in the matrix
                io_PositionInTheBoard[0] = m_ListWithTheCurrentPositinsOptionsOnBoardForRandom[i_RandomListIndexToCheck];

                // The odd value index in list is always a column index in the matrix
                io_PositionInTheBoard[1] = m_ListWithTheCurrentPositinsOptionsOnBoardForRandom[i_RandomListIndexToCheck + 1];
                }
                else if (i_RandomListIndexToCheck % 2 != 0)
                { // The random value is odd

                // The even value index in list is always a row index in the matrix
                io_PositionInTheBoard[0] = m_ListWithTheCurrentPositinsOptionsOnBoardForRandom[i_RandomListIndexToCheck - 1];

                // The odd value index in list is always a column index in the matrix
                io_PositionInTheBoard[1] = m_ListWithTheCurrentPositinsOptionsOnBoardForRandom[i_RandomListIndexToCheck];
                }
            }

            public void PutSignalInTheRequiredUserPositionOnBoard(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int[] io_PositionFromUserInTheBoard)
            {
                int rowInputIndex = io_PositionFromUserInTheBoard[0], columnInputIndex = io_PositionFromUserInTheBoard[1];

                // Put the signal in the game board
                i_OtheloGameBoard.OtheloGameBoardMatrix[rowInputIndex, columnInputIndex] = i_CurrentSignalTurn;

                changeAllTheOpponentSignalsYouBlockOnBoard(i_OtheloGameBoard, i_CurrentSignalTurn, rowInputIndex, columnInputIndex);
            }

            private void changeAllTheOpponentSignalsYouBlockOnBoard(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int i_RowInputIndex, int i_ColumnInputIndex)
            {
                const int k_MaxLowValueToAddColumnsAndRows = -1;
                const int k_MaxHighValueToAddColumnsAndRows = 1;

                // By 'rowIndexToMoveAround' and 'columnIndexToMoveAround' indexes you can scan all the eight sides around you
                for (int rowIndexToMoveAround = k_MaxLowValueToAddColumnsAndRows; rowIndexToMoveAround <= k_MaxHighValueToAddColumnsAndRows; rowIndexToMoveAround++)
                    {
                        for (int columnIndexToMoveAround = k_MaxLowValueToAddColumnsAndRows; columnIndexToMoveAround <= k_MaxHighValueToAddColumnsAndRows; columnIndexToMoveAround++)
                        {
                            // Irellevent to my position - only the cells around me
                            if (rowIndexToMoveAround == 0 && columnIndexToMoveAround == 0)
                            {
                                continue;
                            }

                            recursiveMethodToFlipOpponentSignalsYouBlockInSpecificRow(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowInputIndex, i_ColumnInputIndex, rowIndexToMoveAround, columnIndexToMoveAround);
                        }
                    }
            }

            private bool recursiveMethodToFlipOpponentSignalsYouBlockInSpecificRow(OtheloGameBoard i_OtheloGameBoard, char i_CurrentSignalTurn, int i_RowIndex, int i_ColumnIndex, int i_RowIndexToMoveAround, int i_ColumnIndexToMoveAround)
            {
                const bool v_StayInBoardBordersIfYouMoveOneSide = true;
                const bool v_FlipOpponentSignalsYouBlockInSpecificRow = true;
                const char k_EmptyCellInGameBoard = '\0';

                if (checkYouStayInBoardBordersIfYouMoveOneSideNextToPosition(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowIndex, i_ColumnIndex, i_RowIndexToMoveAround, i_ColumnIndexToMoveAround) == !v_StayInBoardBordersIfYouMoveOneSide)
                {
                    return !v_FlipOpponentSignalsYouBlockInSpecificRow;
                }

                // If we arrive to empty cell before we have a signal type like me on board - so we can't block this row
                if (i_OtheloGameBoard.OtheloGameBoardMatrix[i_RowIndex + i_RowIndexToMoveAround, i_ColumnIndex + i_ColumnIndexToMoveAround] == k_EmptyCellInGameBoard)
                {
                    return !v_FlipOpponentSignalsYouBlockInSpecificRow;
                }

                // If we arrive to cell with same signal type - so it is the end cell of blocking this row
                if (i_OtheloGameBoard.OtheloGameBoardMatrix[i_RowIndex + i_RowIndexToMoveAround, i_ColumnIndex + i_ColumnIndexToMoveAround] == i_CurrentSignalTurn)
                {
                    return v_FlipOpponentSignalsYouBlockInSpecificRow;
                }

                // Recursive calling
                if (recursiveMethodToFlipOpponentSignalsYouBlockInSpecificRow(i_OtheloGameBoard, i_CurrentSignalTurn, i_RowIndex + i_RowIndexToMoveAround, i_ColumnIndex + i_ColumnIndexToMoveAround, i_RowIndexToMoveAround, i_ColumnIndexToMoveAround) == v_FlipOpponentSignalsYouBlockInSpecificRow)
                {
                // Flip the signal cell on board
                i_OtheloGameBoard.OtheloGameBoardMatrix[i_RowIndex + i_RowIndexToMoveAround, i_ColumnIndex + i_ColumnIndexToMoveAround] = i_CurrentSignalTurn;
                    return v_FlipOpponentSignalsYouBlockInSpecificRow;
                }
                else
                {
                    return !v_FlipOpponentSignalsYouBlockInSpecificRow;
                }
            }
        }
    }