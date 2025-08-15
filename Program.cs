using Assignment3.Q1;
using Assignment3.Q2;
using Assignment3.Q3;

namespace Assignment3
{
    public static class Program
    {
        public static void Main()
        {
            //new FinanceApp().Run();   // Run Question 1
            //new HealthSystemApp().Run(); // For Q2
            new WareHouseManager().Run();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
