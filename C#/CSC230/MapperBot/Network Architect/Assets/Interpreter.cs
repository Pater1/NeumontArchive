using System;
using UnityEngine;

namespace Assets {
    public abstract class Interpreter<T>: MonoBehaviour {
        public abstract Command<T> Interpret(GrammarTreeNode<string> sentance);
    }
}