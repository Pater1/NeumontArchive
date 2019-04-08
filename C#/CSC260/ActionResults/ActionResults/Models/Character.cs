using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ActionResults.Models {
    [Serializable]
    public class Character {
        public string Name { get; set; } = "Person Name";
        public int Level { get; set; } = 4;
        public int HealthPoints { get; set; } = 3;
        [XmlIgnore]
        public Dictionary<string, int> Attributes { get; set; } =
            new Dictionary<string, int> {
                {"IQ", 10},
                {"ME", 9},
                {"MA", 8},
                {"PS", 3},
                {"PP", 4},
                {"PE", 5},
                {"PB", 6},
                {"Spd", 7},
            };

        public override string ToString() {
            string ret = "/*This is a string!*/\n{"+$"\n\tName: {Name},\n\tLevel: {Level}\n\tHP: {HealthPoints}\n\tAttributes: "+"{";
            foreach(KeyValuePair<string, int> kv in Attributes) {
                ret += $"\n\t\t{kv.Key}: {kv.Value}";
            }
            ret += "\n\t}\n}";
            return ret;
        }
    }
}