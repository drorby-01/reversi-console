using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// $G$ RUL-006 (-40) Late submission - 1 day.

// $G$ SFN-005 (-7) Unable to play versus a computer player. -  Unable to choose a move.
// $G$ SFN-005 (-7) Unable to play versus a computer player. -  Unable to choose a move.

// $G$ CSS-999 (-20) StyleCop errors.

namespace Ex02_Othelo
{
    public class Program
    {
        public static void Main()
        {
            ConsoleGUI newOtheloGame = new ConsoleGUI();
            newOtheloGame.StartOtheloGame();
        }
    }
}
