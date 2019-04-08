using System;
using System.Collections.Generic;
using System.Reflection;

namespace GuessANumber
{
    public class GuessingGame{
        private Random rand = new Random();
        private GameSettings settings;

        private List<int> guessLog = new List<int>();
        private bool playerWon = false;

        int goalNumber;

        public GuessingGame(Difficulties difficulty)
        {
            UpdateSettings(difficulty);
        }
        public GuessingGame(GameSettings set)
        {
            UpdateSettings(set);
        }
        public void UpdateSettings(Difficulties difficulty)
        {
            UpdateSettings(difficulty.Settings());
        }
        public void UpdateSettings(GameSettings set)
        {
            settings = set;
            ResetGame();
        }

        public void ResetGame()
        {
            guessLog.Clear();
            playerWon = false;

            goalNumber = rand.Next() % (settings.randomNumberMax - settings.randomNumberMin + 1) + settings.randomNumberMin;
        }

        public void RunGame()
        {
            while(guessLog.Count < settings.maxGuessCount)
            {
                int remaining = (settings.maxGuessCount - guessLog.Count);
                Console.Write(remaining + " guess" + (remaining > 1? "es": "") + " remaining! What is your next guess? ");
                string userIn = Console.ReadLine();
                int guess;
                if(int.TryParse(userIn, out guess))
                {
                    if (guessLog.Contains(guess))
                    {
                        Console.WriteLine("Oh! Looks like you've guessed that already. Let's try that again...");
                    }
                    else
                    {
                        if (guess > settings.randomNumberMax || guess < settings.randomNumberMin)
                        {
                            Console.WriteLine("Make sure your guesses are within the range between " + settings.randomNumberMin.ToString() + " and " + settings.randomNumberMax.ToString());
                        }
                        else
                        {
                            if (guess == goalNumber)
                            {
                                Console.WriteLine("We have a winner! Good Job.");
                                guessLog.Add(guess);
                                break;
                            }
                            else if (guess > goalNumber)
                            {
                                Console.WriteLine("Too high");
                            }
                            else if (guess < goalNumber)
                            {
                                Console.WriteLine("Too low");
                            }
                            guessLog.Add(guess);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("This is a NUMBER guessing game... Please try to guess a number.");
                }
            }

            if(guessLog.Count >= settings.maxGuessCount)
            {
                Console.Write("You ran out of guesses after guessing ");
                for(int i = 0; i < guessLog.Count; i++)
                {
                    Console.Write("" + guessLog[i] + "-");
                }
                Console.Write(" The awnswer was: " + goalNumber + ".\n");
            }
            else
            {
                Console.WriteLine("You correctly guessed that the answer was " + goalNumber + " in " + guessLog.Count + " guess" + (guessLog.Count > 1 ? "es" : "") + "!");
            }
        }
    }
}
