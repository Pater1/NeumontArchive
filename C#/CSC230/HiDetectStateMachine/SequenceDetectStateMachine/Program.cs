using System;
using System.Collections.Generic;
using System.Linq;

namespace HowdyDetectStateMachine {
    class Program {
        [Flags]
        enum StateSymbol: long {
            _           = 1 << 0,
            H           = 1 << 1,
            hi          = 1 << 2,
            he          = 1 << 3,
            hel         = 1 << 4,
            hell        = 1 << 5,
            hello       = 1 << 6,
            ho          = 1 << 7,
            how         = 1 << 8,
            howd        = 1 << 9,
            howdy       = 1 << 10,
            A           = 1 << 11,
            al          = 1 << 12,
            alo         = 1 << 13,
            aloh        = 1 << 14,
            aloha       = 1 << 15,
            T           = 1 << 16,
            th          = 1 << 17,
            tha         = 1 << 18,
            than        = 1 << 19,
            thank       = 1 << 20,
            thanks      = 1 << 21,
            thank_      = 1 << 22,
            thank_y     = 1 << 23,
            thank_yo    = 1 << 24,
            thank_you   = 1 << 25,

            Start = _,
            Accepted = hi | hello | howdy | aloha | thanks | thank_you
        }
        class State {
            public Func<State, char, StateSymbol> Logic { get; private set; }
            public StateSymbol ThisSymbol { get; private set; }
            public bool Accepted => (ThisSymbol & StateSymbol.Accepted) != 0;
            public State(StateSymbol thisSymbol, Func<State, char, StateSymbol> logic) {
                ThisSymbol = thisSymbol;
                Logic = logic;
            }
            public StateSymbol Next(char ch) => Logic(this, ch);
        }
        static Dictionary<StateSymbol, State> Map = new State[] {
            new State(StateSymbol._,
                (s, x) => {
                    switch(x){
                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.H,
                (s, x) => {
                    switch(x){
                        case 'i':
                            return StateSymbol.hi;
                        case 'o':
                            return StateSymbol.ho;
                        case 'e':
                            return StateSymbol.he;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.hi,
                (s, x) => {
                    switch(x){
                        default:
                            return StateSymbol.hi;
                    }
                }
            ),

            new State(StateSymbol.he,
                (s, x) => {
                    switch(x){
                        case 'l':
                            return StateSymbol.hel;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.hel,
                (s, x) => {
                    switch(x){
                        case 'l':
                            return StateSymbol.hell;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.hell,
                (s, x) => {
                    switch(x){
                        case 'o':
                            return StateSymbol.hello;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.hello,
                (s, x) => {
                    switch(x){
                        default:
                            return StateSymbol.hello;
                    }
                }
            ),

            new State(StateSymbol.ho,
                (s, x) => {
                    switch(x){
                        case 'w':
                            return StateSymbol.how;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.how,
                (s, x) => {
                    switch(x){
                        case 'd':
                            return StateSymbol.howd;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.howd,
                (s, x) => {
                    switch(x){
                        case 'y':
                            return StateSymbol.howdy;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.howdy,
                (s, x) => {
                    switch(x){
                        default:
                            return StateSymbol.howdy;
                    }
                }
            ),

            new State(StateSymbol.A,
                (s, x) => {
                    switch(x){
                        case 'l':
                            return StateSymbol.al;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.al,
                (s, x) => {
                    switch(x){
                        case 'o':
                            return StateSymbol.alo;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.alo,
                (s, x) => {
                    switch(x){
                        case 'h':
                            return StateSymbol.aloh;


                        case 'H':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.aloh,
                (s, x) => {
                    switch(x){
                        case 'a':
                            return StateSymbol.aloha;
                        case 'i':
                            return StateSymbol.hi;
                        case 'o':
                            return StateSymbol.ho;
                        case 'e':
                            return StateSymbol.he;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.aloha,
                (s, x) => {
                    switch(x){
                        default:
                            return StateSymbol.aloha;
                    }
                }
            ),

            new State(StateSymbol.T,
                (s, x) => {
                    switch(x){
                        case 'h':
                            return StateSymbol.th;


                        case 'H':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.th,
                (s, x) => {
                    switch(x){
                        case 'i':
                            return StateSymbol.hi;
                        case 'a':
                            return StateSymbol.tha;
                        case 'o':
                            return StateSymbol.ho;
                        case 'e':
                            return StateSymbol.he;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.tha,
                (s, x) => {
                    switch(x){
                        case 'l':
                            return StateSymbol.al;
                        case 'n':
                            return StateSymbol.than;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.than,
                (s, x) => {
                    switch(x){
                        case 'k':
                            return StateSymbol.thank;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.thank,
                (s, x) => {
                    switch(x){
                        case 's':
                            return StateSymbol.thanks;
                        case ' ':
                            return StateSymbol.thank_;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.thanks,
                (s, x) => {
                    switch(x){
                        default:
                            return StateSymbol.thanks;
                    }
                }
            ),

            new State(StateSymbol.thank_,
                (s, x) => {
                    switch(x){
                        case 'y':
                            return StateSymbol.thank_y;
                        case ' ':
                            return StateSymbol.thank_;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.thank_y,
                (s, x) => {
                    switch(x){
                        case 'o':
                            return StateSymbol.thank_yo;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.thank_yo,
                (s, x) => {
                    switch(x){
                        case 'u':
                            return StateSymbol.thank_you;


                        case 'H':
                        case 'h':
                            return StateSymbol.H;

                        case 'A':
                        case 'a':
                            return StateSymbol.A;

                        case 'T':
                        case 't':
                            return StateSymbol.T;

                        default:
                            return StateSymbol._;
                    }
                }
            ),
            new State(StateSymbol.thank_you,
                (s, x) => {
                    switch(x){
                        default:
                            return StateSymbol.thank_you;
                    }
                }
            ),
        }.ToDictionary(x => x.ThisSymbol, x => x);

        static Dictionary<StateSymbol, string[]> Accepteds = new Dictionary<StateSymbol, string[]> {
            {StateSymbol.hi, new string[]{"Hi", "sup?"} },
            {StateSymbol.hello,
                new string[]{
                    "Hello",
                    "How are you?"
                }
            },
            {StateSymbol.howdy,
                new string[]{
                    "Howdy partner"
                }
            },
            {StateSymbol.aloha,
                new string[]{
                    "Aloha",
                    "Serf's up!"
                }
            },
            {StateSymbol.thanks,
                new string[]{
                    "You're welcome",
                    "No, thank you!"
                }
            },
            {StateSymbol.thank_you,
                new string[]{
                    "You're welcome",
                    "No, thank you!"
                }
            },
        };
        static string[] Rejecteds = new string[]{
            "What great weather!",
            "Tell me about yourself",
            "What do you like to do?",
            "What makes you sad?"
        };

        static void Main(string[] args) {
            string input = "";
            Random rand = new Random();
            while(input != "exit") {
                input = Console.ReadLine();

                StateSymbol curState = StateSymbol.Start;
                for(int i = 0; i < input.Length; i++) {
                    State s = Map[curState];
                    curState = s.Next(input[i]);
                }

                string[] responces = null;
                if(Map[curState].Accepted) {
                    responces = Accepteds[curState];
                } else {
                    responces = Rejecteds;
                }
                Console.WriteLine(responces[rand.Next() % responces.Length]);
                Console.WriteLine();
            }
        }
    }
}
