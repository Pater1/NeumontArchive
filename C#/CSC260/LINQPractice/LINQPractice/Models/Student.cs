using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LINQPractice.Models {
    public class Student {
        private static Random rand = new Random();
        public static Student[] s_DefaultStudents { get; } = new Student[] {
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Norma",
                _NameLast = "Lamar",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            },
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Daisy",
                _NameLast = "Humphrey",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            },
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Camilla",
                _NameLast = "Albinson",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            },
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Durward",
                _NameLast = "Horn",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            },
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Prosper",
                _NameLast = "Plank",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            },
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Lewis",
                _NameLast = "Peters",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            },
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Lincoln",
                _NameLast = "Read",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            },
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Aurora",
                _NameLast = "Tolbert",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            },
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Louise",
                _NameLast = "Daniell",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            },
            new Student(){
                _ID = rand.Next(),
                _NameFirst = "Gaylord",
                _NameLast = "Sutton",
                _Age = (byte)rand.Next(12, 25),
                _Gender = (Gender)rand.Next(0, 3),
            }
        };

        public int _ID { get; private set; }
        public string _NameFirst { get; set; }
        public string _NameLast { get; set; }
        public byte _Age { get; set; }
        public Gender _Gender { get; set; }
        public enum Gender {
            Male,
            Female,
            Other
        }
    }
}