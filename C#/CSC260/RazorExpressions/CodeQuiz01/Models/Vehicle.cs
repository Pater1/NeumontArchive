using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeQuiz01.Models {
    public abstract class Vehicle {
        public string TypeName => this.GetType().Name;

        public abstract string Operation();
    }
}