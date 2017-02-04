using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Service
{
    class Program
    {

        static void Main(string[] args)
        {
                String command;
                Boolean quitNow = false;
                Console.Clear();
                while (!quitNow)
                {
                    Console.Write(">");
                    command = Console.ReadLine();
                    Console.WriteLine(command);
                    switch (command)
                    {
                        case "help":
                            Console.WriteLine("This should be help.");
                            break;

                        case "/version":
                            Console.WriteLine("This should be version.");
                            break;

                        case "quit":
                            quitNow = true;
                            break;

                        default:
                            Console.WriteLine("Unknown Command " + command);
                            break;
                    }
                }
        }
    }
}
