using System;

namespace Ex02_Othelo
{
    public class OtheloGameBoard
    {
        private const char k_PlayerSignalKind1 = 'X';
        private const char k_PlayerSignalKind2 = 'O';
        private char[,] m_OtheloGameBoardMatrix;
        private int m_OtheloGameBoardMatrixDimension;
        private int m_NumOfFirstPlayerSignalsOnBoard;
        private int m_NumOfSecondPlayerSignalsOnBoard;

        public enum eGameTypes
        {
            Player,
            Computer,
        }

        public OtheloGameBoard(int i_OtheloGameBoardMatrixDimension)
        {
            m_OtheloGameBoardMatrixDimension = i_OtheloGameBoardMatrixDimension;
            m_OtheloGameBoardMatrix = new char[m_OtheloGameBoardMatrixDimension, m_OtheloGameBoardMatrixDimension];

            InitializeOtheloGameBoard();
        }
    
        public void InitializeOtheloGameBoard()
        {
            zeroAllTheBoardCells();

            initializationTheCenterCellsOfTheBoard();
            
            // In the begining status we have only 2 signals from every kind on the board
            m_NumOfFirstPlayerSignalsOnBoard = 2;
            m_NumOfSecondPlayerSignalsOnBoard = 2;
        }

        private void initializationTheCenterCellsOfTheBoard()
        {
            m_OtheloGameBoardMatrix[m_OtheloGameBoardMatrixDimension / 2, m_OtheloGameBoardMatrixDimension / 2] = k_PlayerSignalKind2;
            m_OtheloGameBoardMatrix[(m_OtheloGameBoardMatrixDimension / 2) - 1, (m_OtheloGameBoardMatrixDimension / 2) - 1] = k_PlayerSignalKind2;
            m_OtheloGameBoardMatrix[(m_OtheloGameBoardMatrixDimension / 2) - 1, m_OtheloGameBoardMatrixDimension / 2] = k_PlayerSignalKind1;
            m_OtheloGameBoardMatrix[m_OtheloGameBoardMatrixDimension / 2, (m_OtheloGameBoardMatrixDimension / 2) - 1] = k_PlayerSignalKind1;
        }

        private void zeroAllTheBoardCells()
        {
            const char k_EmptyCellInGameBoard = '\0';

            for (int i = 0; i < this.m_OtheloGameBoardMatrixDimension; i++)
            {
                for (int j = 0; j < this.m_OtheloGameBoardMatrixDimension; j++)
                {
                    this.m_OtheloGameBoardMatrix[i, j] = k_EmptyCellInGameBoard;
                }
            }
        }

        public char[,] OtheloGameBoardMatrix
        {
            get
            {
                return m_OtheloGameBoardMatrix;
            }

            set
            {
                this.m_OtheloGameBoardMatrix = value;
            }
        }

        public int OtheloGameBoardMatrixDimension
        {
            get
            {
                return m_OtheloGameBoardMatrixDimension;
            }

            set
            {
                this.m_OtheloGameBoardMatrixDimension = value;
            }
        }

        public int NumOfFirstPlayerSignalsOnBoard
        {
            get
            {
                return m_NumOfFirstPlayerSignalsOnBoard;
            }

            set
            {
                this.m_NumOfFirstPlayerSignalsOnBoard = value;
            }
        }

        public int NumOfSecondPlayerSignalsOnBoard
        {
            get
            {
                return m_NumOfSecondPlayerSignalsOnBoard;
            }

            set
            {
                this.m_NumOfSecondPlayerSignalsOnBoard = value;
            }
        }

        public void PrintOtheloGameBoard()
        {
            char rowPositionSign = '1';

            Ex02.ConsoleUtils.Screen.Clear(); // Clean the console before printing the game board

            zeroTheSignalCountersOfTheBoard();

            printTheColumnSigns();

            printTheHorizontalSeparationSign(); // Print the horizontal separation sign after the column signs

            for (int i = 0; i < this.m_OtheloGameBoardMatrixDimension; i++)
            {
                printSingleRowInTheBoard(rowPositionSign);

                printTheHorizontalSeparationSign(); // Print the horizontal separation sign after every row

                rowPositionSign++;
            }

            Console.WriteLine();
        }

        private void zeroTheSignalCountersOfTheBoard()
        {
            this.m_NumOfFirstPlayerSignalsOnBoard = 0;
            this.m_NumOfSecondPlayerSignalsOnBoard = 0;
        }

        public void CountAndUpdateHowManySignalsOnBoardFromAnyKind()
        {
            for (int i = 0; i < this.m_OtheloGameBoardMatrixDimension; i++)
            {
                for (int j = 0; j < this.m_OtheloGameBoardMatrixDimension; j++)
                {
                   if (this.m_OtheloGameBoardMatrix[i, j] == k_PlayerSignalKind2 )
                    {
                        this.m_NumOfSecondPlayerSignalsOnBoard++;
                    }
                    else if (this.m_OtheloGameBoardMatrix[i, j] == k_PlayerSignalKind1 )
                    {
                        this.m_NumOfFirstPlayerSignalsOnBoard++;
                    }
                }
            }
        }

