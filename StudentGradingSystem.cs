using System;
using System.Collections.Generic;
using System.IO;

namespace Assignment3.Q4
{
    // a) Student class
    public class Student
    {
        public int Id;
        public string FullName;
        public int Score;

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100) return "A";
            if (Score >= 70 && Score <= 79) return "B";
            if (Score >= 60 && Score <= 69) return "C";
            if (Score >= 50 && Score <= 59) return "D";
            return "F";
        }

        public override string ToString() =>
            $"{FullName} (ID: {Id}): Score = {Score}, Grade = {GetGrade()}";
    }

    // b) Custom exceptions
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // d) StudentResultProcessor
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using var reader = new StreamReader(inputFilePath);
            string? line;
            int lineNumber = 1;

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');

                if (parts.Length != 3)
                    throw new MissingFieldException($"Line {lineNumber}: Missing fields.");

                if (!int.TryParse(parts[0].Trim(), out int id))
                    throw new FormatException($"Line {lineNumber}: Invalid ID format.");

                string name = parts[1].Trim();

                if (!int.TryParse(parts[2].Trim(), out int score))
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid score format.");

                students.Add(new Student(id, name, score));
                lineNumber++;
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using var writer = new StreamWriter(outputFilePath);
            foreach (var student in students)
            {
                writer.WriteLine(student.ToString());
            }
        }

        public void Run()
        {
            try
            {
                string inputFile = "students.txt";     // Input file in project folder
                string outputFile = "report.txt";      // Output file in project folder

                var students = ReadStudentsFromFile(inputFile);
                WriteReportToFile(students, outputFile);

                Console.WriteLine("\nReport generated successfully. Contents:");
                foreach (var student in students)
                    Console.WriteLine(student);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"FileNotFoundException: {ex.Message}");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"InvalidScoreFormatException: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"MissingFieldException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
