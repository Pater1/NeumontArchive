using System;
using System.Collections.Generic;
using System.Linq;

namespace FSM {
    public class StateMachine<T> {
        private IEnumerable<State<T>> _currentStates;

        public IEnumerable<T> Alphabet => Transitions.Where(x => x is DirectTransition<T>).Cast<DirectTransition<T>>().Select(x => x.On).Distinct();
        public ICollection<State<T>> States { get; private set; }
        public ICollection<Transition<T>> Transitions { get; private set; }

        public bool Deterministic {
            get {
                //No Epsilon transitions
                return !States.Select(x => x.EpsilonTransitions).SelectMany(x => x).Any() &&
                       //No transitions on the same 'character'
                       !States.Select(x => {
                           var q = x.DirectTransitions.Select(y => y.On);
                           var w = q.Distinct();
                           return q.Count() == w.Count();
                       }).Where(x => !x).Any();
            }
        }

        private IEnumerable<State<T>> CurrentStates {
            get {
                if(_currentStates == null){
                    _currentStates = StartStates.ToArray();
                }
                return _currentStates;
            }
            set {
                if(value == null) {
                    _currentStates = StartStates.ToArray();
                } else {
                    _currentStates = value;
                }
            }
        }

        private IEnumerable<State<T>> StartStates => States.Where(x => x.Start);
        private IEnumerable<State<T>> AcceptedStates => States.Where(x => x.Accepted);

        public bool IgnoreDefaultFail { get; internal set; }
        public FailTransition<T> DefaultFail { get; internal set; }

        public StateMachine(IEnumerable<State<T>> states, IEnumerable<Transition<T>> transitions) {
            States = states.ToList();
            Transitions = transitions.ToList();
        }
        public StateMachine(IEnumerable<State<T>> states) {
            States = states.ToList();

            Transitions = states.Select(x => new IEnumerable<Transition<T>>[]{
                x.DirectTransitions.Cast<Transition<T>>(),
                x.EpsilonTransitions.Cast<Transition<T>>(),
                new Transition<T>[]{ x.FailTransition },
            }).SelectMany(x => x).SelectMany(x => x).ToList();

            CurrentStates = States.Where(x => x.Start).ToArray();
        }
        private StateMachine() {
            States = new List<State<T>>();
            Transitions = new List<Transition<T>>();
        }

        public StateMachine(IEnumerable<State<T>> states, FailTransition<T> defaultFail) : this(states) {
            DefaultFail = defaultFail;
        }

        internal State<T>[] QueryStates(IEnumerable<string> v) => States.Where(x => v.Contains(x.ID)).ToArray();

        public void Next(T input) {
            CurrentStates = QueryStates(CurrentStates.Select(x => x.Next(input, this)).SelectMany(x => x).Distinct());
        }
        public string[] Ingest(IEnumerable<T> input) {
            foreach(T t in input) {
                Next(t);
            }

            string[] ret = CurrentStates.Select(x => x.Return).Distinct().Where(x => x != null).ToArray();
            Reset();
            return ret;
        }
        public void Reset() {
            CurrentStates = StartStates.ToArray();
        }

        public StateMachine<T> Render() {
            if(Deterministic) return this;

            StateMachine<T> SourceMachine = this; SourceMachine.IgnoreDefaultFail = true;
            StateMachine<T> TargetMachine = new StateMachine<T>();
            IEnumerable<State<T>> InitialSet = SourceMachine.StartStates.Distinct();
            State<T> InitialState = InitialSet.Colapse(/*""*/); InitialState.Start = true; TargetMachine.States.Add(InitialState);
            Queue<(IEnumerable<State<T>>, string)> Queue = new Queue<(IEnumerable<State<T>>, string)>(); Queue.Enqueue((InitialSet, ""));
            while(Queue.Count > 0) {
                Console.Write(Queue.Count + ",");
                (IEnumerable<State<T>> CurrentSet, string CurrentSequence) = Queue.Dequeue();
                State<T> CurrentState = TargetMachine.QueryStates(new string[] { CurrentSet.Colapse().ID })[0];
                foreach(T symbol in SourceMachine.Alphabet) {
                    string NewStateReturn = null;
                    FailTransition<T> NewStateFail = null;
                    List<State<T>> NewStateSet = new List<State<T>>();
                    foreach(State<T> state in CurrentSet) {
                        IEnumerable<State<T>> NextStates = SourceMachine.QueryStates(state.Next(symbol, SourceMachine).Distinct());
                        foreach(State<T> nextState in NextStates) {
                            NewStateSet.Add(nextState);
                            if(nextState.Accepted) {
                                NewStateReturn = nextState.Return;
                            }
                            if(nextState.FailTransition != null){
                                NewStateFail = nextState.FailTransition;
                            }
                        }
                    }
                    if(NewStateSet.Any()) {
                        State<T> NewState = NewStateSet.Distinct().Colapse(/*CurrentSequence + symbol.ToString()*/); 
                        NewState.Return = NewStateReturn;
                        NewState.FailTransition = NewStateFail;
                        if(!TargetMachine.States.Select(x => x.ID).Contains(NewState.ID)) {
                            TargetMachine.States.Add(NewState);
                            Queue.Enqueue((NewStateSet.Distinct(), CurrentSequence + symbol.ToString()));
                        }
                        TargetMachine.Transitions.Add(new DirectTransition<T>(CurrentState.ID, NewState.ID, symbol));
                    }
                }
            }
            TargetMachine.DefaultFail = new FailTransition<T>(InitialState.ID);

            SourceMachine.IgnoreDefaultFail = false;
            TargetMachine.IngestTransitions();
            Console.WriteLine(0);
            Console.WriteLine("Rendered deterministic? " + TargetMachine.Deterministic);
            Console.WriteLine();
            return TargetMachine;
        }

        private void IngestTransitions() {
            foreach(State<T> state in States){
                IEnumerable<Transition<T>> t = Transitions.Where(x => x.FromID == state.ID).ToArray();
                state.DirectTransitions = t.Where(x => x is DirectTransition<T>).Cast<DirectTransition<T>>().ToArray();
                state.EpsilonTransitions = t.Where(x => x is EpsilonTransition<T>).Cast<EpsilonTransition<T>>().ToArray();
            }
        }
    }
    public static class StateExtentions{
        public static State<T> Colapse<T>(this IEnumerable<State<T>> enu, string ID = null) =>
            new State<T>(ID != null? ID : new string(enu.Select(x => x.ID).Distinct().OrderBy(x=>x).SelectMany(x => x + ",").ToArray()));

        private static Random rand = new Random();
        public static long NextLong(this Random ran){
            int a = ran.Next(), b = ran.Next();
            return ((long)a << 32) | (long)b;
        }
        public static T Random<T>(this IEnumerable<T> enu){
            long count = enu.LongCount();
            return enu.Skip((int)(rand.NextLong() % count)).Take(1).Single();
        }
    }
}
