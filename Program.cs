using System;
using Assignment3.Q1;
using Assignment3.Q2;
using Assignment3.Q3;
using Assignment3.Q4;
using Assignment3.Q5;

namespace Assignment3
{
    public static class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Assignment 3 Menu =====");
                Console.WriteLine("1. Question 1 - Finance Management System");
                Console.WriteLine("2. Question 2 - Healthcare System");
                Console.WriteLine("3. Question 3 - Warehouse Inventory Management");
                Console.WriteLine("4. Question 4 - School Grading System");
                Console.WriteLine("5. Question 5 - Inventory Records System");
                Console.WriteLine("0. Exit");
                Console.Write("\nSelect an option: ");

                string? choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        new FinanceApp().Run();
                        break;
                    case "2":
                        new HealthSystemApp().Run();
                        break;
                    case "3":
                        new WareHouseManager().Run();
                        break;
                    case "4":
                        new StudentResultProcessor().Run();
                        break;
                    case "5":
                        new InventoryApp().Run();
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to return to the menu...");
                Console.ReadKey();
            }
        }
    }
}
