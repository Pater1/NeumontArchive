using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessANumber
{
    public struct GameSettings
    {
        public GameSettings(int randMin, int randMax, int guesses)
        {
            randomNumberMin = randMin;
            randomNumberMax = randMax;
            maxGuessCount = guesses;
        }
        public int randomNumberMin { get; private set; }
        public int randomNumberMax { get; private set; }
        public int maxGuessCount { get; private set; }
    }
}
