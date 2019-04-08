using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSM {
    public class State<T> {
        public State(string id, bool start = false, string retur = null) {
            ID = id;

            Start = start;
            Return = retur;
        }
        public State(IEnumerable<Transition<T>> outTransitions, string id, bool start = false, string retur = null) {
            ID = id;

            Start = start;
            Return = retur;

            IEnumerable<Transition<T>> Transitions = outTransitions.Select(x => {
                x.FromID = this.ID;
                return x;
            }).ToArray();

            DirectTransitions = Transitions.Where(x => x is DirectTransition<T>).Cast<DirectTransition<T>>();
            EpsilonTransitions = Transitions.Where(x => x is EpsilonTransition<T>).Cast<EpsilonTransition<T>>();
            FailTransition = Transitions.Where(x => x is FailTransition<T>).Cast<FailTransition<T>>().SingleOrDefault();
        }

        public string ID { get; internal set; }
        public bool Start { get; internal set; }
        public bool Accepted {
            get {
                return Return != null;
            }
        }
        public string Return { get; set; } = null;

        public IEnumerable<string> Next(T on, StateMachine<T> stateMachine){
            List<string> ret = new List<string>();

            IEnumerable<EpsilonTransition<T>> et = EpsilonTransitions;
            while(et != null && et.Any()){
                var curFollow = et.Select(x => x.Follow(on)).Except(ret);

                ret.AddRange(curFollow);
                        //convert state names to state objects
                et = et .Select(x => stateMachine.QueryStates(curFollow)).SelectMany(x => x)
                        //pull each epsilon transition for each state
                        .Select(x => x.EpsilonTransitions).SelectMany(x => x).Distinct().ToArray();
            }

            ret.AddRange(stateMachine.QueryStates(ret).Select(x => x.DirectTransitions).SelectMany(x => x).Select(x => x.Follow(on)).Where(x => x != null).Distinct());
            ret.AddRange(DirectTransitions.Select(x => x.Follow(on)).Where(x => x != null));

            ret = ret.Distinct().ToList();

            if(ret.Any() ){
                return ret;
            }else{
                return new string[] { FailTransition == null?
                    (stateMachine.IgnoreDefaultFail? null: stateMachine.DefaultFail.Follow(on))
                    : FailTransition.Follow(on) }.Where(x => x != null).ToArray();
            }
        }

        public IEnumerable< DirectTransition<T>> DirectTransitions { get; internal set; }
        public IEnumerable<EpsilonTransition<T>> EpsilonTransitions { get; internal set; }
        public FailTransition<T> FailTransition{ get; internal set; }
    }
}