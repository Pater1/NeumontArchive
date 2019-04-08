using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessANumber
{
    class GameManager{
        private GuessingGame game = null;

        public void LetTheGamesBegin()
        {
            while (true)
            {
                Difficulties difficulty = PromptForDifficulty();
                if (game == null) game = new GuessingGame(difficulty);
                else game.UpdateSettings(difficulty);

                game.RunGame();

                while (true)
                {
                    Console.Write("Play another game? [Y/N] ");
                    char cont = char.ToUpper(Console.ReadLine()[0]);
                    if (cont == 'Y') break;
                    else if (cont == 'N') return;
                    else
                    {
                        Console.WriteLine("Please respond with either \'Y\' or \'N\'.");
                    }
                }
            }
        }

        private Difficulties PromptForDifficulty()
        {
            var vals = Enum.GetValues(typeof(Difficulties)).Cast<Difficulties>();
            List<Difficulties> diffs = new List<Difficulties>();
            foreach(Difficulties dif in vals)
            {
                Console.WriteLine(dif.ToString() + " - " + (diffs.Count+1));
                diffs.Add(dif);
            }

            Console.WriteLine("Select your difficutly: ");
            while (true)
            {
                string input = Console.ReadLine();
                int i;
                if(int.TryParse(input, out i))
                {
                    i--;
                    if(i < diffs.Count)
                    {
                        return diffs[i];
                    }
                    else
                    {
                        Console.WriteLine("Please select a difficulty using a number between 1 and " + (diffs.Count).ToString() + ". Try again: ");
                    }
                }
                else
                {
                    Console.WriteLine("Please select a difficulty using a number between 1 and " + (diffs.Count).ToString() + ". Try again: ");
                }
            }
        }
    }
}
