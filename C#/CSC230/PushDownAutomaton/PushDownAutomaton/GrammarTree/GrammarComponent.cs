using System;
using System.Collections.Generic;
using System.Text;

namespace PushDownAutomaton.GrammarTree {
    [Flags]
    public enum GrammarComponent: int {
        //single word components
        Noun = 1 << 0,
        Pronoun = 1 << 1,
        Adjective = 1 << 2,
        Verb = 1 << 3,
        Adverb = 1 << 4,
        Preposition = 1 << 5,
        Conjunction = 1 << 6,
        Interjection = 1 << 7,

        //Phrases
        NounPhrase = 1 << 8,
        PrepositionalPhrase = 1 << 9,
        AdjectivePhrase = 1 << 10,
        AdverbPhrase = 1 << 11,
        VerbPhrase = 1 << 12,
        InfinitivePhrase = 1 << 13,

        //Clauses
        NounClause = 1 << 14,
        AdjectiveClause = 1 << 15,
        AdverbClause = 1 << 16,

        Sentence = 1 << 17 //a.k.a. Dependent Clause
    }
}