        public char GetTheOppositeSignal(char i_MySignalType)
        {
            char playerSignalToReturn;

            if(i_MySignalType == k_PlayerSignalKind1)
            {
                playerSignalToReturn = k_PlayerSignalKind2;
            }
            else
            {
                playerSignalToReturn = k_PlayerSignalKind1;
            }

            return playerSignalToReturn;
        }

        private void printTheColumnSigns()
        {
            char columnPositionSign = 'A';

            Console.Write("     ");
            
            for (int i = 0; i < m_OtheloGameBoardMatrixDimension; i++)
            {
                Console.Write("{0}   ", columnPositionSign);
                columnPositionSign++;
            }
        }

        private void printTheHorizontalSeparationSign()
        {
            Console.WriteLine();
            Console.Write("   =");

            for (int i = 0; i < m_OtheloGameBoardMatrixDimension; i++)
            {
                    Console.Write("====");
            }
        }

        private void printSingleRowInTheBoard(char i_RowPositionSign)
        {
            string startOfRow, strCellValue;
            int rowIndex = i_RowPositionSign - '1';

            // Print the begining of the row
            startOfRow = string.Format(" {0} |", i_RowPositionSign);

            Console.WriteLine();
            Console.Write(startOfRow);

            // Print the cell values in the row
            for (int i = 0; i < this.m_OtheloGameBoardMatrixDimension; i++)
            {   
                strCellValue = string.Format(" {0} |", this.m_OtheloGameBoardMatrix[rowIndex, i]);
                Console.Write(strCellValue);
            }
        }

        public bool CheckTheInputRowNumberIsNotOutOfBoundsOfTheGameBoard(int i_IntRowNumberPositionToCheck)
        {
            bool rowNumberIsNotOutOfBoundsOfTheGameBoard = true;

            if(i_IntRowNumberPositionToCheck < 1 || i_IntRowNumberPositionToCheck > this.m_OtheloGameBoardMatrixDimension)
            {
                rowNumberIsNotOutOfBoundsOfTheGameBoard = !rowNumberIsNotOutOfBoundsOfTheGameBoard;
            }

            return rowNumberIsNotOutOfBoundsOfTheGameBoard;
        }

        public bool CheckTheInputColumnLetterIsNotOutOfBoundsOfTheGameBoard(string i_StrColumnLetterPositionToCheck)
        {
            int onlyOneCharToCheck = 1;
            const char k_FirstColumnLetterInBoard = 'A';
            const char k_LastColumnLetterInSmallBoard = 'F';
            const char k_LastColumnLetterInBigBoard = 'H';
            const int k_SmallGameBoardSize = 6;
            const int k_BigGameBoardSize = 8;

            bool columnLetterIsNotOutOfBoundsOfTheGameBoard = true;
            
                if((i_StrColumnLetterPositionToCheck.Length != onlyOneCharToCheck) || 
                   (i_StrColumnLetterPositionToCheck[0] < k_FirstColumnLetterInBoard) ||
                   (m_OtheloGameBoardMatrixDimension == k_SmallGameBoardSize && i_StrColumnLetterPositionToCheck[0] > k_LastColumnLetterInSmallBoard) ||
                   (m_OtheloGameBoardMatrixDimension == k_BigGameBoardSize && i_StrColumnLetterPositionToCheck[0] > k_LastColumnLetterInBigBoard))
                {
                    columnLetterIsNotOutOfBoundsOfTheGameBoard = !columnLetterIsNotOutOfBoundsOfTheGameBoard;
                }

            return columnLetterIsNotOutOfBoundsOfTheGameBoard;
        }

        public bool CheckTheOtheloBoardGameHasFreePlace()
        {
            const char k_EmptyCellInGameBoard = '\0';
            bool hasFreePlaceOnBoardGame = true;
            int BoardGameIsFull = 0;

            for (int i = 0; i < this.m_OtheloGameBoardMatrixDimension; i++)
            {
                for(int j = 0; j < this.m_OtheloGameBoardMatrixDimension; j++)
                {
                    if(this.m_OtheloGameBoardMatrix[i, j] == k_EmptyCellInGameBoard)
                    {
                        i = this.m_OtheloGameBoardMatrixDimension;
                        BoardGameIsFull = 0;
                        break; // Finish the loops cause you know there is atleast one free cell on board
                    }
                }
            }

            if(BoardGameIsFull == 1)
            {
                hasFreePlaceOnBoardGame = !hasFreePlaceOnBoardGame;
            }

            return hasFreePlaceOnBoardGame;
        }

        public bool CheckTheRequiredPositionOnBoardIsEmpty(int i_RowIndex, int i_ColIndex)
        {
            bool requiredCellIsEmpty = true;

            if (this.m_OtheloGameBoardMatrix[i_RowIndex, i_ColIndex] == k_PlayerSignalKind1 || this.m_OtheloGameBoardMatrix[i_RowIndex, i_ColIndex] == k_PlayerSignalKind2)
            {
                requiredCellIsEmpty = !requiredCellIsEmpty;
            }

            return requiredCellIsEmpty;
        }
    }
}
