using System;

namespace Ex02_Othelo
{
    public class OtheloGamePlayer
    {
        private const char k_PlayerSignalKind1 = 'X';
        private const char k_PlayerSignalKind2 = 'O';
        private const int k_DefaultStartingPointsScore = 0;
        private string m_PlayerName;
        private char m_PlayerSignal;
        private int m_PlayerPoints;
        private OtheloGameBoard.eGameTypes m_StateGame;

        public OtheloGamePlayer()
        {
        }

        public OtheloGamePlayer(string i_PlayerName, char i_PlayerSignal, OtheloGameBoard.eGameTypes i_StateGame)
        {
            PlayerName = i_PlayerName;
            PlayerSignal = i_PlayerSignal;
            StateGame = i_StateGame;
            m_PlayerPoints = k_DefaultStartingPointsScore;
        }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }

            set
            {
                m_PlayerName = value;
            }
        }

        public char PlayerSignal
        {
            get
            {
                return m_PlayerSignal;
            }

            set
            {
                m_PlayerSignal = value;
            }
        }

        public int PlayerPoints
        {
            get
            {
                return m_PlayerPoints;
            }

            set
            {
                m_PlayerPoints = value;
            }
        }

        public OtheloGameBoard.eGameTypes StateGame
        {
            get
            {
                return m_StateGame;
            }

            set
            {
                m_StateGame = value;
            }
        }
    }
}
