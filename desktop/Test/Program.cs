using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            User user = new User();
            user.email = "admin";

            Console.WriteLine(user.getToken("admin"));
            Console.ReadKey();
        }
    }
}
