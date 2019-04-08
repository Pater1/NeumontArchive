using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapCommandInterpreter: Interpreter<Map> {
    public class AddConnectionCommand: Command<Map> {
        public float distance;
        public string cityA, cityB;

        public static AddConnectionCommand FromSentance(GrammarTreeNode<string> sentance) {
            float dist = float.Parse(sentance.Where(x => x.componentType == GrammarComponent.Number).First().leafValue);
            string[] cities = sentance.Where(x => x.componentType == GrammarComponent.City).Take(2).Select(x => x.leafValue).ToArray();
            return new AddConnectionCommand(dist, cities[0], cities[1]);
        }

        public AddConnectionCommand(float distance, string cityA, string cityB) {
            this.distance = distance;
            this.cityA = cityA;
            this.cityB = cityB;
        }

        public void Execute(Map on) {
            on.AddConnection(cityA, cityB, distance);
            on.Graph();
        }
    }
    public class PathFindCommand: Command<Map> {
        public string cityFrom, cityTo;
        public string[] citiesThrough;

        public static PathFindCommand FromSentance(GrammarTreeNode<string> sentance) {
            GrammarTreeNode<string> from, to, through;
            from = sentance.Where(x => x.componentType == GrammarComponent.PathCommand && x.Values.Contains("from")).FirstOrDefault();
            to = sentance.Where(x => x.componentType == GrammarComponent.PathCommand && x.Values.Contains("to")).FirstOrDefault();
            through = sentance.Where(x => x.componentType == GrammarComponent.PathCommand && x.Values.Contains("through")).FirstOrDefault();

            string cityFrom, cityTo;
            string[] citiesThrough;

            if(from != null) {
                cityFrom = from.Where(x => x.componentType == GrammarComponent.City).Select(x => x.leafValue).FirstOrDefault();
            } else {
                cityFrom = null;
            }
            if(to != null) {
                cityTo = to.Where(x => x.componentType == GrammarComponent.City).Select(x => x.leafValue).FirstOrDefault();
            } else {
                cityTo = null;
            }
            if(through != null) {
                citiesThrough = through.Where(x => x.componentType == GrammarComponent.City).Select(x => x.leafValue).ToArray();
            } else {
                citiesThrough = new string[0];
            }

            return new PathFindCommand(cityFrom != null ? cityFrom : "", cityTo != null ? cityTo : "", citiesThrough);
        }

        public PathFindCommand(string cityFrom, string cityTo, string[] citiesThrough) {
            this.cityFrom = cityFrom;
            this.cityTo = cityTo;
            this.citiesThrough = citiesThrough;
        }

        public void Execute(Map on) {
            on.PathThrough(this);
        }
    }
    public override Command<Map> Interpret(GrammarTreeNode<string> sentance) {
        if(sentance.componentType == GrammarComponent.ConnectionCommand){
            return AddConnectionCommand.FromSentance(sentance);
        }else if(sentance.componentType == GrammarComponent.PathFindCommand){
            return PathFindCommand.FromSentance(sentance);
        }
        return null;
    }
}