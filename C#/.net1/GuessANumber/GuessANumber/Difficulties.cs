using System.Collections.Generic;

namespace GuessANumber
{
    public enum Difficulties : int
    {
        Easy = 1,
        Medium = 64,
        Hard = 1024
    }
    public static class DifficultiesExtentions
    {
        private static readonly Dictionary<Difficulties, GameSettings> difficultyMap = new Dictionary<Difficulties, GameSettings>
        {
            {Difficulties.Easy, new GameSettings(1, 10, 5)},
            {Difficulties.Medium, new GameSettings(1, 50, 5)},
            {Difficulties.Hard, new GameSettings(1, 100, 5)}
        };

        public static GameSettings Settings(this Difficulties diff)
        {
            return difficultyMap[diff];
        }
    }
}
