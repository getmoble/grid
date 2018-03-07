using Grid.Infrastructure;
using System;

namespace GridTestConsole
{
    public class Program
    {
       public static void Main(string[] args)
        {
            var password= HashHelper.Hash("123456");
            if(password != null)
            {

            }
            Console.Read();
        }
    }
}
