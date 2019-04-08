using AlgoDataStructures;
using System;
using System.Collections.Generic;

namespace MazeSolver {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Please input the filepath to a maze:" + Environment.NewLine);

            string filePath = Console.ReadLine();
            //string filePath = "E:\\Neumont\\Algo2\\MazeSolver\\MazeTest.txt";
            Graph<string>[] graphs = Graph<string>.ParseFile(filePath);

            foreach(Graph<string> g in graphs){
                IEnumerable<string> path;
                if(g.TryMazeSolve(out path)){
                    foreach(string s in path){
                        Console.Write(s);
                    }
                    Console.WriteLine();
                } else{
                    Console.WriteLine("Maze cannot be solved");
                }
            }

            Console.ReadLine();
        }
    }
}