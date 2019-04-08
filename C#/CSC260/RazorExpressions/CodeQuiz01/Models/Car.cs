using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeQuiz01.Models {
    public class Car : Vehicle {
        public override string Operation() {
            return "drive";
        }
    }
}