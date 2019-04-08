using System;
using System.Collections.Generic;
using System.Text;

namespace FSM {
    public abstract class Transition<T> {
        protected Transition(string toState) {
            ToID = toState;
        }
        protected Transition(string fromState, string toState) {
            ToID = toState;
            FromID = fromState;
        }

        public string ToID { get; internal set; }
        public string FromID { get; internal set; }
        public abstract string Follow(T on);
    }
    public class DirectTransition<T>: Transition<T> {
        public DirectTransition(string toState, T on) : base(toState) {
            On = on;
        }
        public DirectTransition(string fromState, string toState, T on) : base(fromState, toState) {
            On = on;
        }

        public T On { get; private set; }
        public override string Follow(T on) {
            if(EqualityComparer<T>.Default.Equals(On, on)) {
                return ToID;
            } else {
                return null;
            }
        }
    }
    public class EpsilonTransition<T>: Transition<T> {
        public EpsilonTransition(string toState) : base(toState) { }
        public EpsilonTransition(string fromState, string toState) : base(fromState, toState) { }
        public override string Follow(T on) => ToID;
    }
    public class FailTransition<T>: Transition<T> {
        public FailTransition(string toState) : base(toState) { }
        public FailTransition(string fromState, string toState) : base(fromState, toState) { }
        public override string Follow(T on) => ToID;
    }
}
