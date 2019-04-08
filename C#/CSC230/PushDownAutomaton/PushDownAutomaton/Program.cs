using PushDownAutomaton.GrammarTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PushDownAutomaton {
    class Program {
        private static Dictionary<string, GrammarComponent> vocabulary = new Dictionary<string, GrammarComponent>() {
            #region prepositions
            { "with", GrammarComponent.Preposition },
            { "at", GrammarComponent.Preposition },
            { "from", GrammarComponent.Preposition },
            { "into", GrammarComponent.Preposition },
            { "during", GrammarComponent.Preposition },
            { "including", GrammarComponent.Preposition },
            { "until", GrammarComponent.Preposition },
            { "against", GrammarComponent.Preposition },
            { "among", GrammarComponent.Preposition },
            { "throughout", GrammarComponent.Preposition },
            { "despite", GrammarComponent.Preposition },
            { "towards", GrammarComponent.Preposition },
            { "upon", GrammarComponent.Preposition },
            { "concerning", GrammarComponent.Preposition },
            { "of", GrammarComponent.Preposition },
            { "to", GrammarComponent.Preposition /*| GrammarComponent.Adverb*/ },
            { "in", GrammarComponent.Preposition /*| GrammarComponent.Adverb*/ },
            { "for", GrammarComponent.Preposition /*| GrammarComponent.Conjunction*/ },
            { "on", GrammarComponent.Preposition /*| GrammarComponent.Adverb | GrammarComponent.Adjective*/ },
            { "by", GrammarComponent.Preposition /*| GrammarComponent.Adverb*/ },
            { "about", GrammarComponent.Preposition /*| GrammarComponent.Adverb | GrammarComponent.Adjective*/ },
            { "like", /*GrammarComponent.Preposition |*/ GrammarComponent.Verb /*| GrammarComponent.Conjunction*/ },
            { "through", GrammarComponent.Preposition /*| GrammarComponent.Adverb | GrammarComponent.Adjective*/ },
            { "over", GrammarComponent.Preposition /*| GrammarComponent.Adjective | GrammarComponent.Noun*/ },
            { "before", GrammarComponent.Preposition /*| GrammarComponent.Adverb | GrammarComponent.Conjunction*/ },
            { "between", GrammarComponent.Preposition /*| GrammarComponent.Adverb*/ },
            { "after", GrammarComponent.Preposition /*| GrammarComponent.Adjective | GrammarComponent.Adverb*/ },
            { "since", GrammarComponent.Preposition /*| GrammarComponent.Adverb | GrammarComponent.Conjunction*/ },
            { "without", GrammarComponent.Preposition /*| GrammarComponent.Adverb | GrammarComponent.Conjunction*/ },
            { "under", GrammarComponent.Preposition /*| GrammarComponent.Adverb | GrammarComponent.Adjective*/ },
            { "within", GrammarComponent.Preposition /*| GrammarComponent.Adverb*/ },
            { "along", GrammarComponent.Preposition /*| GrammarComponent.Adverb*/ },
            { "following", GrammarComponent.Preposition /*| GrammarComponent.Noun | GrammarComponent.Adjective*/ },
            { "across", GrammarComponent.Preposition /*| GrammarComponent.Adverb | GrammarComponent.Adjective*/ },
            { "behind", GrammarComponent.Preposition /*| GrammarComponent.Adverb | GrammarComponent.Adjective*/ },
            { "beyond", GrammarComponent.Preposition/* | GrammarComponent.Noun*/ },
            { "plus", GrammarComponent.Preposition /*| GrammarComponent.Adjective | GrammarComponent.Noun*/ },
            { "except", GrammarComponent.Preposition /*| GrammarComponent.Conjunction*/ },
            { "but", /*GrammarComponent.Conjunction |*/ GrammarComponent.Preposition /*| GrammarComponent.Adverb*/ },
            { "up", /*GrammarComponent.Adverb |*/ GrammarComponent.Preposition /*| GrammarComponent.Adjective*/ },
            { "out", /*GrammarComponent.Adverb |*/ GrammarComponent.Preposition /*| GrammarComponent.Adjective*/ },
            { "around", /*GrammarComponent.Adverb |*/ GrammarComponent.Preposition },
            { "down", /*GrammarComponent.Adverb | */GrammarComponent.Preposition /*| GrammarComponent.Adjective*/ },
            { "off", /*GrammarComponent.Adverb | */GrammarComponent.Preposition /*| GrammarComponent.Adjective*/ },
            { "above", /*GrammarComponent.Adverb | */GrammarComponent.Preposition /*| GrammarComponent.Adjective*/ },
            { "near",/* GrammarComponent.Adverb |*/ GrammarComponent.Preposition /*| GrammarComponent.Adjective*/ },
            #endregion
            
            #region Pronouns
            {"i", GrammarComponent.Pronoun },
            {"you", GrammarComponent.Pronoun },
            {"he", GrammarComponent.Pronoun },
            {"she", GrammarComponent.Pronoun },
            {"it", GrammarComponent.Pronoun },
            {"we", GrammarComponent.Pronoun },
            {"they", GrammarComponent.Pronoun },
            #endregion

            #region Nouns
            {"dog", GrammarComponent.Noun },
            {"cat", GrammarComponent.Noun },
            {"time", GrammarComponent.Noun },
            {"year", GrammarComponent.Noun },
            {"people", GrammarComponent.Noun },
            {"way", GrammarComponent.Noun },
            {"day", GrammarComponent.Noun },
            {"man", GrammarComponent.Noun },
            {"thing", GrammarComponent.Noun },
            {"woman", GrammarComponent.Noun },
            {"life", GrammarComponent.Noun },
            {"child", GrammarComponent.Noun },
            {"world", GrammarComponent.Noun },
            {"school", GrammarComponent.Noun },
            {"state", GrammarComponent.Noun },
            {"family", GrammarComponent.Noun },
            {"student", GrammarComponent.Noun },
            {"group", GrammarComponent.Noun },
            {"country", GrammarComponent.Noun },
            {"problem", GrammarComponent.Noun },
            {"hand", GrammarComponent.Noun },
            {"part", GrammarComponent.Noun },
            {"place", GrammarComponent.Noun },
            {"case", GrammarComponent.Noun },
            {"week", GrammarComponent.Noun },
            {"company", GrammarComponent.Noun },
            {"system", GrammarComponent.Noun },
            {"program", GrammarComponent.Noun },
            {"question", GrammarComponent.Noun },
            {"work", GrammarComponent.Noun /*| GrammarComponent.Verb*/ },
            {"government", GrammarComponent.Noun },
            {"number", GrammarComponent.Noun },
            {"night", GrammarComponent.Noun },
            {"point", GrammarComponent.Noun },
            {"home", GrammarComponent.Noun },
            {"water", GrammarComponent.Noun },
            #endregion

            #region Verbs
            {"chase", GrammarComponent.Verb },
            {"be", GrammarComponent.Verb },
            {"have", GrammarComponent.Verb },
            {"do", GrammarComponent.Verb },
            {"say", GrammarComponent.Verb },
            {"go", GrammarComponent.Verb },
            {"get", GrammarComponent.Verb },
            {"make", GrammarComponent.Verb },
            {"know", GrammarComponent.Verb },
            {"think ", GrammarComponent.Verb },
            {"take", GrammarComponent.Verb },
            {"see", GrammarComponent.Verb },
            {"come", GrammarComponent.Verb },
            {"want", GrammarComponent.Verb },
            {"look", GrammarComponent.Verb },
            {"use", GrammarComponent.Verb },
            {"find", GrammarComponent.Verb },
            {"give", GrammarComponent.Verb },
            {"tell", GrammarComponent.Verb },
            {"call", GrammarComponent.Verb },
            {"try", GrammarComponent.Verb },
            {"ask", GrammarComponent.Verb },
            {"need", GrammarComponent.Verb },
            {"feel", GrammarComponent.Verb },
            {"become", GrammarComponent.Verb },

            {"is", GrammarComponent.Verb },
            {"am", GrammarComponent.Verb },
            {"are", GrammarComponent.Verb },
            #endregion
            
            {"the", GrammarComponent.Adjective },
            {"cool", GrammarComponent.Adjective },
            {"because", GrammarComponent.Preposition },
            {"yes", GrammarComponent.Sentence },
        };
        private static GrammarComponentMapping[] mapping = new GrammarComponentMapping[]{
            new GrammarComponentMapping(GrammarComponent.Sentence, new GrammarComponent[]{ GrammarComponent.NounPhrase, GrammarComponent.VerbPhrase }),

            new GrammarComponentMapping(GrammarComponent.Noun, new GrammarComponent[]{ GrammarComponent.Adjective, GrammarComponent.Noun }),
            new GrammarComponentMapping(GrammarComponent.NounPhrase, new GrammarComponent[]{ GrammarComponent.Noun }),
            new GrammarComponentMapping(GrammarComponent.NounPhrase, new GrammarComponent[]{ GrammarComponent.Pronoun }),

            new GrammarComponentMapping(GrammarComponent.PrepositionalPhrase, new GrammarComponent[]{ GrammarComponent.Preposition, GrammarComponent.NounPhrase }),
            new GrammarComponentMapping(GrammarComponent.PrepositionalPhrase, new GrammarComponent[]{ GrammarComponent.Preposition, GrammarComponent.Sentence }),

            new GrammarComponentMapping(GrammarComponent.Verb, new GrammarComponent[]{ GrammarComponent.Adverb, GrammarComponent.Verb }),
            new GrammarComponentMapping(GrammarComponent.VerbPhrase, new GrammarComponent[]{ GrammarComponent.Verb }),
            new GrammarComponentMapping(GrammarComponent.VerbPhrase, new GrammarComponent[]{ GrammarComponent.Verb, GrammarComponent.NounPhrase }),
        };
        private static List<VocabularyResponceMapping<string>> WordMap = new List<VocabularyResponceMapping<string>>() {
            new VocabularyResponceMapping<string>(new GrammarComponent[]{ }, "i", "you"),
            new VocabularyResponceMapping<string>(new GrammarComponent[]{ GrammarComponent.NounPhrase}, "you", "i"),
            new VocabularyResponceMapping<string>(new GrammarComponent[]{ GrammarComponent.VerbPhrase, GrammarComponent.NounPhrase}, "you", "me"),
        };
        static void Main(string[] args) {
            string input = "";
            while(input != "exit"){
                input = Console.ReadLine().ToLower();
                try {
                    IEnumerable<string> split = StringSplitStateMachine.SplitOut(input);

                    IEnumerable<GrammarTreeNode<string>> leaves = new Vocabulary<string>(vocabulary).Translate(split);

                    GrammarTreeNode<string> parsed = new PushDownParser<string>(mapping).Parse(leaves);

                    Console.WriteLine($"{new Replier(WordMap).Reply(parsed)}");
                }catch {
                    Console.WriteLine($"Sorry, I don't understand...");
                }
            }
            Console.WriteLine($"Bye!");
        }
    }
}
