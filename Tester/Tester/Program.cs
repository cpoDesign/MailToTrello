using System;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting service");
            Service.Core.Service.Process();
            Console.WriteLine("Service has completed processing");
            Console.ReadKey();
        }
    }
}
