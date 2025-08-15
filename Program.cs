using Assignment3.Q1;
using Assignment3.Q2;
using Assignment3.Q3;

namespace Assignment3
{
    public static class Program
    {
        public static void Main()
        {
            // new FinanceApp().Run();
            // new HealthSystemApp().Run();
            new WareHouseManager().Run();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
